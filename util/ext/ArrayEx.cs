using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;

namespace util.ext
{
    public static class ArrayEx
    {
        public static T[] keep<T>(this T[] arr, Func<T[], bool> func)
            => func(arr) ? arr : null;

        public static bool empty<T>(this T[] arr)
            => arr == null || arr.Length == 0;

        public static bool first<T>(this T[] iter, Func<T, bool> func, out int idx)
        {
            idx = -1;
            for(int i = 0; i< iter.Length;i ++)
            {
                if (func(iter[i]))
                {
                    idx = i;
                    return true;
                }
            }
            return false;
        }

        public static T[] reverse<T>(this T[] arr)
        {
            Array.Reverse(arr);
            return arr;
        }

        public static T[] set<T>(this T[] arr, int idx, IEnumerable<T> items)
        {
            if (null != items)
            {
                foreach (var it in items)
                {
                    arr[idx++] = it;
                }
            }
            return arr;
        }

        public static T[] set<T>(this T[] arr, int idx, List<T> items)
        {
            items?.CopyTo(arr, idx);
            return arr;
        }

        public static void set<T>(this T[] arr, Func<int, T> func)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = func(i);
            }
        }

        public static T[] set<T>(this T[] arr, T value)
        {
            return arr.set(0, arr.Length, value);
        }

        public static T[] set<T>(this T[] arr, int idx, T value)
        {
            arr[idx] = value;
            return arr;
        }

        public static T[] set<T>(this T[] arr, int off, int cnt, T value)
        {
            while (cnt-- > 0)
            {
                arr[off++] = value;
            }
            return arr;
        }

        public static D[] conv<S, D>(this S[] src, Func<S, D> func)
        {
            if (null == src)
                return null;

            var dst = new D[src.Length];
            for (int i = 0; i < src.Length; i++)
            {
                dst[i] = func(src[i]);
            }
            return dst;
        }

        public static T[] each<T>(this T[] arr, Action<int, T> func)
        {
            if (arr != null)
                for (int i = 0; i < arr.Length; i++)
                    func(i, arr[i]);
            return arr;
        }

        public static T item<T>(this T[] arr, int idx)
            => arr?.Length > idx ? arr[idx] : default(T);

        public static T[] append<T>(this T[] src, params T[] items)
        {
            var arr = new List<T>();
            if (null != src)
                arr.AddRange(src);
            if (null != items)
                arr.AddRange(items);
            return arr.ToArray();
        }

        public static T[] delete<T>(this T[] arr, Func<T, bool> func)
        {
            if (null == arr)
                return null;

            var list = new List<T>();
            foreach (var elem in arr)
            {
                if (func(elem))
                    continue;
                list.Add(elem);
            }
            return list.ToArray();
        }

        public static T[] delete<T>(this T[] arr, T item) where T : IComparable
        {
            if (null == arr)
                return arr;

            var list = new List<T>();
            foreach (var elem in arr)
            {
                if (elem == null && item == null)
                    continue;
                if (elem?.CompareTo(item) == 0)
                    continue;
                list.Add(elem);
            }
            return list.ToArray();
        }

        public static T[] merge<T>(this T[] src, params T[][] arrs)
        {
            var list = new List<T>();
            if (null != src)
                list.AddRange(src);
            foreach (var arr in arrs)
            {
                if (null != arr)
                    list.AddRange(arr);
            }
            return list.ToArray();
        }

        public static T last<T>(this T[] arr)
            => arr[arr.Length - 1];

        public static T[] head<T>(this T[] src, int count)
        {
            return src.Take(count).ToArray();
        }

        public static T[] tail<T>(this T[] arr, int count)
        {
            if (arr.Length <= count)
                return arr;
            return arr.Skip(arr.Length - count).ToArray();
        }

        public static T[] sub<T>(this T[] src, int offset, int count)
        {
            var dst = new T[count];
            if (count == 0)
                return dst;
            Array.Copy(src, offset, dst, 0, count);
            return dst;
        }

        public static T[] skip<T>(this T[] src, int count)
        {
            return src.Skip(count).ToArray();
        }

        public static T[] fix<T>(this T[] src, int count)
        {
            if (src.Length == count)
                return src;
            else if (src.Length > count)
                return src.head(count);
            var dst = new T[count];
            Array.Copy(src, dst, src.Length);
            return dst;
        }
    }
}
