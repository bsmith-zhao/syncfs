using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt
{
    public enum DirCryptType
    {
        HmacIvCbc,
    }

    public abstract class DirCrypt
    {
        public abstract int KeySize { get; }
        public abstract byte[] encrypt(byte[] plain);
        public abstract byte[] decrypt(byte[] cipher, bool b16);
    }

    public class HmacIvCbcCrypt : DirCrypt
    {
        public override int KeySize => 64;
        public byte[] Key
        {
            set => cbc = new HmacIvCbc(value);
        }

        HmacIvCbc cbc;

        public override byte[] decrypt(byte[] cipher, bool b16)
            => cbc.decrypt(cipher, b16);

        public override byte[] encrypt(byte[] plain)
            => cbc.encrypt(plain);
    }
}
