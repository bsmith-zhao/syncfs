using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class IEnumerableEx
    {
        public static void count(this int max,
            Action<int> func)
        {
            int n = 0;
            while (n < max)
                func(n++);
        }

        public static IEnumerable<int> count(this int max)
        {
            int n = 0;
            while (n < max)
                yield return n++;
        }

        public static IEnumerable<T> usable<T>(this IEnumerable<T> iter)
            => iter ?? Enumerable.Empty<T>();

        public static IEnumerable<T> combine<T>(this IEnumerable<T> first,
            IEnumerable<T> second, params IEnumerable<T>[] others)
        {
            if (first != null)
                foreach (var e in first)
                    yield return e;
            if (second != null)
                foreach (var e in second)
                    yield return e;
            foreach (var other in others)
                if (other != null)
                    foreach (var e in other)
                        yield return e;
        }

        public static IEnumerable<T> exclude<T>(this IEnumerable<T> iter, Func<T, bool> func)
        {
            if (iter != null)
                foreach (T elem in iter)
                    if (!func(elem))
                        yield return elem;
        }

        public static IEnumerable<T> pick<T>(this IEnumerable<T> iter, Func<T, bool> func)
        {
            if (iter != null)
                foreach (T elem in iter)
                    if (func(elem))
                        yield return elem;
        }

        public static IEnumerable<T> pick<T>(this IEnumerable iter, Func<T, bool> func)
        {
            if (iter != null)
                foreach (T elem in iter)
                    if (func(elem))
                        yield return elem;
        }

        public static IEnumerable<O> conv<I, O>(
            this IEnumerable iter, 
            Func<I, O> func)
        {
            if (iter != null)
                foreach (I e in iter)
                    yield return func(e);
        }

        public static IEnumerable<O> conv<I, O>(
            this IEnumerable<I> iter, 
            Func<I, O> func)
        {
            if (iter != null)
                foreach (I e in iter)
                    yield return func(e);
        }

        public static T first<T>(this IEnumerable iter,
            Func<T, bool> func)
        {
            if (iter != null)
                foreach (T e in iter)
                    if (func(e))
                        return e;
            return default(T);
        }

        public static bool first<T>(this IEnumerable iter,
            Func<T, bool> func, out T item)
        {
            if (iter != null)
                foreach (T e in iter)
                    if (func(e))
                    {
                        item = e;
                        return true;
                    }
            item = default(T);
            return false;
        }

        public static T first<T>(this IEnumerable<T> iter, 
            Func<T, bool> func)
        {
            if (iter != null)
                foreach (T e in iter)
                    if (func(e))
                        return e;
            return default(T);
        }

        public static bool first<T>(this IEnumerable<T> iter, Func<T, bool> func, out T res)
        {
            if (null != iter)
                foreach (T e in iter)
                    if (func(e))
                    {
                        res = e;
                        return true;
                    }
            res = default(T);
            return false;
        }

        public static T first<T>(this IEnumerable<T> iter)
            => iter != null ? iter.FirstOrDefault() : default(T);

        public static List<T> newList<T>(this IEnumerable<T> iter)
        {
            if (iter == null)
                return new List<T>();
            return new List<T>(iter);
        }

        public static List<T> toList<T>(this IEnumerable iter)
        {
            if (iter == null)
                return new List<T>();
            return new List<T>(iter.OfType<T>());
        }

        //public static List<T> newList<T>(this IEnumerable<T> iter)
        //{
        //    if (iter == null)
        //        return new List<T>();
        //    return new List<T>(iter.OfType<T>());
        //}

        public static List<T> newList<T>(this IEnumerable iter)
        {
            if (iter == null)
                return new List<T>();
            return new List<T>(iter.OfType<T>());
        }

        public static IEnumerable each<T>(this IEnumerable iter, Action<T> func)
        {
            if (null != iter)
                foreach (T e in iter)
                    func(e);
            return iter;
        }

        public static int index<T>(this IEnumerable<T> iter, Func<T, bool> func)
        {
            int idx = 0;
            if (iter != null)
                foreach (var e in iter)
                {
                    if (func(e))
                        return idx;
                    idx++;
                }
            return -1;
        }

        public static IEnumerable<T> conv<T>(this IEnumerable iter)
        {
            if (iter != null)
                foreach (T e in iter)
                    yield return e;
        }

        public static IEnumerable<T> each<T>(
            this IEnumerable<T> iter, Action<T> func)
        {
            if (null != iter)
                foreach (var e in iter)
                    func(e);
            return iter;
        }

        public static Dictionary<K, V> toMap<K, V>(
            this IEnumerable<V> iter, 
            Action<V, Dictionary<K,V>> func)
        {
            var map = new Dictionary<K, V>();
            foreach (var e in iter)
                func(e, map);
            return map;
        }

        public static Dictionary<K, V> toMap<K, V>(
            this IEnumerable<V> iter, Func<V, K> func)
        {
            var map = new Dictionary<K, V>();
            foreach (var e in iter)
                map[func(e)] = e;
            return map;
        }

        public static bool exist<T>(this IEnumerable<T> iter, Func<T, bool> func)
        {
            if (null != iter)
                foreach (var e in iter)
                    if (func(e))
                        return true;
            return false;
        }

        public static bool exist<T>(this IEnumerable iter, Func<T, bool> check)
        {
            if (null != iter)
                foreach (T e in iter)
                    if (check(e))
                        return true;
            return false;
        }
    }
}
