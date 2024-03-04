using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class ColorEx
    {
        public static Color gray(this int value)
            => Color.FromArgb(value, value, value);

        public static Color zoom(this Color c, double r)
            => Color.FromArgb((int)(c.R * r), (int)(c.G * r), (int)(c.B * r));
    }
}
