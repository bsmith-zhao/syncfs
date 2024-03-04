using util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt
{
    public static class AesCtr
    {
        public static byte[] ctrXor(this byte[] key, byte[] data, byte[] nonce, long counter)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                var enc = aes.CreateEncryptor();
                return ctrXor(enc, data, nonce, counter);
            }
        }

        public static byte[] ctrXor(this ICryptoTransform enc, byte[] data, byte[] nonce, long counter)
        {
            var chain = new byte[(data.Length / 16 + 1) * 16];
            chain.ctrSetNonce(nonce);
            chain.ctrSetCtr(counter, chain.Length / 16);

            var cipher = new byte[chain.Length];
            enc.TransformBlock(chain, 0, chain.Length, cipher, 0);

            data.xor(0, cipher, 0, data.Length);

            return data;
        }

        public static void ctrSetNonce(this byte[] chain, byte[] nonce)
        {
            for (int i = 0; i < chain.Length; i += 16)
            {
                Buffer.BlockCopy(nonce, 0, chain, i, 8);
            }
        }

        public static void ctrSetCtr(this byte[] chain, long begin, int count)
        {
            count = count * 16;
            for (int i = 8; i < count; i += 16)
            {
                Buffer.BlockCopy((begin++).bytes(), 0, chain, i, 8);
            }
        }

        unsafe public static void ctrSetNonByPtr(this byte[] chain, long nonce)
        {
            fixed (byte* P0 = chain)
            {
                long* ptr = (long*)P0;
                int count = chain.Length / 16;
                while (count-- > 0)
                {
                    *ptr = nonce;
                    ptr += 2;
                }
            }
        }

        unsafe public static void ctrSetCtrByPtr(this byte[] chain, long begin, int count)
        {
            fixed (byte* P0 = chain)
            {
                long* ptr = (long*)(P0 + 8);
                while (count-- > 0)
                {
                    *ptr = begin++;
                    ptr += 2;
                }
            }
        }
    }
}
