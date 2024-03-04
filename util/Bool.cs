using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public static class Bool
    {
        public static void ifdo(this bool cond, Action func)
        {
            if (cond)
                func();
        }

        public static void truedo(this bool cond, Action func)
        {
            if (cond)
                func();
        }

        public static void falsedo(this bool cond, Action func)
        {
            if (!cond)
                func();
        }

        public static bool and(this bool a, bool b)
            => a && b;

        public static bool or(this bool a, bool b)
            => a || b;
    }
}
