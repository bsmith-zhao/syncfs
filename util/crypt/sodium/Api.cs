using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace util.crypt.sodium
{
    public class Sodium
    {
        static Sodium() => checkLib();

        public static void checkLib()
        {
            if (!libValid)
                throw new Error<Sodium>("InitFail");
        }

        public static void checkGcm()
        {
            checkLib();
            if (!gcmValid)
                throw new Error<Sodium>("Gcm.NoChip");
        }

        public const string dll = "libsodium-64.dll";

        public static bool libValid = sodium_init() != -1;

        public static bool gcmValid
            = crypto_aead_aes256gcm_is_available() == 1;

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sodium_init();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int crypto_aead_aes256gcm_is_available();

        [DllImport(Sodium.dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int
            crypto_aead_chacha20poly1305_ietf_encrypt(
            byte* cipher,
            out ulong cipherLen,
            byte* data,
            ulong dataLen,
            byte* aad,
            ulong aadLen,
            byte* nsec,
            byte* nonce,
            byte* key);

        [DllImport(Sodium.dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int
            crypto_aead_chacha20poly1305_ietf_decrypt(
            byte* data,
            out ulong dataLen,
            byte* nsec,
            byte* cipher,
            ulong cipherLen,
            byte* aad,
            ulong aadLen,
            byte* nonce,
            byte* key);

        [DllImport(Sodium.dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int
            crypto_aead_aes256gcm_encrypt(
            byte* cipher,
            out ulong cipherLen,
            byte* data,
            ulong dataLen,
            byte* aad,
            ulong aadLen,
            byte* nsec,
            byte* nonce,
            byte* key);

        [DllImport(Sodium.dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int
            crypto_aead_aes256gcm_decrypt(
            byte* data,
            out ulong dataLen,
            byte* nsec,
            byte* cipher,
            ulong cipherLen,
            byte* aad,
            ulong aadLen,
            byte* nonce,
            byte* key);

        [DllImport(Sodium.dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int
            crypto_aead_xchacha20poly1305_ietf_encrypt(
            byte* cipher,
            out ulong cipherLen,
            byte* data,
            ulong dataLen,
            byte* aad,
            ulong aadLen,
            byte* nsec,
            byte* nonce,
            byte* key);

        [DllImport(Sodium.dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int
            crypto_aead_xchacha20poly1305_ietf_decrypt(
            byte* data,
            out ulong dataLen,
            byte* nsec,
            byte* cipher,
            ulong cipherLen,
            byte* aad,
            ulong aadLen,
            byte* nonce,
            byte* key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern int crypto_generichash_blake2b
            (
            byte* dst, int dstLen,
            byte* src, long srcLen,
            byte* key, int keylen
            );

        // dstSize => [16, 64]
        public static unsafe byte[] blake2b(byte[] src, int dstSize)
        {
            byte[] dst = new byte[dstSize];
            blake2b(src, dst);
            return dst;
        }

        public static unsafe void blake2b(byte[] src, byte[] dst)
            => blake2b(src, dst, null);

        // key size => [16, 64]
        public static unsafe void blake2b(byte[] src, byte[] dst, byte[] key)
        {
            fixed (byte*
                srcPtr = src,
                dstPtr = dst,
                keyPtr = key)
            {
                crypto_generichash_blake2b(
                    dstPtr, dst.Length,
                    srcPtr, src.Length,
                    keyPtr, key?.Length ?? 0);
            }
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern int crypto_hash_sha256
            (
            byte* dst,
            byte* src, long srcLen
            );

        public static unsafe void sha256(byte[] src, byte[] dst)
        {
            fixed (byte* srcPtr = src,
                dstPtr = dst)
            {
                crypto_hash_sha256
                    (
                    dstPtr,
                    srcPtr, src.Length
                    );
            }
        }

        public static unsafe byte[] sha256(byte[] src)
        {
            byte[] dst = new byte[32];
            sha256(src, dst);
            return dst;
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern int crypto_hash_sha512
            (
            byte* dst,
            byte* src, long srcLen
            );

        public static unsafe void sha512(byte[] src, byte[] dst)
        {
            fixed (byte* srcPtr = src, dstPtr = dst)
            {
                crypto_hash_sha512
                    (
                    dstPtr,
                    srcPtr, src.Length
                    );
            }
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int crypto_pwhash_argon2id(
            byte* key, long keyLen,
            byte* pwd, long pwdLen,
            byte* salt,
            long cpu, int mem,
            int alg);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int crypto_pwhash_alg_argon2id13();

        /// <summary>
        /// fast => {cpu:2, mem:64M}
        /// middle => {cpu:3, mem:256M}
        /// slow => {cpu:4, mem:1G}
        /// recomand: {cpu:32, mem:64M}, {cpu:10, mem:256M}
        /// </summary>
        /// <param name="pwd">[0, 4294967295U]</param>
        /// <param name="salt">16</param>
        /// <param name="cpu"></param>
        /// <param name="mem"></param>
        /// <param name="keySize">[16, 4294967295U]</param>
        /// <returns></returns>
        public static unsafe byte[] argon2id(
            byte[] pwd, byte[] salt,
            long cpu, int mem,
            int keySize = 32)
        {
            if (salt.Length != 16)
                throw new Error<Sodium>("Argon2id.BadSalt", salt.Length);
            var key = new byte[keySize];
            fixed (byte* pwdPtr = pwd,
                        saltPtr = salt,
                        keyPtr = key)
            {
                int code = crypto_pwhash_argon2id
                    (
                    keyPtr, key.Length,
                    pwdPtr, pwd.Length,
                    saltPtr,
                    cpu, mem, crypto_pwhash_alg_argon2id13()
                    );
                if (code != 0)
                    throw new Error<Sodium>("Argon2id.GenFail", code);
            }
            return key;
        }
    }
}
