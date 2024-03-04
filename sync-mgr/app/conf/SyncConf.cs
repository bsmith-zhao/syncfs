using Newtonsoft.Json;
using sync.sync;
using sync.work;
using System.ComponentModel;
using util.ext;

namespace sync.app
{
    public class SyncConf
    {
        [Category("Sync"), ReadOnly(true), PasteIgnore]
        public string Type { get; set; }

        [Category("Sync")]
        public CompareConf Compare { get; set; } 
            = new CompareConf();

        [Category("Sync")]
        public HashConf Hash { get; set; } 
            = new HashConf();
    }
}
