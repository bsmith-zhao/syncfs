using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public static class Log
    {
        public static Action<string> output;

        public static void log(this object src)
            => output?.Invoke($"[{logTime}]{src}");

        public static void log(this Exception err)
            => output?.Invoke($"[{logTime}]{err.Message}\r\n{err.StackTrace}");

        static object logTime
            => DateTime.Now;
    }
}
