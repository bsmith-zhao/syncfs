using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;
using util.rep;
using vfs.rep;

namespace vfs.mgr.conf
{
    public class VfsConf
    {
        [UnifyPath]
        public string Path { get; set; } = "V:";

        public string Name { get; set; } = "VFS";

        [ReadOnly(true)]
        public RepType Type { get; set; } = RepType.AeadFS;

        object _src;
        [TypeConverter(typeof(ExpandProp))]
        public object Source
        {
            get => _src is RepConf ra ? ra
                : (_src = _src.json().obj(argsClass())) as RepConf;
            set => _src = value;
        }

        Type argsClass()
        {
            switch (Type)
            {
                case RepType.AeadFS:
                    return typeof(AeadRepConf);
            }
            return null;
        }

        public RepConf getRepConf() => Source as RepConf;
        public RepArgs newRepArgs() => getRepConf().newRepArgs();

        public string sourcPath()
            => getRepConf().getSource();

        public string mountInfo()
            => $"{Path} <{Name}>";
    }
}
