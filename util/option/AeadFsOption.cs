﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;
using util.ext;
using util.prop;
using util.prop.adjust;
using util.prop.edit;
using util.rep.aead;

namespace util.option
{
    [TypeConverter(typeof(ExpandClass))]
    public class AeadFsOption
    {
        const string DefaultEncode = "utf-8";

        [UnifyEncode(DefaultEncode)]
        [Editor(typeof(EncodeDropList), typeof(UITypeEditor))]
        public string Encode { get; set; } = DefaultEncode;

        [RangeLimit(32, 128), EditByWheel(8)]
        public int MasterKeySize { get; set; } = 48;

        [RangeLimit(16, 64), EditByWheel(4)]
        public int FileIdSize { get; set; } = 16;

        public int BlockSize = 16.kb();
        [RangeLimit("4K", "128K"), EditByWheel("4K"), ByteSize]
        [DisplayName("BlockSize"), JsonIgnore]
        public string BlockBytes
        {
            get => ((long)BlockSize).byteSize();
            set => BlockSize = (int)value.byteSize();
        }

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

        public DirCryptType DirCrypt { get; set; }
            = DirCryptType.HmacIvCbc;

        public override string ToString()
            => $"{DataCrypt}";

        public AeadFsConf createConf()
            => new AeadFsConf
            {
                Encode = Encode,
                MasterKeySize = MasterKeySize,
                FileIdSize = FileIdSize,
                BlockSize = BlockSize,
                KeyDerive = KeyDerive,
                DataCrypt = DataCrypt,
                DirCrypt = DirCrypt,
            }.create();
    }
}
