using System;

namespace util
{
    public static class Msg
    {
        public static Action<object> output = Console.WriteLine;

        public static void msg(this object src)
            => output?.Invoke($"{src}");

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
