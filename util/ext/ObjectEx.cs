using util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using util.prop;

namespace util.ext
{
    public class PasteIgnore : Attribute
    {
    }

    public static class ObjectEx
    {
        public static T use<T>(this T obj, Action<T> func)
        {
            func(obj);
            return obj;
        }

        public static bool paste(this object dst, object src,
            Action<string> notify = null)
        {
            bool update = false;
            var srcCls = src.GetType();
            dst.GetType().members().each(df =>
            {
                if (df.canGet && !df.mark<PasteIgnore>()
                    && srcCls.member(df.name, out var sf)
                    && sf.canSet && df.canAssign(sf))
                {
                    df.set(dst, sf.get(src));
                    notify?.Invoke(df.name);
                    update = true;
                }
            });
            return update;
        }

        public static object free<T>(this object src, ref T res)
        {
            if (res is IDisposable d)
                d.Dispose();
            res = default(T);
            return src;
        }

        public static object prop<T>(this T obj, string name)
        {
            return typeof(T).GetProperty(name).GetValue(obj);
        }

        public static T prop<T>(this T obj, string name, object value)
        {
            typeof(T).GetProperty(name).SetValue(obj, value);
            return obj;
        }

        public static bool prop(this Type cls, string name,
                                out PropertyInfo fld)
            => (fld = cls.GetProperty(name)) != null;

        public static bool fld(this Type cls, string name,
                                out FieldInfo fld)
            => (fld = cls.GetField(name)) != null;

        public static bool member(this Type cls, string name,
                                out Member mem)
            => (mem = cls.fld(name, out var f) ? new Member(f)
            : (cls.prop(name, out var p) ? new Member(p)
            : null)) != null;

        public static IEnumerable<Member> members(this Type cls)
        {
            foreach (var f in cls.GetFields())
                yield return new Member(f);
            foreach (var p in cls.GetProperties())
                yield return new Member(p);
        }

        public static void update<T>(this T value, ref T obj, Action func)
        {
            if (obj?.Equals(value) == true)
                return;
            obj = value;
            func();
        }

        public static void update<T>(this T value, ref T fld, Action<T> before, Action<T> after)
        {
            if (fld?.Equals(value) == true)
                return;
            T old = fld;
            before?.Invoke(value);
            fld = value;
            after?.Invoke(old);
        }

        public static object @new(this Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
