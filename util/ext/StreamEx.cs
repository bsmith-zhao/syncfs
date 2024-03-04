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

        public static bool isSame(this Stream srcIn, 
                                Stream dstIn, 
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
                srcLen = srcIn.readFull(srcBuff);
                dstLen = dstIn.readFull(dstBuff);
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

        public static bool read(this Stream fin, byte[] data, out int len)
            => (len = fin.Read(data, 0, data.Length)) > 0;

        public static bool read(
            this Stream fin, 
            byte[] data, int offset, int count, 
            out int len)
            => (len = fin.Read(data, offset, count)) > 0;

        public static void readExact(this Stream fin, byte[] data)
        {
            readExact(fin, data, 0, data.Length);
        }

        public static void readExact(this Stream fin, byte[] data, int offset, int count)
        {
            int actual = readFull(fin, data, offset, count);
            if (actual != count)
                throw new Error(typeof(Stream), 
                    "ReadMismatch", count, actual);
        }

        public static int readFull(this Stream fin, byte[] data)
            => readFull(fin, data, 0, data.Length);

        public static bool readFull(this Stream fin, byte[] data, out int len)
            => (len = fin.readFull(data, 0, data.Length)) > 0;

        public static int readFull(this Stream fin, 
            byte[] data, int offset, int total)
        {
            int remain = total, len;
            while (remain > 0 
                && fin.read(data, offset, remain, out len))
            {
                remain -= len;
                offset += len;
            }
            return total - remain;
        }
    }
}
