using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;

namespace util.prop
{
    public class EditByWheel : Attribute
    {
        double delta;

        public EditByWheel(object delta)
        {
            this.delta = $"{delta}".num();
        }

        public bool modify(object src, bool add, out object dst)
        {
            dst = null;
            try
            {
                var value = $"{src}".num();
                if (add)
                    value += delta;
                else
                    value -= delta;
                dst = Convert.ChangeType(value, src.GetType());
            }
            catch { }
            return dst != null;
        }
    }
}
