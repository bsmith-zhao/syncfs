using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.crypt
{
    // alteritive: NuGet -> HkdfDotNet

    /// <summary>
    /// This class implements rfc5869 HMAC-based Extract-and-Expand Key Derivation Function 
    /// (HKDF) using HMACSHA256.
    /// Reference: https://www.rfc-editor.org/rfc/rfc5869
    /// </summary>
    public static class HKDF
    {
        /// <summary>
        /// Returns a 32 byte psuedorandom number that can be used with the Expand method if 
        /// a cryptographically secure pseudorandom number is not already available.
        /// </summary>
        /// <param name="salt">(Optional, but you should use it) Non-secret random value. 
        /// If less than 64 bytes it is padded with zeros. Can be reused but output is 
        /// stronger if not reused. (And of course output is much stronger with salt than 
        /// without it)</param>
        /// <param name="key">Material that is not necessarily random that
        /// will be used with the HMACSHA256 hash function and the salt to produce
        /// a 32 byte psuedorandom number.</param>
        /// <returns></returns>
        public static byte[] hkdfExtract(this byte[] key, byte[] salt)
        {
            //For algorithm docs, see section 2.2: https://www.rfc-editor.org/rfc/rfc5869 
            salt = salt ?? new byte[0];
            using (HMACSHA256 hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(key, offset: 0, count: key.Length);
            }
        }

        /// <summary>
        /// Returns a secure pseudorandom key of the desired length. Useful as a key derivation
        /// function to derive one cryptograpically secure pseudorandom key from another
        /// cryptograpically secure pseudorandom key. This can be useful, for example,
        /// when needing to create a subKey from a master key.
        /// </summary>
        /// <param name="key">A cryptograpically secure pseudorandom number. Can be obtained
        /// via the Extract method or elsewhere. Must be 32 bytes or greater. 64 bytes is 
        /// the prefered size.  Shorter keys are padded to 64 bytes, longer ones are hashed
        /// to 64 bytes.</param>
        /// <param name="info">(Optional) Context and application specific information.
        /// Allows the output to be bound to application context related information.</param>
        /// <param name="size">Length of output in bytes.</param>
        /// <returns></returns>
        public static byte[] hkdfExpand(this byte[] key, byte[] info, int size)
        {
            //For algorithm docs, see section 2.3: https://www.rfc-editor.org/rfc/rfc5869 
            //Also note:
            //       SHA256 has a block size of 64 bytes
            //       SHA256 has an output length of 32 bytes (but can be truncated to less)

            info = info ?? new byte[0];

            const int hashSize = 32;

            //Min recommended length for Key is the size of the hash output (32 bytes in this case)
            //See section 2: https://www.rfc-editor.org/rfc/rfc2104#section-3
            //Also see:      http://security.stackexchange.com/questions/95972/what-are-requirements-for-hmac-secret-key
            if (key == null || key.Length < 32)
            {
                throw new ArgumentOutOfRangeException("Key should be 32 bytes or greater.");
            }

            if (size > 255 * hashSize)
            {
                throw new ArgumentOutOfRangeException("Output length must 8160 bytes or less which is 255 * the SHA256 block site of 32 bytes.");
            }

            int outputIndex = 0;
            byte[] buffer;
            byte[] hash = new byte[0];
            byte[] output = new byte[size];
            int count = 1;
            int bytesToCopy;

            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                while (outputIndex < size)
                {
                    //Setup buffer to hash
                    buffer = new byte[hash.Length + info.Length + 1];
                    Buffer.BlockCopy(hash, 0, buffer, 0, hash.Length);
                    Buffer.BlockCopy(info, 0, buffer, hash.Length, info.Length);
                    buffer[buffer.Length - 1] = (byte)count++;

                    //Hash the buffer and return a 32 byte hash
                    hash = hmac.ComputeHash(buffer, offset: 0, count: buffer.Length);

                    //Copy as much of the hash as we need to the final output
                    bytesToCopy = Math.Min(size - outputIndex, hash.Length);
                    Buffer.BlockCopy(hash, 0, output, outputIndex, bytesToCopy);
                    outputIndex += bytesToCopy;
                }
            }

            return output;
        }


        /// <summary>
        /// Generates a psuedorandom number of the length specified.  This number is suitable
        /// for use as an encryption key, HMAC validation key or other uses of a cryptographically
        /// secure psuedorandom number.
        /// </summary>
        /// <param name="salt">non-secret random value. If less than 64 bytes it is 
        /// padded with zeros. Can be reused but output is stronger if not reused.</param>
        /// <param name="key">Material that is not necessarily random that
        /// will be used with the HMACSHA256 hash function and the salt to produce
        /// a 32 byte psuedorandom number.</param>
        /// <param name="info">(Optional) context and application specific information.
        /// Allows the output to be bound to application context related information. Pass 0 length
        /// byte array to omit.</param>
        /// <param name="size">Length of output in bytes.</param>
        public static byte[] hkdfDerive(this byte[] key, byte[] salt, byte[] info, int size)
        {
            return key.hkdfExtract(salt).hkdfExpand(info, size);
        }

        public static byte[] hkdfDerive(this byte[] key, byte[] salt, string info, int size)
        {
            return key.hkdfExtract(salt).hkdfExpand(info.utf8(), size);
        }
    }
}
