using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt.sodium;

namespace util.crypt
{
    public enum AeadCryptType
    {
        AesGcm12,
        ChaCha20Poly1305,
        XChaCha20Poly1305,
    }

    public abstract class AeadCrypt
    {
        public abstract int KeySize { get; }
        public abstract int NonceSize { get; }
        public abstract int TagSize { get; }
        public virtual byte[] Key { set; get; }

        public AeadCrypt setKey(byte[] key)
        {
            Key = key;
            return this;
        }

        public static AeadCrypt create(AeadCryptType type)
        {
            switch (type)
            {
                case AeadCryptType.AesGcm12:
                    return new AesGcm12();
                case AeadCryptType.ChaCha20Poly1305:
                    return new ChaCha20Poly1305();
                case AeadCryptType.XChaCha20Poly1305:
                    return new XChaCha20Poly1305();
            }
            return null;
        }

        public abstract void encrypt(byte[] plain, int plainOff, int plainLen,
                            byte[] nonce,
                            byte[] cipher, int cipherOff = 0,
                            byte[] aad = null);

        public abstract bool decrypt(byte[] cipher, int cipherOff, int cipherLen,
                            byte[] nonce,
                            byte[] plain, int plainOff = 0,
                            byte[] aad = null);
    }
}
