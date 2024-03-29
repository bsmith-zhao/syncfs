﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class Try
    {
        public static Action<Exception> output = showMessage;

        static void showMessage(Exception err)
            => err.Message.msg();

        public static void notify(this Exception err, 
            string func = null, object args = null)
        {
            func = func ?? true.lastFunc();
            err.log(func, args);
            output?.Invoke(err);
        }

        public static void trylog(this bool src,
            Action func)
        {
            try { func(); }
            catch (Exception err)
            {
                err.log(true.lastFunc());
            }
        }

        public static void trydo(this bool src, Action func)
        {
            try { func(); }
            catch (Exception err)
            {
                err.notify(true.lastFunc());
            }
        }

        public static T tryget<T>(this bool src, Func<T> func)
        {
            try { return func(); }
            catch (Exception err)
            {
                err.notify(true.lastFunc());
            }
            return default(T);
        }

        public static T trygetQuiet<T>(this bool src, 
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
