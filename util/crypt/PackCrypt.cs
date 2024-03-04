using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt
{
    public class PackCrypt
    {
        public PackCrypt(AeadCrypt aead, byte[] nonce)
        {
            this.aead = aead;
            this.nonce = nonce.jclone();
            counter = nonce.i64(nonce.Length - 8);
        }

        AeadCrypt aead;
        byte[] nonce;
        long counter;

        public void encrypt(byte[] plain, int plainOff, int plainLen,
                            long packIdx,
                            byte[] cipher, int cipherOff = 0)
        {
            aead.encrypt(plain, plainOff, plainLen,
                        getNonce(packIdx),
                        cipher, cipherOff,
                        aad(packIdx));
        }

        public void decrypt(byte[] cipher, int cipherOff, int cipherLen,
                            long packIdx,
                            byte[] plain, int plainOff = 0)
        {
            if (!aead.decrypt(cipher, cipherOff, cipherLen,
                            getNonce(packIdx),
                            plain, plainOff,
                            aad(packIdx)))
                throw new Error(this, "VerifyFail", packIdx);
        }

        byte[] getNonce(long index)
        {
            (counter + index).copyTo(nonce, nonce.Length - 8);
            return nonce;
        }

        byte[] aad(long index) => index.bytes();
    }
}
