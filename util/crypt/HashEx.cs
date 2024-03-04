using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt
{
    public static class HashEx
    {
        public static byte[] hmac256(this byte[] key, byte[] data)
        {
            using (var mac = new HMACSHA256(key))
            {
                return mac.ComputeHash(data);
            }
        }

        public static byte[] pbkdf2(this byte[] pwd, byte[] salt, int turns, int size)
        {
            using (var hash = new Rfc2898DeriveBytes(pwd, salt, turns))
            {
                return hash.GetBytes(size);
            }
        }

        public static byte[] sha256(this byte[] data)
        {
            using (var sha = HashAlgorithm.Create("SHA256"))
            {
                return sha.ComputeHash(data);
            }
        }

        public static byte[] sha512(this byte[] data)
        {
            using (var sha = HashAlgorithm.Create("SHA512"))
            {
                return sha.ComputeHash(data);
            }
        }
    }
}
