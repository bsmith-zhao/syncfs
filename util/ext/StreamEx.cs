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
            out int len)
            => (len = fin.Read(data, offset, count)) > 0;

        public static void readExact(this Stream fin, byte[] data)
        {
            readExact(fin, data, 0, data.Length);
        }

        public static void readExact(this Stream fin, 
            byte[] data, int offset, int count)
        {
            int actual = readFull(fin, data, offset, count);
            if (actual != count)
                throw new IOException(typeof(Stream)
                    .trans("ReadMismatch", count, actual));
        }

        public static int readFull(this Stream fin, byte[] data)
            => readFull(fin, data, 0, data.Length);

        public static bool readFull(this Stream fin, 
            byte[] data, out int actual)
            => (actual = fin.readFull(data, 0, data.Length)) > 0;

        public static int readFull(this Stream fin,
            byte[] dst, int offset, int count)
            => dst.readFull(offset, count, fin.Read);

        public static int readFull(this byte[] dst,
            int offset, int count,
            Func<byte[], int, int, int> read)
        {
            int remain = count;
            int len;
            while (remain > 0
                && (len = read(dst, offset, remain)) > 0)
            {
                remain -= len;
                offset += len;
            }
            return count - remain;
        }

        public static int readByUnit(this int total, int unit,
            Func<int, int> read, Action<int> proc)
        {
            int remain = total;
            int actual;
            while (remain > 0
                && (actual = read(remain.atMost(unit))) > 0)
            {
                proc?.Invoke(actual);
                remain -= actual;
            }
            return total - remain;
        }

        public static void writeByUnit(this int total, int unit,
            Action<int> write)
        {
            int actual;
            while (total > 0)
            {
                actual = total.atMost(unit);
                write(actual);
                total -= actual;
            }
        }
    }
}
