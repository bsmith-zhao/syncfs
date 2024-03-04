using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt
{
    public enum KeyDeriveType
    {
        HKDF,
        //Blake2B
    }

    public abstract class KeyDerive
    {
        public abstract byte[] derive(byte[] mkey, byte[] ctx, int size);
    }

    public class HkdfDerive : KeyDerive
    {
        public override byte[] derive(byte[] mkey, byte[] ctx, int size)
            => mkey.hkdfDerive(null, ctx, size);
    }
}
