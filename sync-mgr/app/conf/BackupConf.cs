using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;
using util.rep;

namespace sync.app
{
    [TypeConverter(typeof(ExpandProp))]
    public class BackupConf
    {
        public bool Enable { get; set; } = false;
        [UnifyPath]
        public string Directory { get; set; } = "(bak)";
        public bool KeepAll { get; set; } = false;
        [RangeLimit(1, 10), EditByWheel(1)]
        public int Reversion { get; set; } = 3;

        public override string ToString() 
            => Enable ? $"{Directory} : {(KeepAll?"all":$"{Reversion}")}" : "";

        public Backup create() => new Backup
        {
            enable = Enable,
            dir = Directory,
            rev = Reversion,
            all = KeepAll,
        };
    }
}
