using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.crypt
{
    public class HmacIvCbc : IDisposable
    {
        Aes aes;
        HMACSHA256 hmac;
        ICryptoTransform ecbEnc;
        ICryptoTransform ecbDec;
        ICryptoTransform ecbPadEnc;
        ICryptoTransform ecbPadDec;

        public HmacIvCbc(byte[] key)
        {
            init(key.head(key.Length / 2), key.tail(key.Length / 2));
        }

        public HmacIvCbc(byte[] encKey, byte[] macKey)
        {
            init(encKey, macKey);
        }

        void init(byte[] encKey, byte[] macKey)
        {
            aes = Aes.Create();
            aes.Key = encKey;

            hmac = new HMACSHA256(macKey);
            ecbEnc = getEncoder(CipherMode.ECB);
            ecbPadEnc = getEncoder(CipherMode.ECB, null, PaddingMode.PKCS7);
            ecbDec = getDecoder(CipherMode.ECB);
            ecbPadDec = getDecoder(CipherMode.ECB, null, PaddingMode.PKCS7);
        }

        ICryptoTransform getEncoder(CipherMode mode, byte[] iv = null, PaddingMode pad = PaddingMode.None)
        {
            aes.Mode = mode;
            aes.Padding = pad;
            if (null != iv)
                aes.IV = iv;
            return aes.CreateEncryptor();
        }

        byte[] cbcEnc(byte[] iv, byte[] src, int srcPos, int count, byte[] dst, int dstPos)
        {
            using (var enc = getEncoder(CipherMode.CBC, iv))
            {
                enc.TransformBlock(src, srcPos, count, dst, dstPos);
            }
            return dst;
        }

        ICryptoTransform getDecoder(CipherMode mode, byte[] iv = null, PaddingMode pad = PaddingMode.None)
        {
            aes.Mode = mode;
            aes.Padding = pad;
            if (null != iv)
                aes.IV = iv;
            return aes.CreateDecryptor();
        }

        byte[] cbcDec(byte[] iv, byte[] src, int srcPos, int count, byte[] dst, int dstPos)
        {
            using (var enc = getDecoder(CipherMode.CBC, iv))
            {
                enc.TransformBlock(src, srcPos, count, dst, dstPos);
            }
            return dst;
        }

        public byte[] encrypt(byte[] data)
        {
            if (data.Length < 16)
                return ecbPadEnc.TransformFinalBlock(data, 0, data.Length);
            else if (data.Length == 16)
                return ecbEnc.transform(data);

            var cipher = new byte[data.Length];
            var iv = hmac.ComputeHash(data, 16, data.Length - 16).head(16);

            // no need steal cipher
            if (data.Length % 16 == 0)
                return cbcEnc(iv, data, 0, data.Length, cipher, 0);

            // transform N-1 CBC block
            var lastPos = 16 * (data.Length / 16);
            cbcEnc(iv, data, 0, lastPos, cipher, 0);

            // steal cipher text for padding
            var lastSize = data.Length - lastPos;
            var stealPos = lastPos - 16 + lastSize;
            Buffer.BlockCopy(cipher, stealPos, data, data.Length - 16, 16 - lastSize);
            ecbEnc.TransformBlock(data, data.Length - 16, 16, cipher, cipher.Length - 16);

            return cipher;
        }

        public byte[] decrypt(byte[] cipher, bool b16)
        {
            if (cipher.Length == 16)
            {
                if (b16)
                    return ecbDec.transform(cipher);
                else
                    return ecbPadDec.TransformFinalBlock(cipher, 0, 16);
            }

            var data = new byte[cipher.Length];
            // last steal block
            if (data.Length % 16 != 0)
            {
                ecbDec.TransformBlock(cipher, cipher.Length - 16, 16, data, data.Length - 16);
                // pay back the steal cipher
                Buffer.BlockCopy(data, data.Length - 16, cipher, cipher.Length - 16, 16);
            }
            // middle normal cbc chain
            var middleCount = data.Length / 16 - 1;
            if (middleCount > 0)
                cbcDec(cipher.head(16), cipher, 16, middleCount * 16, data, 16);
            // first block
            var iv = hmac.ComputeHash(data, 16, data.Length - 16).head(16);
            cbcDec(iv, cipher, 0, 16, data, 0);

            return data;
        }

        public void Dispose()
        {
            ecbEnc?.Dispose();
            ecbDec?.Dispose();
            ecbPadEnc?.Dispose();
            ecbPadDec?.Dispose();
            aes?.Dispose();
            hmac?.Dispose();
        }
    }
}
