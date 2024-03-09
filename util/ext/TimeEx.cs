using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class TimeEx
    {
        public const long TicksPerMS = 10000;
        public const string UnifyFormat = "yyyy/MM/dd HH:mm:ss";
        public const string LongMsFormat = "yyyyMMddHHmmssfff";

        public static string text(this DateTime t) 
            => t.ToString(UnifyFormat);

        public static DateTime time(this string t) 
            => DateTime.ParseExact(t, UnifyFormat, null);

        public static long totalMs(this DateTime t) 
            => t.Ticks / TicksPerMS;

        public static long longMs(this DateTime t) 
            => long.Parse(t.ToString(LongMsFormat));

        public static int ticks(this bool src)
            => Environment.TickCount;
    }
}
