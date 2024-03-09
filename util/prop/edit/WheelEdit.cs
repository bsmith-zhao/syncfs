using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.prop.edit
{
    public abstract class WheelEdit : AdjustValue
    {
        public abstract bool next(object src, 
            out object dst, bool up);
    }
}
