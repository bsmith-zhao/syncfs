using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public static class TestEx
    {
        public static int index = 1;

        public static void showHex(this byte[] data, string name)
        {
            $"{name} size: {data.Length}, {data.hex()}".msg();
        }

        public static void showB64(this byte[] data, string name)
        {
            $"{name} size: {data.Length}, {data.b64()}".msg();
        }

        public static void evalTime(this string name, Action func)
        {
            var begin = DateTime.UtcNow;
            $"{name} begin".msg();
            func();
            var span = DateTime.UtcNow - begin;
            $"{name} cost: {span}".msg();
        }

        public static void begin()
        {
            index = 1;
            $"\r\n[start]".msg();
        }

        public static void @case(this string msg)
        {
            $"<{index++}>{msg}".msg();
        }

        public static void assert(this bool @true)
        {
            if (!@true)
                throw new Exception("cond != true, assert fail!!");
        }

        public static void assertError(this Action func)
        {
            func.showTry();
            invokeError(func);
        }

        public static void assertError<T>(this Action<T> func, T a1)
        {
            func.showTry(a1);
            invokeError(() => func(a1));
        }

        public static void assertError<T1, T2>(this Action<T1, T2> func, T1 a1, T2 a2)
        {
            func.showTry(a1, a2);
            invokeError(() => func(a1, a2));
        }

        public static void showCall(this Delegate func, params object[] args)
        {
            $"call {func.Method.Name}({args.json().jump(1).cut(1)})".msg();
        }

        public static void showTry(this Delegate func, params object[] args)
        {
            $"try {func.Method.Name}({args.json().jump(1).cut(1)})".msg();
        }

        static void invokeError(Action func)
        {
            Exception err = null;
            try { func(); }
            catch (Exception e) { err = e; }
            if (null == err)
                throw new Exception("no error throw, assert fail!!");
            else
                $"throw <{err.GetType().Name}>[{err.Message}]".msg();
        }
    }
}
