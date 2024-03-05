using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;

namespace xtext
{
    [TypeConverter(typeof(ExpandProp))]
    public class ExtendTextOption
    {
        PwdDeriveType[] kgs = { PwdDeriveType.PBKDF2, PwdDeriveType.Argon2id };
        [TypeConverter(typeof(ArrayProp))]
        public PwdDeriveType[] PwdDerives
        {
            get => kgs;
            set
            {
                kgs = value;
                if (kgs.empty())
                    kgs = new PwdDeriveType[] { PwdDeriveType.Argon2id };
            }
        }

        public KeyDeriveType KeyDerive { get; set; }
            = KeyDeriveType.HKDF;

        public AeadCryptType DataCrypt { get; set; }
            = AeadCryptType.XChaCha20Poly1305;

        public ExtendText createText()
            => new ExtendText
            {
                Type = ExtendText.FileType,
                Version = ExtendText.MaxVersion,
                KeyDerive = KeyDerive,
                DataCrypt = DataCrypt,
            };

        public override string ToString() => $"{DataCrypt}";
    }
}
