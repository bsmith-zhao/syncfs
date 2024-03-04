using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.crypt
{
    public static class AesEx
    {
        public static byte[] transform(this ICryptoTransform enc, byte[] src, byte[] dst = null)
        {
            dst = dst ?? new byte[src.Length];
            enc.TransformBlock(src, 0, src.Length, dst, 0);
            return dst;
        }

        public static byte[] transform(this ICryptoTransform enc, byte[] src, int begin, int count, byte[] dst)
        {
            enc.TransformBlock(src, begin, count, dst, 0);
            return dst;
        }

        public static byte[] cbcHmacIvEnc(this byte[] encKey, byte[] macKey, byte[] data)
        {
            using (var enc = new HmacIvCbc(encKey, macKey))
            {
                return enc.encrypt(data);
            }
        }

        public static byte[] aesRnd(this int size)
        {
            return new byte[size].aesRnd();
        }

        public static byte[] aesRnd(this byte[] data)
        {
            using (var rnd = new RNGCryptoServiceProvider())
            {
                rnd.GetBytes(data);
                return data;
            }
        }

        public static byte[] aesKey(this int size)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = size * 8;
                return aes.Key;
            }
        }

        public static byte[] ecbEnc(this byte[] key, byte[] data, int offset, int count)
        {
            return key.aesEnc(data, offset, count, CipherMode.ECB, PaddingMode.None);
        }

        public static byte[] ecbDec(this byte[] key, byte[] data, int offset, int count)
        {
            return key.aesDec(data, offset, count, CipherMode.ECB, PaddingMode.None);
        }

        public static byte[] ecbEnc(this byte[] key, byte[] data, PaddingMode pad = PaddingMode.None)
        {
            return key.aesEnc(data, CipherMode.ECB, pad);
        }

        public static byte[] ecbDec(this byte[] key, byte[] data, PaddingMode pad = PaddingMode.None)
        {
            return key.aesDec(data, CipherMode.ECB, pad);
        }

        public static byte[] cbcEnc(this byte[] key, byte[] data, byte[] iv, PaddingMode pad = PaddingMode.None)
        {
            return key.aesEnc(data, CipherMode.CBC, pad, iv);
        }

        public static byte[] cbcDec(this byte[] key, byte[] data, byte[] iv, PaddingMode pad = PaddingMode.None)
        {
            return key.aesDec(data, CipherMode.CBC, pad, iv);
        }

        public static byte[] aesEnc(this byte[] key, byte[] data, CipherMode mode, PaddingMode pad, byte[] iv = null)
        {
            using (var aes = aesInit(key, mode, pad, iv))
            {
                var enc = aes.CreateEncryptor();
                return aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            }
        }

        public static byte[] aesEnc(this byte[] key, byte[] data, int offset, int count, CipherMode mode, PaddingMode pad, byte[] iv = null)
        {
            using (var aes = aesInit(key, mode, pad, iv))
            {
                var enc = aes.CreateEncryptor();
                return aes.CreateEncryptor().TransformFinalBlock(data, offset, count);
            }
        }

        public static byte[] aesDec(this byte[] key, byte[] data, CipherMode mode, PaddingMode pad, byte[] iv = null)
        {
            using (var aes = aesInit(key, mode, pad, iv))
            {
                return aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            }
        }

        public static byte[] aesDec(this byte[] key, byte[] data, int offset, int count, CipherMode mode, PaddingMode pad, byte[] iv = null)
        {
            using (var aes = aesInit(key, mode, pad, iv))
            {
                return aes.CreateDecryptor().TransformFinalBlock(data, offset, count);
            }
        }

        public static Aes aesInit(this byte[] key, CipherMode mode, PaddingMode pad, byte[] iv)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = mode;
            aes.Padding = pad;
            if (null != iv)
                aes.IV = iv;
            return aes;
        }
    }
}
