using System;
using System.Collections.Generic;
using System.IO;

namespace util
{
    public class BytesListBuffer : Stream
    {
        public long BlockSize = 64 * 1024;
        public List<byte[]> Blocks = new List<byte[]>();

        public Func<int, bool> BeforeWrite;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        long _length = 0;
        public override long Length => _length;

        long _position = 0;
        public override long Position
        {
            get => _position;
            set => _position = value;
        }

        protected byte[] block
        {
            get
            {
                while (Blocks.Count <= blockIdx)
                {
                    Blocks.Add(new byte[BlockSize]);
                }
                return Blocks[(int)blockIdx];
            }
        }
        protected long blockIdx => Position / BlockSize;
        protected long blockPos => Position % BlockSize;

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            count = (int)Math.Min(_length - Position, count);

            int totalRead = 0;
            int copySize = 0;
            while (count > 0)
            {
                copySize = (int)Math.Min(count, BlockSize - blockPos);
                Buffer.BlockCopy(block, (int)blockPos, buffer, offset, copySize);

                Position += copySize;
                count -= copySize;
                offset += copySize;
                totalRead += copySize;
            }
            return totalRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (BeforeWrite?.Invoke(count) == false)
                return;

            long beginPos = Position;
            int copySize;
            try
            {
                while (count > 0)
                {
                    copySize = Math.Min(count, (int)(BlockSize - blockPos));
                    Buffer.BlockCopy(buffer, offset, block, (int)blockPos, copySize);

                    _length = Math.Max(_length, Position + copySize);
                    Position += copySize;

                    count -= copySize;
                    offset += copySize;
                }
            }
            catch
            {
                Position = beginPos;
                throw;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length - offset;
                    break;
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public byte[] ToArray()
        {
            long pos = Position;
            Position = 0;
            byte[] dst = new byte[Length];
            Read(dst, 0, (int)Length);
            Position = pos;
            return dst;
        }

        public void ReadFrom(Stream rd, long count)
        {
            byte[] buff = new byte[4096];
            int read;
            do
            {
                read = rd.Read(buff, 0, (int)Math.Min(4096, count));
                count -= read;
                this.Write(buff, 0, read);
            }
            while (count > 0);
        }

        public void WriteTo(Stream wrt)
        {
            long pos = Position;
            Position = 0;
            this.CopyTo(wrt);
            Position = pos;
        }

        //void check(byte[] buffer, int offset, int count)
        //{
        //    if (count < 0)
        //        throw new ArgumentOutOfRangeException();
        //    if (buffer == null)
        //        throw new ArgumentNullException();
        //    if (offset < 0)
        //        throw new ArgumentOutOfRangeException();
        //}
    }
}
