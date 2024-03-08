using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;

namespace vfs.mgr.conf
{
    [TypeConverter(typeof(ExpandClass))]
    public class BackupConf
    {
        public bool Enable { get; set; } = true;

        //string _folder = DefaultFolder;
        //public string Folder
        //{
        //    get => _folder;
        //    set => _folder = value.pathUnifyName() 
        //        ?? DefaultFolder;
        //}

        const string DefaultFolder = "(bak)";
        [UnifyPathName(DefaultFolder)]
        public string Folder { get; set; } = DefaultFolder;

        public override string ToString() => "";

        public string getFolder() 
            => Enable ? Folder : null;
    }
}
