using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.prop
{
    public abstract class AdjustValue : Attribute
    {
        public abstract object adjust(object value);
    }
}
