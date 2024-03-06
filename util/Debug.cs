using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public static class Debug
    {
#if DEBUG
        // debug stuff goes here
#else
  // release stuff goes here
#endif
        public static Action<object> output = Console.WriteLine;

        public static void debug(this object src)
        {
            output?.Invoke($"[debug]{src}");
        }

        public static void debugj(this object src)
            => output?.Invoke($"[debug]{src.json()}");
    }
}
