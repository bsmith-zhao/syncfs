using HkdfDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;

namespace util.crypt
{
    public class HKDFTest : Test
    {
        public override void test()
        {
            nullSaltTest();
            randomSaltTest();
            randomSaltAndInfoTest();
        }

        public void nullSaltTest()
        {
            "key = 0x00 * 32".@case();
            deriveKey(new byte[32].set((byte)0x00), null, null);

            "key = 0xFF * 32".@case();
            deriveKey(new byte[32].set((byte)0xFF), null, null);

            "key = random(32)".@case();
            deriveKey(32.aesRnd(), null, null);
        }

        public void randomSaltTest()
        {
            var rnd = new Random();

            "key = 0x00 * 32, salt = random(1,32)".@case();
            deriveKey(new byte[32].set((byte)0x00), rnd.Next(1,32).aesRnd(), null);

            "key = 0xFF * 32, salt = random(1,32)".@case();
            deriveKey(new byte[32].set((byte)0xFF), rnd.Next(1, 32).aesRnd(), null);

            "key = random(32), salt = random(1,32)".@case();
            deriveKey(32.aesRnd(), rnd.Next(1, 32).aesRnd(), null);
        }

        public void randomSaltAndInfoTest()
        {
            var rnd = new Random();

            "key = 0x00 * 32, salt = random(1,32), info = random(1,32)".@case();
            deriveKey(new byte[32].set((byte)0x00), rnd.Next(1, 32).aesRnd(), rnd.Next(1, 32).aesRnd());

            "key = 0xFF * 32, salt = random(1,32), info = random(1,32)".@case();
            deriveKey(new byte[32].set((byte)0xFF), rnd.Next(1, 32).aesRnd(), rnd.Next(1, 32).aesRnd());

            "key = random(32), salt = random(1,32), info = random(1,32)".@case();
            deriveKey(32.aesRnd(), rnd.Next(1, 32).aesRnd(), rnd.Next(1, 32).aesRnd());
        }

        public void deriveKey(byte[] key, byte[] salt, byte[] info)
        {
            var subKey1 = Hkdf.DeriveKey(HashAlgorithmName.SHA256, key, 32, info, salt);
            var subKey2 = key.hkdfDerive(salt, info, 32);

            subKey1.showHex(nameof(subKey1));
            subKey2.showHex(nameof(subKey2));

            assert(subKey1.b64() == subKey2.b64());
        }
    }
}
