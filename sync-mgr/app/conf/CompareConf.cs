using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;

namespace sync.app
{
    [TypeConverter(typeof(ExpandProp))]
    public class CompareConf
    {
        public CompareType Default { get; set; } 
            = CompareType.Hash;

        string[] hashs;
        [TypeConverter(typeof(ArrayProp))]
        public string[] ByHash
        {
            get => hashs;
            set => hashs = value.pathUnify();
        }

        string[] datas;
        [TypeConverter(typeof(ArrayProp))]
        public string[] ByData
        {
            get => datas;
            set => datas = value.pathUnify();
        }

        public TimeSpan Expire { get; set; } = new TimeSpan(7, 0, 0, 0);

        public override string ToString()
            => new string[]
            {
                $"{Default}",
                others.join(",").conv(s => $"{other}[{s}]"),
            }.exclude(s => s.empty()).join(",");

        CompareType other 
            => Default == CompareType.Hash 
            ? CompareType.Data : CompareType.Hash;

        string[] others
            => Default == CompareType.Hash ? datas : hashs;

        public Compare newCompare()
            => new Compare(Default, ByHash, ByData)
            {
                expire = Expire,
            };
    }
}
