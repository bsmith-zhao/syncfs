using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;
using util.rep.aead;

namespace xtext
{
    public class TextFile
    {
        public const string FileType = "xtext";
        public const int MaxVersion = 1;

        public string Type = null;
        public int Version = 0;

        public PwdDeriveEntry[] PwdDerives;
        public KeyDeriveType KeyDerive = KeyDeriveType.HKDF;
        public AeadCryptType DataCrypt = AeadCryptType.XChaCha20Poly1305;

        public byte[] Nonce;
        public byte[] Cipher;

        public static TextFile load(string path)
        {
            if (!path.fileExist())
                return null;
            var txt = path.readText().obj<TextFile>();
            if (txt.Type != FileType)
                throw new Error<TextFile>("InvalidType", txt.Type);
            if (txt.Version > MaxVersion)
                throw new Error<AeadFsConf>("InvalidVersion",
                    txt.Version, MaxVersion);

            return txt;
        }

        byte[] data;
        public bool decrypt(byte[] pwd)
        {
            data = new byte[Cipher.Length - aead.TagSize];
            return getConfCrypt(pwd).decrypt(Cipher, 0, Cipher.Length, Nonce, data);
        }

        AeadCrypt _aead;
        AeadCrypt aead => _aead ?? (_aead = newDataCrypt());
        AeadCrypt getConfCrypt(byte[] pwd)
        {
            var pkey = derivePwdKey(pwd, aead.KeySize);
            return aead.setKey(pkey);
        }

        public AeadCrypt newDataCrypt()
            => AeadCrypt.create(DataCrypt);

        byte[] derivePwdKey(byte[] pwd, int size)
        {
            var pwdKey = pwd ?? new byte[0];
            PwdDerives.each(kg => pwdKey = kg.deriveKey(pwdKey, size));
            return pwdKey;
        }

        public byte[] getData() => data;
        public void setData(byte[] d) => this.data = d;

        public void save(string path, byte[] pwd, PwdDeriveEntry[] pds)
        {
            this.PwdDerives = pds;
            this.encrypt(pwd);
            var json = this.jsonIndent();
            json.bakSaveTo(path);
        }

        public TextFile encrypt(byte[] pwd)
        {
            Nonce = aead.NonceSize.aesRnd();
            Cipher = new byte[data.Length + aead.TagSize];
            getConfCrypt(pwd).encrypt(data, 0, data.Length, Nonce, Cipher);
            return this;
        }
    }
}
