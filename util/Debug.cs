using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public static class Debug
    {
        public static Action<object> output = showMessage;

        static void showMessage(object obj)
            => obj.msg();

        public static void debug(this object src)
        {
            output?.Invoke($"[debug]<{true.lastFunc()}>{src}");
        }

        public static void debugj(this object src)
            => output?.Invoke($"[debug]{src.json()}");
    }
}
