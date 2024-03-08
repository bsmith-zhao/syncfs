using util;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace util
{
    public static class Bytes
    {
        public static bool allZero(this byte[] src, int off, int cnt)
        {
            while (cnt-- > 0)
                if (src[off++] != 0)
                    return false;
            return true;
        }

        public static void copyTo(this byte[] src, int srcIdx, byte[] dst, int dstIdx = 0, int? count = null)
        {
            Buffer.BlockCopy(src, srcIdx, dst, dstIdx, count ?? (src.Length - srcIdx));
            //Array.Copy(src, srcIdx, dst, dstIdx, count ?? (src.Length - srcIdx));
        }

        public static void pasteBy(this byte[] dst, int dstIdx, byte[] src, int srcIdx = 0, int? count = null)
        {
            Buffer.BlockCopy(src, srcIdx, dst, dstIdx, count ?? (src.Length - srcIdx));
            //Array.Copy(src, srcIdx, dst, dstIdx, count ?? (src.Length - srcIdx));
        }

        public static bool isSame(this byte[] src, byte[] dst, int size)
            => memcmp(src, dst, size) == 0;

        public static bool isSame(this byte[] src, byte[] dst)
            => src.Length == dst.Length
            && memcmp(src, dst, src.Length) == 0;

        public static bool isSame(this byte[] src, int srcIdx, byte[] dst, int dstIdx = 0, int? count = null)
        {
            count = count ?? (dst.Length - dstIdx);
            while (count-- > 0)
            {
                if (src[srcIdx++] != dst[dstIdx++])
                    return false;
            }
            return true;
        }

        //public unsafe static void xorByPtr(this byte[] data, int dataOff, byte[] mask, int maskOff, int count)
        //{
        //    int div = count / 8;
        //    fixed (byte* D0 = data, K0 = mask)
        //    {
        //        long* dptr = (long*)(D0 + dataOff);
        //        long* kptr = (long*)(K0 + maskOff);

        //        for (int i = 0; i < div; i++)
        //        {
        //            *(dptr + i) ^= *(kptr + i);
        //        }
        //    }

        //    dataOff += div * 8;
        //    maskOff += div * 8;
        //    count = count % 8;
        //    while (count > 0)
        //    {
        //        data[dataOff++] ^= mask[maskOff++];
        //        count--;
        //    }
        //}

        //public static void xor(this byte[] data, int dataOffset, byte[] key, int keyOffset, int count)
        //{
        //    while (count > 0)
        //    {
        //        data[dataOffset++] ^= key[keyOffset++];
        //        count--;
        //    }
        //}

        public static string hex(this byte[] data)
        {
            return BytesToHexEnhance(data);
        }

        public static byte[] hex(this string text)
        {
            return HexToBytesEnhance(text);
        }

        public static string b64(this byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] b64(this string code)
            => Convert.FromBase64String(b64Pad(code));

        public static string b64Pad(this string code)
        {
            switch (code.Length % 4)
            {
                case 1: code = $"{code}==="; break;
                case 2: code = $"{code}=="; break;
                case 3: code = $"{code}="; break;
            }
            return code;
        }

        public static string BytesToHexEnhance(byte[] data)
        {
            char[] chars = new char[data.Length * 2];
            int b, ci = 0, bi = 0;
            while (bi < data.Length)
            {
                b = data[bi] >> 4;
                chars[ci++] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = data[bi++] & 0xF;
                chars[ci++] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(chars);
        }

        public static byte[] HexToBytesEnhance(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Error(typeof(Bytes), "SizeNotEven", hex.Length);

            byte[] bytes = new byte[hex.Length / 2];

            int low, high, ci = 0, bi = 0;
            while (bi < bytes.Length)
            {
                low = hex[ci++];
                low = low - (low < 58 ? 48 : (low < 97 ? 55 : 87));
                high = hex[ci++];
                high = high - (high < 58 ? 48 : (high < 97 ? 55 : 87));
                bytes[bi++] = (byte)((low << 4) + high);
            }

            return bytes;
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(byte[] src, byte[] dst, int size);

    }
}
