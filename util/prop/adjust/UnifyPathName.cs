using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.prop
{
    public class UnifyPathName : AdjustValue
    {
        string def;

        public UnifyPathName(string @default = null)
            => this.def = @default;

        public override object adjust(object value)
            => $"{value}".pathUnifyName() ?? def;
    }
}
