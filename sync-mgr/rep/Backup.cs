using util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public class Backup
    {
        public bool enable = true;
        public string dir = "(bak)";
        public bool all = false;
        public int rev = 3;

        public override string ToString()
            => this.desc();
    }
}
