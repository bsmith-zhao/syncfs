﻿using sync.hash;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;
using util.prop.edit;

namespace sync.app
{
    [TypeConverter(typeof(ExpandClass))]
    public class HashConf
    {
        [Browsable(false)]
        public HashType Type { get; set; } = HashType.SHA256;

        public HashMode Mode { get; set; } = HashMode.Sample;

        [NumberWheel(256, 64 * 1024, 256)]
        public int Block { get; set; } = 1024;

        [NumberWheel(5, 200, 5)]
        public int Count { get; set; } = 10;

        public TimeSpan Expire { get; set; } = new TimeSpan(7, 0, 0, 0);

        public Hash newHash() => new Hash
        {
            mode = Mode,
            type = Type,
            block = Block,
            count = Count,
            expire = Expire,
        };

        public override string ToString()
            => $"{Mode}";
    }
}
