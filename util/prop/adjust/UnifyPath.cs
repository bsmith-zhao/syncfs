using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.prop
{
    public class UnifyPath : AdjustValue
    {
        string def;

        public UnifyPath(string @default = null)
            => this.def = @default;

        public override object adjust(object value)
            => $"{value}".pathUnify() ?? def;
    }
}
