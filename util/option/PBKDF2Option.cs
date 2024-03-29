﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;
using util.ext;
using util.prop;
using util.prop.edit;

namespace util.option
{
    [TypeConverter(typeof(ExpandClass))]
    public class PBKDF2Option
    {
        public int SaltSize = 32;
        [NumberWheel(20000, 100000, 5000)]
        public int Turns { get; set; } = 30000;

        public override string ToString()
            => $"Turns:{Turns}";

        public PwdDerive create()
            => new PBKDF2
            {
                salt = SaltSize.aesRnd(),
                turns = new Random().Next((int)(Turns * 0.9), (int)(Turns * 1.1)),
            };

        public void evalTime()
        {
            create().genKey("123abc".utf8(), 32);
        }
    }
}
