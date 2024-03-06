using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.prop;
using util.rep;
using vfs.rep;

namespace sync.app.conf
{
    public class DirRepConf : RepConf
    {
        string dir => Path;
        [Editor(typeof(DirPicker), typeof(UITypeEditor))]
        [Category("1.Reposit"), UnifyPath]
        public string Path { get; set; }

        public override Reposit openRep()
        {
            if (!exist())
                throw new Error<NormalDirReposit>("NotExist", dir);
            return new NormalDirReposit(dir);
        }
        public override string ToString() => dir;
        public override bool exist() => dir.dirExist();
        public override bool canModifyPwd() => false;

        public override RepArgs newRepArgs()
            => new NormalDirArgs { dir = dir };

        public override string getSource()
            => Path;

        public override string createRep()
            => (Path = true.pickDir()).pathName();
    }
}
