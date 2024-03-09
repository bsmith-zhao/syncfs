using Newtonsoft.Json;
using sync.app.conf;
using System;
using System.ComponentModel;
using System.Text;
using util;
using util.crypt;
using util.crypt.sodium;
using util.ext;
using util.option;
using util.prop;
using util.prop.edit;
using util.rep.aead;

namespace sync.app
{
    public class AppOption
    {
        [TypeConverter(typeof(ExpandClass))]
        public AeadFsOption AeadFS { get; set; }
            = new AeadFsOption();

        [TypeConverter(typeof(ExpandClass))]
        public PBKDF2Option PBKDF2 { get; set; }
            = new PBKDF2Option();

        [TypeConverter(typeof(ExpandClass))]
        public Argon2idOption Argon2id { get; set; }
            = new Argon2idOption();

        [ReadOnly(true)]
        public string Lock { get; set; } = "sync.lock";

        [Browsable(false)]
        public string VfsExe { get; set; } = "vfs.exe";

        [NumberWheel(1, 20, 1)]
        public int LogCount { get; set; } = 10;

        public PwdDeriveEntry newKgEntry(PwdDeriveType type)
            => new PwdDeriveEntry
            {
                type = type,
                args = newKeyGen(type),
            };

        public PwdDerive newKeyGen(PwdDeriveType type)
        {
            switch (type)
            {
                case PwdDeriveType.PBKDF2:
                    return PBKDF2.create();
                case PwdDeriveType.Argon2id:
                    return Argon2id.create();
            }
            return null;
        }

        public PwdDeriveEntry[] newPwdDerives(PwdDeriveType[] kgs)
            => kgs.conv(newKgEntry);
    }
}
