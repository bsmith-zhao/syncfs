using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt
{
    public enum PwdDeriveType
    {
        PBKDF2,
        Argon2id,
    }

    public abstract class PwdDerive
    {
        public abstract byte[] genKey(byte[] pwd, int keySize);
    }

    public class PBKDF2 : PwdDerive
    {
        public byte[] salt;
        public int turns = 20000;

        public override byte[] genKey(byte[] pwd, int keySize)
            => pwd.pbkdf2(salt, turns, keySize);
    }
}
