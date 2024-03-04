using util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using util.rep;
using util.ext;

namespace sync.hash
{
    public class HashItem : FileItem
    {
        public string code;
        public long hashTime;

        string _lowPath;
        public string lowPath => _lowPath ?? (_lowPath = path.low());

        public List<HashItem> copys;
        public int copyCount => 1 + (copys?.Count ?? 0);

        public new string path
        {
            get => base.path;
            set
            {
                base.path = value;
                _lowPath = null;
            }
        }

        public bool samePath(HashItem other)
            => this.lowPath == other.lowPath
            && this.name == other.name;
    }
}
