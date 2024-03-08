using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;
using util.ext;
using util.rep.aead;

namespace util.option
{
    [TypeConverter(typeof(ExpandClass))]
    public class PwdDeriveOption
    {
        [TypeConverter(typeof(ExpandClass))]
        public PBKDF2Option PBKDF2 { get; set; }
            = new PBKDF2Option();

        [TypeConverter(typeof(ExpandClass))]
        public Argon2idOption Argon2id { get; set; }
            = new Argon2idOption();

        public PwdDeriveEntry newDeriveEntry(PwdDeriveType type)
            => new PwdDeriveEntry
            {
                type = type,
                args = newDerive(type),
            };

        public PwdDerive newDerive(PwdDeriveType type)
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
            => kgs.conv(newDeriveEntry);

        public override string ToString() => "";
    }
}
