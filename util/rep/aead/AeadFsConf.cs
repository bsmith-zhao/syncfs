using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;
using util.crypt.sodium;
using util.ext;

namespace util.rep.aead
{
    public class AeadFsConf
    {
        public string Type = null;
        public int Version = 0;
        public string Encode = "utf-8";
        public int MasterKeySize = 64;
        public int FileIdSize = 16;
        public int BlockSize = 4 * 1024;
        public PwdDeriveEntry[] PwdDerives;
        public KeyDeriveType KeyDerive = KeyDeriveType.HKDF;
        public AeadCryptType DataCrypt = AeadCryptType.ChaCha20Poly1305;
        public DirCryptType DirCrypt = DirCryptType.HmacIvCbc;
        public byte[] Nonce;
        public byte[] Cipher;

        byte[] derivePwdKey(byte[] pwd, int size)
        {
            var pwdKey = pwd ?? new byte[0];
            PwdDerives.each(kg => pwdKey = kg.deriveKey(pwdKey, size));
            return pwdKey;
        }

        byte[] mkey;
        public bool decrypt(byte[] pwd)
        {
            mkey = new byte[Cipher.Length - aead.TagSize];
            return getCrypt(pwd).decrypt(Cipher, 0, Cipher.Length, Nonce, mkey);
        }

        public void setMKey(byte[] mkey)
            => this.mkey = mkey;

        public byte[] getMKey()
            => mkey;

        public AeadFsConf create()
        {
            Type = AeadFsReposit.Type;
            Version = AeadFsReposit.Version;
            mkey = new byte[MasterKeySize].aesRnd();
            return this;
        }

        public AeadFsConf encrypt(byte[] pwd)
        {
            Nonce = aead.NonceSize.aesRnd();
            Cipher = new byte[mkey.Length + aead.TagSize];
            getCrypt(pwd).encrypt(mkey, 0, mkey.Length, Nonce, Cipher);
            return this;
        }

        public AeadCrypt newCrypt()
            => AeadCrypt.create(DataCrypt);

        AeadCrypt _aead;
        AeadCrypt aead => _aead ?? (_aead = newCrypt());
        AeadCrypt getCrypt(byte[] pwd)
        {
            var pkey = derivePwdKey(pwd, aead.KeySize);
            return aead.setKey(pkey);
        }

        KeyDerive kdf;
        KeyDerive newKdf()
        {
            switch (KeyDerive)
            {
                case KeyDeriveType.HKDF:
                    return new HkdfDerive();
            }
            return null;
        }
        
        public byte[] deriveEncKey(byte[] ctx, int size)
            => (kdf ?? (kdf = newKdf())).derive(mkey, ctx, size);

        public DirCrypt newDirCrypt(byte[] ctx)
        {
            switch (DirCrypt)
            {
                case DirCryptType.HmacIvCbc:
                    var enc = new HmacIvCbcCrypt();
                    enc.Key = deriveEncKey(ctx, enc.KeySize);
                    return enc;
            }
            return null;
        }

        public int nonceSize() => aead.NonceSize;
        public int tagSize() => aead.TagSize;
        public int packSize() => BlockSize + tagSize();

        public static string confPath(string dir)
            => $"{dir}/{AeadFsReposit.Type}.conf";

        public static AeadFsConf loadByDir(string dir)
            => load(confPath(dir));

        public void saveByDir(string dir, byte[] pwd, PwdDeriveEntry[] pds)
            => save(confPath(dir), pwd, pds);

        public static AeadFsConf load(string path)
        {
            if (!path.fileExist())
                return null;
            var conf = path.readText().obj<AeadFsConf>();
            if (conf.Type != AeadFsReposit.Type)
                throw new Error<AeadFsConf>("InvalidType", conf.Type);
            if (conf.Version > AeadFsReposit.Version)
                throw new Error<AeadFsConf>("InvalidVersion",
                    conf.Version, AeadFsReposit.Version);

            return conf;
        }

        public void save(string path, byte[] pwd, PwdDeriveEntry[] pds)
        {
            this.PwdDerives = pds;
            this.encrypt(pwd);
            var json = this.jsonIndent();
            json.saveTo(path);
            json.saveTo($"{path}1");
            json.saveTo($"{path}2");
        }
    }

    public class PwdDeriveEntry
    {
        public PwdDeriveType type;
        public object args;

        PwdDerive getArgs()
            => (args is PwdDerive ? args : args = args.jcopy(getType()))
                as PwdDerive;

        public byte[] deriveKey(byte[] pwd, int size)
            => getArgs().genKey(pwd, size);

        Type getType()
        {
            switch (type)
            {
                case PwdDeriveType.PBKDF2:
                    return typeof(PBKDF2);
                case PwdDeriveType.Argon2id:
                    return typeof(Argon2id);
            }
            return null;
        }
    }
}
