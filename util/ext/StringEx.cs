using util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;

namespace util.ext
{
    public static class StringEx
    {
        public static char charAt(this string str, int idx)
            => idx >= 0 && idx < str?.Length 
            ? str[idx] : default(char);

        public static bool empty(this string str)
            => str == null || str.Length == 0;

        public static string str(this object obj)
            => obj?.ToString();

        public static string str<T>(this T obj, Func<T, string> func)
            => func(obj);

        public static string low(this string str)
            => str?.ToLower();

        public static string[] low(this string[] src)
        {
            if (src == null)
                return null;
            var dst = new string[src.Length];
            src.each((i, s) => dst[i] = s.low());
            return dst;
        }

        public static string jump(this string str, int count)
            => str?.Length >= count ? str.Substring(count) : null;

        public static string cut(this string str, int count)
            => str?.Length >= count
            ? str.Substring(0, str.Length - count)
            : null;

        public static string conv(this string str, Func<string, string> func)
            => str != null ? func(str) : null;

        //public static string keep(this string str, Func<string, bool> func)
        //    => func(str) ? str : null;

        public static string join<T>(this IEnumerable<T> iter,
            string sep)
            => iter == null ? null : string.Join(sep, iter);

        public static byte[] utf8(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static string utf8(this byte[] data)
        {
            if (data == null)
                return null;
            return Encoding.UTF8.GetString(data);
            //return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static string utf8(this byte[] data, int offset, int count)
        {
            if (data == null)
                return null;
            return Encoding.UTF8.GetString(data, offset, count);
        }

        //static Encoding GBK = Encoding.GetEncoding("GBK");
        //public static byte[] gbk(this string str)
        //{
        //    return GBK.GetBytes(str);
        //}

        //public static bool isGbk(this string str, out byte[] data)
        //{
        //    data = str.gbk();
        //    return data.gbk() == str;
        //}

        //public static string gbk(this byte[] data)
        //{
        //    return GBK.GetString(data);
        //}

        public static char last(this string str)
            => str?.Length > 0 ? str[str.Length - 1]
            : default(char);

        public static int lastIdx(this string str, char c, int count)
        {
            int idx = str.Length;
            while (count-- > 0 && idx-- > 0)
                if (str[idx] == c)
                    return idx;
            return -1;
        }

        public static string tail(this string str, int count)
            => str.Substring(str.Length - count);
    }
}
