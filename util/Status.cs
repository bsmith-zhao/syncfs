using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public static class Status
    {
        public static Action<string> output;

        public static void status(this string msg)
        {
            output?.Invoke(msg);
        }
    }
}
