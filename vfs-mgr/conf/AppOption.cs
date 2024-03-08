using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.crypt.sodium;
using util.ext;
using util.option;
using util.prop;
using util.prop.adjust;
using util.rep.aead;

namespace vfs.mgr.conf
{
    public class AppOption
    {
        public AeadFsOption AeadFS { get; set; }
            = new AeadFsOption();

        public PBKDF2Option PBKDF2 { get; set; }
            = new PBKDF2Option();

        public Argon2idOption Argon2id { get; set; }
            = new Argon2idOption();

        public string VfsExe { get; set; } = "vfs.exe";

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
