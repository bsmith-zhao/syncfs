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
        const string DefaultFolder = "(bak)";

        public bool Enable { get; set; } = false;

        string _folder = DefaultFolder;
        public string Folder
        {
            get => _folder;
            set => _folder = value.pathUnifyName()
                ?? DefaultFolder;
        }

        public bool KeepAll { get; set; } = false;
        [RangeLimit(1, 10), EditByWheel(1)]
        public int Reversion { get; set; } = 3;

        public override string ToString() 
            => Enable ? $"{Folder} : {(KeepAll?"all":$"{Reversion}")}" : "";

        public Backup create() => new Backup
        {
            enable = Enable,
            dir = Folder,
            rev = Reversion,
            all = KeepAll,
        };

        public string getFolder()
            => Enable ? Folder : null;
    }
}
