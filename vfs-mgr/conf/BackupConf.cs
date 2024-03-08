using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;

namespace vfs.mgr.conf
{
    [TypeConverter(typeof(ExpandProp))]
    public class BackupConf
    {
        const string DefaultFolder = "(bak)";

        public bool Enable { get; set; } = true;

        string _folder = DefaultFolder;
        public string Folder
        {
            get => _folder;
            set => _folder = value.pathUnifyName() 
                ?? DefaultFolder;
        }

        public override string ToString() => "";

        public string getFolder() 
            => Enable ? Folder : null;
    }
}
