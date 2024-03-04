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
        public static Action<object> output = Console.WriteLine;

        public static void debug(this object msg)
        {
            output?.Invoke($"[debug]{msg?.ToString() ?? "<null>"}");
        }

        public static void debugj(this object msg)
        {
            output?.Invoke($"[debug]{msg?.json() ?? "<null>"}");
        }
    }
}
