using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class ListEx
    {
        public static bool has<T>(this List<T> list, T item)
            => list?.Contains(item) == true;

        public static bool empty<T>(this List<T> list)
            => null == list || list.Count == 0;

        public static List<T> copy<T>(this List<T> list)
            => new List<T>(list);

        public static T last<T>(this List<T> list)
            => list?.Count > 0 ? list[list.Count - 1] : default(T);

        public static T last<T>(this List<T> list, Func<T, bool> cond)
        {
            int n = list?.Count ?? 0;
            while (n-- > 0)
            {
                if (cond(list[n]))
                    return list[n];
            }
            return default(T);
        }

        public static List<T[]> reverse<T>(this List<T[]> list)
        {
            list.each(p => p.reverse());
            return list;
        }

        public static List<R> conv<T, R>(this List<T> list, Func<T, R> func)
        {
            if (list == null)
                return null;
            var res = new List<R>();
            foreach (var elem in list)
                res.Add(func(elem));
            return res;
        }

        public static string join<T>(this List<T> list, string sep)
            => list.ToArray().conv(e => e?.ToString()).join(sep);

        public static string join(this List<string> list, string sep)
            => list.ToArray().join(sep);

        public static List<T> add<T>(this List<T> list, T item)
        {
            list = list ?? new List<T>();
            list.Add(item);
            return list;
        }

        public static List<T> add<T>(this List<T> list, IEnumerable<T> items)
        {
            list = list ?? new List<T>();
            if (null != items)
                list.AddRange(items);
            return list;
        }

        public static List<T> limit<T>(this List<T> list, int size)
        {
            if (list.Count > size)
                list.RemoveRange(size, list.Count - size);
            return list;
        }
    }
}
