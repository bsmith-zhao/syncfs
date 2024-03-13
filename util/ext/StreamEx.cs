using util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class StreamEx
    {
        public static void append(this Stream fs,
            byte[] data)
        {
            if (data?.Length > 0)
            {
                fs.Position = fs.Length;
                fs.write(data);
            }
        }

        public static void append(this Stream fs,
            byte[] data, int offset, int count)
        {
            fs.Position = fs.Length;
            fs.Write(data, offset, count);
        }

        public static void extend(this Stream fs,
            long size)
        {
            fs.SetLength(fs.Length + size);
        }

        public static bool readRow(this StreamReader rd, 
                                    out string row)
            => (row = rd.ReadLine()) != null;

        public static void copyTo(this Stream fin, 
                                Stream fout, 
                                Action<long> update, 
                                byte[] buff = null)
        {
            buff = buff ?? new byte[4.mb()];
            int size;
            while (fin.read(buff, out size))
            {
                fout.Write(buff, 0, size);
                update?.Invoke(size);
            }
        }

        public static bool isSame(this Stream src, 
                                Stream dst, 
                                Action<long> update, 
                                ref byte[] srcBuff, 
                                ref byte[] dstBuff, 
                                int buffSize = 256*1024)
        {
            srcBuff = srcBuff ?? new byte[buffSize];
            dstBuff = dstBuff ?? new byte[buffSize];
            int srcLen, dstLen;
            while (true)
            {
                srcLen = src.readFull(srcBuff);
                dstLen = dst.readFull(dstBuff);
                if (srcLen != dstLen)
                    return false;
                if (srcLen <= 0)
                    return true;
                if (!srcBuff.isSame(dstBuff, srcLen))
                    return false;

                update?.Invoke(srcLen);
            }
        }

        public static void write(this Stream fout, 
            long pos, byte[] data, int offset, int count)
        {
            fout.Position = pos;
            fout.Write(data, offset, count);
        }

        public static void write(this Stream fout, byte[] data)
        {
            fout.Write(data, 0, data.Length);
        }

        public static int read(this Stream fin, byte[] data)
            => fin.Read(data, 0, data.Length);

        public static bool read(this Stream fin, 
            byte[] data, out int actual)
            => (actual = fin.Read(data, 0, data.Length)) > 0;

        public static bool read(
            this Stream fin, 
            byte[] data, int offset, int count, 
            out int actual)
            => (actual = fin.Read(data, offset, count)) > 0;

        public static void readExact(this Stream fin, 
            long pos, byte[] data)
        {
            fin.Position = pos;
            readExact(fin, data, 0, data.Length);
        }

        public static byte[] readExact(this Stream fin,
            long pos, long size)
        {
            if (size <= 0)
                return null;
            fin.Position = pos;
            var data = new byte[size];
            readExact(fin, data, 0, data.Length);
            return data;
        }

        public static byte[] readExactRange(this Stream fin,
            long begin, long end)
            => readExact(fin, begin, end - begin);

        public static void readExact(this Stream fin, byte[] data)
        {
            readExact(fin, data, 0, data.Length);
        }

        public static void readExact(this Stream fin, 
            byte[] data, int offset, int total)
        {
            int actual = readFull(fin, data, offset, total);
            if (actual != total)
                throw new IOException(typeof(Stream)
                    .trans("ReadMismatch", total, actual));
        }

        public static int readFull(this Stream fin,
            long pos, byte[] data, int offset, int total)
        {
            fin.Position = pos;
            return readFull(fin, data, offset, total);
        }

        public static int readFull(this Stream fin, byte[] data)
            => readFull(fin, data, 0, data.Length);

        public static bool readFull(this Stream fin, 
            byte[] data, out int actual)
            => (actual = fin.readFull(data, 0, data.Length)) > 0;

        public static int readFull(this Stream fin,
            byte[] dst, int offset, int total)
        {
            int remain = total;
            int actual;
            while (remain > 0
                && (actual = fin.Read(dst, offset, remain)) > 0)
            {
                remain -= actual;
                offset += actual;
            }
            return total - remain;
        }
    }
}
