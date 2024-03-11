using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;
using vfs.rep;

namespace vfs.mgr.conf
{
    [TypeConverter(typeof(ExpandClass))]
    public abstract class RepConf
    {
        public abstract bool createRep();
        public abstract string getSource();
        public abstract RepArgs newRepArgs();
        public abstract bool canModifyPwd();
        public virtual void modifyPwd() { }
        public abstract string vfsName();

        public override string ToString() => "";

        public static Type confClass(RepType type)
        {
            switch (type)
            {
                case RepType.NormalDir:
                    return typeof(DirRepConf);
                case RepType.AeadFS:
                    return typeof(AeadRepConf);
            }
            return null;
        }

        public static RepConf newConf(RepType type)
            => confClass(type).@new() as RepConf;
    }
}
