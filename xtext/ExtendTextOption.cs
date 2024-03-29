﻿using System;
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
    [TypeConverter(typeof(ExpandClass))]
    public class TextFileOption
    {
        PwdDeriveType[] kgs = { PwdDeriveType.PBKDF2, PwdDeriveType.Argon2id };
        [TypeConverter(typeof(ArrayField))]
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

        public TextFile createText()
            => new TextFile
            {
                Type = TextFile.FileType,
                Version = TextFile.MaxVersion,
                KeyDerive = KeyDerive,
                DataCrypt = DataCrypt,
            };

        public override string ToString() => $"{DataCrypt}";
    }
}
