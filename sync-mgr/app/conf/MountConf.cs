using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;

namespace sync.app.conf
{
    [TypeConverter(typeof(ExpandProp))]
    public class MountConf
    {
        [UnifyPath]
        public string Path { get; set; } = "V:";
        public string Name { get; set; } = "VFS";

        public override string ToString()
            => $"{Name} ({Path})";
    }
}
