using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.rep;
using util.rep.aead;

namespace vfs.rep
{
    public class AeadFsArgs : RepArgs
    {
        public string path;
        public byte[] mkey;

        public override Reposit newRep()
        {
            var conf = AeadFsConf.load(path);
            conf.setMKey(mkey);

            return new AeadFsReposit(path.pathDir(), conf);
        }
    }
}
