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
        public static byte[] FileKeyDomain = "file-key".utf8();

        public Stream fs;
        public AeadFsConf conf;

        public static long dataSize(long fileSize, AeadFsConf conf)
        {
            var packTotal = fileSize - headSize(conf);
            return (packTotal / conf.packSize()) * conf.BlockSize
                    + ((packTotal % conf.packSize()) - conf.tagSize()).max(0);
        }

        public static int headSize(AeadFsConf conf)
            => Type.Length + 2 + conf.FileIdSize + conf.nonceSize();

        byte[] fileId;
        byte[] nonce;
        public AeadFsStream create()
        {
            fileId = conf.FileIdSize.aesRnd();
            nonce = aeadEnc.NonceSize.aesRnd();

            var header = Type.utf8().merge(Version.bytes(), fileId, nonce);
            fs.write(header);

            return this;
        }

        public AeadFsStream open()
        {
            var header = new byte[prefix];
            if (fs.readFull(header) != header.Length
                || header.utf8(0, Type.Length) != Type)
                throw new Error(this, "InvalidType");
            var ver = header.i16(Type.Length);
            if (ver > Version)
                throw new Error(this, "InvalidVersion", ver);

            fileId = header.sub(Type.Length + 2, conf.FileIdSize);
            nonce = header.tail(aeadEnc.NonceSize);
            streamLen = dataSize(fs.Length, conf);

            return this;
        }

        int? pre;
        int prefix => (int)(pre ?? (pre = headSize(conf)));
        int packSize => blockSize + tagSize;
        int blockSize => conf.BlockSize;
        int tagSize => aeadEnc.TagSize;

        AeadCrypt ae;
        AeadCrypt aeadEnc => ae ?? (ae = conf.newCrypt());
        PackCrypt pke;
        PackCrypt packEnc => pke ?? (pke = new PackCrypt(aeadEnc.setKey(conf.deriveEncKey(FileKeyDomain.merge(fileId), aeadEnc.KeySize)), nonce));

        long actionPos;

        byte[] pk;
        byte[] pack => pk ?? (pk = new byte[packSize]);
        
        byte[] blk;
        byte[] block => blk ?? (blk = new byte[blockSize]);

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
            => fs.Position = prefix + packIdx * packSize;

        int readFile(byte[] dst, int dstLen, long packIdx)
        {
            seekFile(packIdx);
            return fs.readFull(dst, 0, dstLen);
        }

        int seekPack()
        {
            buffPos = (int)((blockIdx - buffIdx) * packSize);
            return packSize.min(buffLen - buffPos);
        }

        void decryptPack(int dataSize, byte[] dst, int dstOff = 0)
            => packEnc.decrypt(buff, buffPos, dataSize + tagSize, blockIdx, dst, dstOff);

        void encryptPack(byte[] src, int srcOff, int srcLen)
        {
            packEnc.encrypt(src, srcOff, srcLen, blockIdx, buff, buffPos);
            buffPos += srcLen + tagSize;
        }

        public override int Read(byte[] dst, int offset, int count)
        {
            actionPos = streamPos;

            readBuff(count);
            int remain = count, dataLen, readLen;
            while (remain > 0)
            {
                if (actionPos >= streamLen)
                    break;
                dataLen = seekPack() - tagSize;
                readLen = remain.min(dataLen - blockOff);
                if (readLen <= 0)
                    throw new Error(this, "DataShort", remain, dataLen, blockOff);

                if (blockOff == 0 && dataLen <= remain)
                    decryptPack(dataLen, dst, offset);
                else
                {
                    decryptPack(dataLen, block);
                    Buffer.BlockCopy(block, blockOff, dst, offset, readLen);
                }

                offset += readLen;
                remain -= readLen;

                actionPos += readLen;
            }

            streamPos = actionPos;
            return count - remain;
        }

        public override void Write(byte[] src, int offset, int count)
        {
            actionPos = streamPos;
            var actionLen = streamLen;

            initBuff(count);
            int writeLen;
            while (count > 0)
            {
                writeLen = count.min(blockSize - blockOff);
                if (blockOff == 0
                    && (writeLen == blockSize // overwrite whole block
                        || writeLen >= actionLen - actionPos)) // overwrite exist block
                {
                    encryptPack(src, offset, writeLen);
                }
                else
                {
                    var dataLen = readFile(pack, pack.Length, blockIdx) - tagSize;
                    if (dataLen != (actionLen - blockIdx * blockSize).min(blockSize))
                        throw new Error(this, "DataError", 
                                    dataLen, 
                                    actionLen - blockIdx * blockSize);

                    packEnc.decrypt(pack, 0, dataLen + tagSize, blockIdx, block);
                    Buffer.BlockCopy(src, offset, block, blockOff, writeLen);

                    encryptPack(block, 0, dataLen.max(blockOff + writeLen));
                }

                offset += writeLen;
                count -= writeLen;

                actionPos += writeLen;
                actionLen = actionLen.max(actionPos);
            }
            writeBuff();

            streamPos = actionPos;
            streamLen = actionLen;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var pos = streamPos;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    pos = offset;
                    break;
                case SeekOrigin.Current:
                    pos += offset;
                    break;
                case SeekOrigin.End:
                    pos = streamLen - offset;
                    break;
            }
            return Position = pos;
        }

        public override void SetLength(long value)
        {
            if (value == 0)
            {
                var fsLen = headSize(conf);
                fs.SetLength(fsLen);
                fs.Position = fsLen;
                streamLen = 0;
                streamPos = 0;
            }
            else
                throw new Error(this, "UnsupportLength", value);
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
                if (value < 0 || value > Length)
                    throw new Error(this, "OutOfRange", value, Length);
                streamPos = value;
            }
        }
        public override void Flush() => fs.Flush();
        public override void Close()
        {
            fs?.Close();
            fs = null;
        }
    }
}
