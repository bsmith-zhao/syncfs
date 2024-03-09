using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.prop.edit
{
    public class NumberWheel : WheelEdit
    {
        double min;
        double max;
        double delta;

        public NumberWheel(double min, double max,
            double delta)
        {
            this.min = min;
            this.max = max;
            this.delta = delta;
        }

        double def => (max - min) / 2;

        public override object adjust(object value)
            => convert(value, $"{value}".f64(def)
                .limit(min, max));

        object convert(object src, double num)
        {
            if (src is int) return (int)num;
            if (src is long) return (long)num;
            if (src is short) return (short)num;
            if (src is float) return (float)num;
            return num;
        }

        public override bool next(object src, out object dst, bool up)
        {
            var old = $"{src}".f64(def);
            var num = old + (up ? delta : -delta);
            num = num.limit(min, max);
            dst = convert(src, num);
            return num != old;
        }
    }
}
