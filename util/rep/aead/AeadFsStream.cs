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

        const int BuffSize = (int)(2 * Number.MB);

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
            => getDataSize(fileSize - getHeadSize(conf),
                conf.packSize(), conf.BlockSize, conf.tagSize());

        static long getDataSize(long packTotal,
            int packSize, int blockSize, int tagSize)
            => (packTotal / packSize) * blockSize
            + ((packTotal % packSize) - tagSize).atLeast(0);

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
        }

        long counter;
        int tagSize;
        int blockSize;
        int packSize;
        int unitLimit;
        void init()
        {
            counter = nonce.i64(nonce.Length - 8);
            tagSize = conf.tagSize();
            blockSize = conf.BlockSize;
            packSize = blockSize + tagSize;
            unitLimit = (BuffSize / blockSize)
                    .atLeast(1) * blockSize;
        }

        long streamLen() => getDataSize(fs.Length - headSize,
                            packSize, blockSize, tagSize);

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
            buffLen = readFile(buffIdx, buff, buffLen);
        }

        void writeBuff()
        {
            if (buffPos <= 0)
                return;
            fs.write(filePos(buffIdx), buff, 0, buffPos);
        }

        long filePos(long packIdx)
            => headSize + packIdx * packSize;

        int readFile(long packIdx, byte[] dst, int count)
            => fs.readFull(filePos(packIdx), dst, 0, count);

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
            return total.readByUnit(unitLimit,
                unit => readDecrypt(dst, offset, unit),
                actual => offset += actual);
        }

        int readDecrypt(byte[] dst, int offset, int total)
        {
            actionPos = streamPos;
            var actionLen = streamLen();

            readBuff(total);
            int remain = total;
            int dataLen, readLen;
            while (remain > 0)
            {
                if (actionPos >= actionLen)
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
            return total - remain;
        }

        public override void Write(byte[] src, int offset, int total)
        {
            total.writeByUnit(unitLimit, actual => 
            {
                encryptWrite(src, offset, actual);
                offset += actual;
            });
        }

        void encryptWrite(byte[] src, int offset, int count)
        {
            actionPos = streamPos;
            var actionLen = streamLen();

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
                    var dataLen = readFile(blockIdx, pack, pack.Length) - tagSize;
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
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var pos = streamPos;

            if (origin == SeekOrigin.Begin)
                pos = offset;
            else if (origin == SeekOrigin.Current)
                pos += offset;
            else if (origin == SeekOrigin.End)
                pos = streamLen() - offset;

            return Position = pos;
        }

        public override void SetLength(long len)
        {
            if (len < 0)
                throwIOError("NegativeLength", len);

            var oldPos = streamPos;
            try
            {
                var delta = len - streamLen();
                if (delta > 0)
                {
                    // front padding range
                    var frontPad = (blockSize % (blockSize - (streamLen() % blockSize)))
                                    .atMost(delta);
                    this.append(frontPad);
                    // zero spare pack range
                    var spareCount = (len - streamLen()) / blockSize;
                    if (spareCount > 0)
                    {
                        // for inner non-zero padding file system
                        var spareBuff = new byte[spareCount.atMost(BuffSize / packSize)
                                                .atLeast(1) * packSize];
                        (spareCount* packSize).writeByUnit(spareBuff.Length, unit
                              => fs.append(spareBuff, 0, unit));
                    }
                    // last padding range
                    this.append(len - streamLen());
                }
                else if (delta < 0)
                {
                    var lastPack = len / blockSize;
                    // read last block tail data
                    var tailBuff = this.readExactRange(lastPack * blockSize, len);
                    // truncate to last block start pos
                    fs.SetLength(filePos(lastPack));
                    // write back last block tail
                    this.append(tailBuff);
                }
            }
            finally
            {
                streamPos = oldPos.atMost(streamLen());
            }
        }

        public override bool CanRead => fs.CanRead;
        public override bool CanSeek => fs.CanSeek;
        public override bool CanWrite => fs.CanWrite;
        //long streamLen = 0;
        public override long Length => streamLen();
        long streamPos = 0;
        public override long Position
        {
            get => streamPos;
            set
            {
                if (value < 0 || value > streamLen())
                    throwIOError("OutOfRange", value, streamLen());
                streamPos = value;
            }
        }
        public override void Flush() => fs.Flush();
        public override void Close()
        {
            true.free(ref fs);
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
