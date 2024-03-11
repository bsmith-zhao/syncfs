using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using vfs.rep;

namespace vfs.mgr.conf
{
    public class DirRepConf : RepConf
    {
        string dir;
        [ReadOnly(true)]
        public string Path { get => dir; set => dir = value; }

        public override bool canModifyPwd()
            => false;

        public override string getSource()
            => Path;

        public override RepArgs newRepArgs()
            => new NormalDirArgs { dir = dir };

        public bool createRep(out string dir)
        {
            if (!true.pickDir(out dir))
                return false;

            Path = dir;
            return true;
        }

        public override bool createRep()
            => true.pickDir(out dir);

        public override string vfsName()
            => dir.pathName();
    }
}
