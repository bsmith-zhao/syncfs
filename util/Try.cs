using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class Try
    {
        public static Action<Exception> notify = showMessage;

        static void showMessage(Exception err)
            => err.Message.msg();

        public static void handle(this Exception err)
        {
            err.log();
            notify?.Invoke(err);
        }

        public static void trydo(this object src, Action func)
        {
            try { func(); }
            catch (Exception err)
            {
                err.handle();
            }
        }

        public static T tryget<T>(this object src, Func<T> func)
        {
            try { return func(); }
            catch (Exception err)
            {
                err.handle();
            }
            return default(T);
        }

        public static T trygetQuiet<T>(this object src, 
            Func<T> func)
        {
            try
            {
                return func();
            }
            catch { }
            return default(T);
        }
    }
}
