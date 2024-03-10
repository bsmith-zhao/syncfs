using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;
using util.ext;

namespace util.rep.aead
{
    public class AeadFsStream : Stream
    {
        public const string Type = "aead";
        public const short Version = 1;
        public static byte[] KeyContext = "file-key".utf8();

        FileStream fs;
        AeadFsConf conf;
        int headSize;

        public AeadFsStream(FileStream fs, AeadFsConf conf, bool create)
        {
            try
            {
                this.fs = fs;
                this.conf = conf;
                this.headSize = getHeadSize(conf);

                if (create)
                    this.create();
                else
                    open();

                init();
            }
            catch
            {
                Close();
                throw;
            }
        }

        public static long getDataSize(long fileSize, AeadFsConf conf)
        {
            var packTotal = fileSize - getHeadSize(conf);
            return (packTotal / conf.packSize()) * conf.BlockSize
                    + ((packTotal % conf.packSize()) - conf.tagSize())
                    .atLeast(0);
        }

        public static int getHeadSize(AeadFsConf conf)
            => Type.Length + 2 + conf.FileIdSize + conf.nonceSize();

        byte[] fileId;
        byte[] nonce;
        void create()
        {
            fileId = conf.FileIdSize.aesRnd();
            nonce = conf.nonceSize().aesRnd();

            var header = Type.utf8().merge(Version.bytes(), fileId, nonce);
            fs.write(header);
        }
        void open()
        {
            var header = new byte[headSize];
            if (fs.readFull(header) != header.Length)
                throwIOError("HeaderShort");
            if (header.utf8(0, Type.Length) != Type)
                throwIOError("InvalidType");
            var ver = header.i16(Type.Length);
            if (ver > Version)
                throwIOError("InvalidVersion", ver);

            fileId = header.sub(Type.Length + 2, conf.FileIdSize);
            nonce = header.tail(conf.nonceSize());
            streamLen = getDataSize(fs.Length, conf);
        }

        long counter;
        int tagSize;
        int blockSize;
        int packSize;
        const int MaxBuff = (int)(1 * Number.MB);
        int unitSize;
        void init()
        {
            counter = nonce.i64(nonce.Length - 8);
            tagSize = conf.tagSize();
            blockSize = conf.BlockSize;
            packSize = blockSize + tagSize;
            unitSize = ((MaxBuff / blockSize) * blockSize)
                    .atLeast(blockSize);
        }

        void throwIOError(string key, params object[] args)
            => throw new IOException(this.trans(key, args.append(fs.Name)));

        AeadCrypt _aead;
        AeadCrypt aead => _aead ?? (_aead = conf.newDataCrypt(KeyContext.merge(fileId)));

        byte[] _pack;
        byte[] pack => _pack ?? (_pack = new byte[packSize]);
        
        byte[] _block;
        byte[] block => _block ?? (_block = new byte[blockSize]);

        long actionPos;
        long blockIdx => actionPos / blockSize;
        int blockOff => (int)(actionPos % blockSize);

        byte[] buff;
        int buffLen;
        int buffPos;
        long buffIdx;
        void initBuff(int dataLen)
        {
            buffLen = ((blockOff + dataLen - 1) / blockSize + 1) * packSize;
            if (buff == null || buff.Length < buffLen)
                buff = new byte[buffLen];
            buffPos = 0;
            buffIdx = blockIdx;
        }

        void readBuff(int dataLen)
        {
            initBuff(dataLen);
            buffLen = readFile(buff, buffLen, buffIdx);
        }

        void writeBuff()
        {
            if (buffPos <= 0)
                return;
            seekFile(buffIdx);
            fs.Write(buff, 0, buffPos);
        }

        void seekFile(long packIdx)
            => fs.Position = headSize + packIdx * packSize;

        int readFile(byte[] dst, int dstLen, long packIdx)
        {
            seekFile(packIdx);
            return fs.readFull(dst, 0, dstLen);
        }

        int seekPack()
        {
            buffPos = (int)((blockIdx - buffIdx) * packSize);
            return packSize.atMost(buffLen - buffPos);
        }

        void decryptBuffPack(int dataSize, byte[] dst, int dstOff = 0)
            => decryptPack(buff, buffPos, dataSize + tagSize,
                            blockIdx,
                            dst, dstOff);

        void encryptPackToBuff(byte[] src, int srcOff, int srcLen)
        {
            encryptPack(src, srcOff, srcLen, blockIdx, buff, buffPos);
            buffPos += srcLen + tagSize;
        }

        public override int Read(byte[] dst, int offset, int total)
        {
            return total.readByUnit(unitSize,
                unit => readDecrypt(dst, offset, unit),
                actual => offset += actual);
        }

        int readDecrypt(byte[] dst, int offset, int count)
        {
            actionPos = streamPos;

            readBuff(count);
            int remain = count;
            int dataLen, readLen;
            while (remain > 0)
            {
                if (actionPos >= streamLen)
                    break;
                dataLen = seekPack() - tagSize;
                readLen = remain.atMost(dataLen - blockOff);
                if (readLen <= 0)
                    throwIOError("DataShort",
                                remain,
                                dataLen,
                                blockOff);

                if (blockOff == 0 && dataLen <= remain)
                    decryptBuffPack(dataLen, dst, offset);
                else
                {
                    decryptBuffPack(dataLen, block);
                    Buffer.BlockCopy(block, blockOff, dst, offset, readLen);
                }

                offset += readLen;
                remain -= readLen;

                actionPos += readLen;
            }

            streamPos = actionPos;
            return count - remain;
        }

        public override void Write(byte[] src, int offset, int total)
        {
            total.writeByUnit(unitSize, unit => 
            {
                writeEncrypt(src, offset, unit);
                offset += unit;
            });
        }

        void writeEncrypt(byte[] src, int offset, int count)
        {
            actionPos = streamPos;
            var actionLen = streamLen;

            initBuff(count);
            int writeLen;
            while (count > 0)
            {
                writeLen = count.atMost(blockSize - blockOff);
                if (blockOff == 0
                    && (writeLen == blockSize // overwrite whole block
                        || writeLen >= actionLen - actionPos)) // overwrite exist block
                {
                    encryptPackToBuff(src, offset, writeLen);
                }
                else
                {
                    var dataLen = readFile(pack, pack.Length, blockIdx) - tagSize;
                    if (dataLen != (actionLen - blockIdx * blockSize).atMost(blockSize))
                        throwIOError("DataError",
                                    dataLen, 
                                    actionLen - blockIdx * blockSize);

                    decryptPack(pack, 0, dataLen + tagSize, blockIdx, block);
                    Buffer.BlockCopy(src, offset, block, blockOff, writeLen);

                    encryptPackToBuff(block, 0, dataLen.atLeast(blockOff + writeLen));
                }

                offset += writeLen;
                count -= writeLen;

                actionPos += writeLen;
                actionLen = actionLen.atLeast(actionPos);
            }
            writeBuff();

            streamPos = actionPos;
            streamLen = actionLen;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var pos = streamPos;

            if (origin == SeekOrigin.Begin)
                pos = offset;
            else if (origin == SeekOrigin.Current)
                pos += offset;
            else if (origin == SeekOrigin.End)
                pos = streamLen - offset;

            return Position = pos;
        }

        public override void SetLength(long len)
        {
            if (len < 0)
                throwIOError("NegativeLength", len);

            var oldPos = streamPos;
            try
            {
                if (len > streamLen)
                {
                    // front padding range
                    var frontPad = streamLen % blockSize;
                    if (frontPad > 0)
                    {
                        frontPad = (blockSize - frontPad).atMost(len - streamLen);
                        appendData(new byte[frontPad]);
                    }
                    // zero spare pack range
                    var spareCount = (len - streamLen) / blockSize;
                    if (spareCount > 0)
                    {
                        if (frontPad == 0)
                        {
                            // move inner stream pos to end
                            fs.Position = fs.Length;
                        }
                        var zero = new byte[packSize];
                        while (spareCount-- > 0)
                        {
                            // direct write zero pack to inner stream
                            fs.write(zero);
                            streamLen += blockSize;
                        }
                    }
                    // last padding range
                    var lastPad = len - streamLen;
                    if (lastPad > 0)
                    {
                        appendData(new byte[lastPad]);
                    }
                }
                else if (len < streamLen)
                {
                    var blockCount = len / blockSize;
                    var blockEnd = blockCount * blockSize;
                    var padSize = len - blockEnd;
                    byte[] padBuff = null;
                    if (padSize > 0)
                    {
                        padBuff = new byte[padSize];
                        streamPos = blockEnd;
                        this.readExact(padBuff);
                    }
                    fs.SetLength(headSize + blockCount * packSize);
                    streamLen = blockEnd;
                    if (padBuff != null)
                    {
                        appendData(padBuff);
                    }
                }
            }
            finally
            {
                streamPos = oldPos.atMost(streamLen);
            }
        }

        void appendData(byte[] data)
        {
            streamPos = streamLen;
            Write(data, 0, data.Length);
        }

        public override bool CanRead => fs.CanRead;
        public override bool CanSeek => fs.CanSeek;
        public override bool CanWrite => fs.CanWrite;
        long streamLen = 0;
        public override long Length => streamLen;
        long streamPos = 0;
        public override long Position
        {
            get => streamPos;
            set
            {
                if (value < 0 || value > streamLen)
                    throwIOError("OutOfRange", value, streamLen);
                streamPos = value;
            }
        }
        public override void Flush() => fs.Flush();
        public override void Close()
        {
            this.free(ref fs);
        }

        byte[] mergeNonce(long packIdx)
            => (counter + packIdx).copyTo(nonce, -8);

        byte[] aad = new byte[8];
        byte[] attachData(long packIdx)
            => packIdx.copyTo(aad);

        public void encryptPack(byte[] plain, int plainOff, int plainLen,
                                long packIdx,
                                byte[] cipher, int cipherOff = 0)
        {
            aead.encrypt(plain, plainOff, plainLen,
                        mergeNonce(packIdx),
                        cipher, cipherOff,
                        attachData(packIdx));
        }

        public void decryptPack(byte[] cipher, int cipherOff, int cipherLen,
                                long packIdx,
                                byte[] plain, int plainOff = 0)
        {
            // spare file, whole pack are all zero
            if (cipherLen == packSize
                && cipher.allZero(cipherOff, cipherLen))
            {
                Buffer.BlockCopy(cipher, cipherOff,
                                plain, plainOff,
                                cipherLen - tagSize);
            }
            else if (!aead.decrypt(cipher, cipherOff, cipherLen,
                            mergeNonce(packIdx),
                            plain, plainOff,
                            attachData(packIdx)))
            {
                throwIOError("VerifyFail", packIdx);
            }
        }
    }
}
