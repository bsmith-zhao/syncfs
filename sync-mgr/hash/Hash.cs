using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using util;
using util.ext;

namespace sync.hash
{
    public enum HashType
    {
        SHA1, SHA256, SHA512
    }

    public enum HashMode
    {
        Sample, Full
    }

    public class Hash
    {
        public HashType type = HashType.SHA256;
        public HashMode mode = HashMode.Sample;
        public int block = 1024;
        public int count = 25;
        public TimeSpan expire;

        public override string ToString()
            => this.desc();

        byte[] bf;
        byte[] buff => bf ?? (bf = new byte[1024 * 1024]);

        HashBuff sp;
        HashBuff sample => sp ?? (sp = new HashBuff(block * count));

        public string getSpec()
        {
            if (mode == HashMode.Full)
                return $"type={type},mode={mode}";
            else
                return $"type={type},mode={mode},block={block},count={count}";
        }

        public HashAlgorithm newAlg() => HashAlgorithm.Create(type.ToString());

        public byte[] compute(Stream fin, HashAlgorithm alg, Action<long> update = null)
        {
            if (mode == HashMode.Sample)
            {
                takeSample(fin, update);
                return alg.ComputeHash(sample.data, 0, sample.size);
            }
            else
            {
                alg.Initialize();
                var buff = this.buff;
                int len;
                while (fin.read(buff, out len))
                {
                    alg.TransformBlock(buff, 0, len, null, 0);
                    update?.Invoke(len);
                }
                alg.TransformFinalBlock(buff, 0, 0);
                return alg.Hash;
            }
        }

        void takeSample(Stream fin, Action<long> update)
        {
            var offset = 0;
            sample.reset();
            long total = fin.Length;
            if (total <= sample.total)
            {
                sample.readExact(fin, (int)total, update);
            }
            else
            {
                // file first
                sample.readExact(fin, block, update);
                // file spans
                long span = (total - block * 2) / (count - 2);
                int divs = (int)(span / block);
                var jump = (offset % divs + 1) * block;
                for (int i = 0; i < count - 2; i++)
                {
                    seekToPos(fin, span * i + jump, update);
                    // fin.Position = span * i + delta;
                    sample.readExact(fin, block, update);
                }
                // file last.
                seekToPos(fin, total - block, update);
                // fin.Position = total - block;
                sample.readExact(fin, block, update);
            }
        }

        void seekToPos(Stream fin, long pos, Action<long> update)
        {
            long span = pos - fin.Position;
            if (fin.CanSeek)
            {
                fin.Position = pos;
                update?.Invoke(span);
                return;
            }
            int len = 0;
            while (span > 0 && fin.read(buff, 0, (int)span.min(buff.Length), out len))
            {
                span -= len;
                update?.Invoke(len);
            }
        }

        public class HashBuff
        {
            public byte[] data;
            public int size = 0;
            public int total => data.Length;

            public HashBuff(int total)
            {
                data = new byte[total];
            }

            public HashBuff(byte[] data)
            {
                this.data = data;
            }

            public HashBuff reset()
            {
                size = 0;
                return this;
            }

            public void readExact(Stream fin, int count, Action<long> update)
            {
                fin.readExact(data, size, count);
                update?.Invoke(count);
                size += count;
            }
        }
    }
}
