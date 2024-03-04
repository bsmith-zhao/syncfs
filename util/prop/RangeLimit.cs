using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.prop
{
    public class RangeLimit : Attribute
    {
        object min;
        object max;

        double minValue;
        double maxValue;

        public RangeLimit(object min, object max)
        {
            minValue = $"{min}".num();
            maxValue = $"{max}".num();

            this.min = min;
            this.max = max;
        }

        //public bool limit(object src, out object dst)
        //{
        //    dst = null;
        //    var value = $"{src}".num();
        //    if (value < minValue)
        //        dst = min;
        //    else if (value > maxValue)
        //        dst = max;
        //    return dst != null;
        //}

        public bool limit(object src, Action<object> func)
        {
            var value = $"{src}".num();
            if (value < minValue)
                func(min);
            else if (value > maxValue)
                func(max);
            else
                return false;
            return true;
        }
    }
}
