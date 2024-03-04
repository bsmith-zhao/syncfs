using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt.sodium
{
    public abstract class SodiumAeadCrypt : AeadCrypt
    {
        public SodiumAeadCrypt() => Sodium.checkLib();

        public override unsafe void encrypt(byte[] data, int dataOff, int dataLen,
                            byte[] nonce,
                            byte[] cipher, int cipherOff = 0,
                            byte[] aad = null)
        {
            fixed (byte* cipherPtr = cipher)
            fixed (byte* dataPtr = data)
            fixed (byte* noncePtr = nonce)
            fixed (byte* keyPtr = Key)
            fixed (byte* aadPtr = aad)
            {
                encFunc
                    (
                    cipherPtr + cipherOff, out var cipherLen,
                    dataPtr + dataOff, (ulong)dataLen,
                    aadPtr, (ulong)(aad?.Length ?? 0),
                    null,
                    noncePtr,
                    keyPtr
                    );
            }
        }

        public override unsafe bool decrypt(byte[] cipher, int cipherOff, int cipherLen,
                                    byte[] nonce,
                                    byte[] data, int dataOff = 0,
                                    byte[] aad = null)
        {
            fixed (byte* cipherPtr = cipher)
            fixed (byte* dataPtr = data)
            fixed (byte* noncePtr = nonce)
            fixed (byte* keyPtr = Key)
            fixed (byte* aadPtr = aad)
            {
                return decFunc
                    (
                    dataPtr + dataOff, out var dataLen,
                    null,
                    cipherPtr + cipherOff, (ulong)cipherLen,
                    aadPtr, (ulong)(aad?.Length ?? 0),
                    noncePtr,
                    keyPtr
                    ) == 0;
            }
        }

        protected abstract DecryptFunc decFunc { get; }
        protected abstract EncryptFunc encFunc { get; }

        protected unsafe delegate int EncryptFunc(
            byte* cipher,
            out ulong cipherLen,
            byte* data,
            ulong dataLen,
            byte* aad,
            ulong aadLen,
            byte* nsec,
            byte* nonce,
            byte* key);

        protected unsafe delegate int DecryptFunc(
            byte* data,
            out ulong dataLen,
            byte* nsec,
            byte* cipher,
            ulong cipherLen,
            byte* aad,
            ulong aadLen,
            byte* nonce,
            byte* key);
    }
}
