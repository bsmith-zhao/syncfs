using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public delegate void LogOutput(params string[] msgs);

    public static class Log
    {
        public static LogOutput output;

        public static void log(this Exception err)
            => output?.Invoke($"[{DateTime.Now}]{err.Message}", 
                err.StackTrace);
    }
}
