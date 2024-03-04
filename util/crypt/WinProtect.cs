using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.crypt
{
    public static class WinProtect
    {
        public static byte[] winTryDec(this byte[] cipher, byte[] salt = null)
        {
            try
            {
                return cipher?.winDec(salt);
            }
            catch { }
            return null;
        }

        public static byte[] winEnc(this byte[] data, byte[] salt = null)
        {
            return ProtectedData.Protect(data, salt, DataProtectionScope.CurrentUser);
        }

        public static byte[] winDec(this byte[] cipher, byte[] entropy = null)
        {
            return ProtectedData.Unprotect(cipher, entropy, DataProtectionScope.CurrentUser);
        }
    }
}
