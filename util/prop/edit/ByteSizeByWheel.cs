using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.prop.edit
{
    public class ByteSizeByWheel : WheelEdit
    {
        long min;
        long max;
        long delta;
        long def;

        public ByteSizeByWheel(string min, string max, 
            string delta = null, 
            string def = null)
        {
            this.min = min.byteSize();
            this.max = max.byteSize();
            this.delta = (delta != null) ? delta.byteSize()
                : (this.max - this.min) / 20;
            this.def = (def != null) ? def.byteSize()
                : (this.max - this.min) / 2;
        }

        public override object adjust(object value)
        {
            return $"{value}".byteSize(def)
                .limit(min, max).byteSize();
        }

        public override bool next(object src, out object dst, bool up)
        {
            var old = $"{src}".byteSize(def);
            var size = old + (up ? delta : -delta);
            size = size.limit(min, max);
            dst = size.byteSize();
            return size != old;
        }
    }
}
