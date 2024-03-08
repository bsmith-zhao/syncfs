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
    [TypeConverter(typeof(ExpandClass))]
    public abstract class RepConf
    {
        public abstract string getSource();
        public abstract RepArgs newRepArgs();
        public abstract bool canModifyPwd();
        public virtual void modifyPwd() { }

        public override string ToString() => "";
    }
}
