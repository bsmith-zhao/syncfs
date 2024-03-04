using System;

namespace util
{
    public static class Msg
    {
        public static Action<object> output = Console.WriteLine;

        public static void msg(this object obj)
            => output?.Invoke(obj);

        public static void msgj(this object obj)
            => output?.Invoke(obj?.json() ?? "<null>");

        public static void msgRecover(this object obj, Action func)
        {
            var fout = output;
            try
            {
                func();
            }
            finally
            {
                output = fout;
            }
        }

        public static void recover(Action func)
        {
            var fout = output;
            try
            {
                func();
            }
            finally
            {
                output = fout;
            }
        }
    }
}
