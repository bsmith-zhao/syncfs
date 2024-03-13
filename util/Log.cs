using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public static class Log
    {
        public static Action<string> output;

        public static void log(this object src)
            => output?.Invoke($"\r\n[{logTime}]{src}");

        public static void log(this Exception err, string func = null, object args = null)
        {
            if (output == null)
                return;
            func = func ?? true.lastFunc();
            log($"\r\n[{func}]({args})<{err.TargetSite.shortName()}>{err.Message}");
#if DEBUG
            output?.Invoke(err.StackTrace);
#endif
        }

        static object logTime => DateTime.Now;

        public static void trace(this object obj)
        {
            obj.msg();
            obj.log();
        }
    }
}
