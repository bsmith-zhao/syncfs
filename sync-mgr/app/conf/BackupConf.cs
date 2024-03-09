using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;
using util.prop.edit;
using util.rep;

namespace sync.app
{
    [TypeConverter(typeof(ExpandClass))]
    public class BackupConf
    {
        public bool Enable { get; set; } = false;

        const string DefaultFolder = "(bak)";
        [UnifyPathName(DefaultFolder)]
        public string Folder { get; set; } = DefaultFolder;

        public bool KeepAll { get; set; } = false;

        [NumberWheel(1, 10, 1)]
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
