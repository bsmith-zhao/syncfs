using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt.sodium
{
    public class XChaCha20Poly1305 : SodiumAeadCrypt
    {
        public override int KeySize => 32;
        public override int NonceSize => 24;
        public override int TagSize => 16;

        protected override unsafe EncryptFunc encFunc
            => Sodium.crypto_aead_xchacha20poly1305_ietf_encrypt;

        protected override unsafe DecryptFunc decFunc
            => Sodium.crypto_aead_xchacha20poly1305_ietf_decrypt;
    }
}
