using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.rep;

namespace vfs.rep
{
    public class NormalDirArgs : RepArgs
    {
        public string dir;

        public override Reposit newRep()
            => new NormalDirReposit(dir);
    }
}
