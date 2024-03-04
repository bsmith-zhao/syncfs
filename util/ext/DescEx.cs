using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class DescEx
    {
        public static string desc(this object obj,
            Func<object, object> conv = null)
            => mergeFlds(obj, conv);

        public static string desc<T>(this object obj,
            Func<object, object> conv = null)
            => mergeFlds(obj, conv, typeof(T));

        static string mergeFlds(this object obj,
            Func<object, object> conv, Type owner = null)
            => $"{{\r\n{string.Join(",\r\n", enumFlds(obj, conv, owner))}\r\n}}";

        static IEnumerable<string> enumFlds(object obj,
            Func<object, object> conv, Type owner = null)
        {
            var cls = obj.GetType();
            owner = owner ?? cls;
            if (owner.Namespace != null)
                yield return $"  <class>: {owner.FullName}";
            string s;
            foreach (var f in cls.GetFields())
            {
                if ((s = f.GetValue(obj).toText(conv)) == null)
                    continue;
                yield return makeFld(f.Name, s);
            }
            foreach (var f in cls.GetProperties())
            {
                if (!f.CanRead
                    || (s = f.GetValue(obj).toText(conv)) == null)
                    continue;
                yield return makeFld(f.Name, s);
            }
        }

        static string makeFld(string name, string value)
        {
            if (value.StartsWith("{") && value.EndsWith("}"))
                return string.Join("\r\n", splitFld(name, value));
            else
                return $"  {name}: {value}";
        }

        static IEnumerable<string> splitFld(string name, string value)
        {
            yield return $"  {name}:";
            foreach (var s in value.Split('\r', '\n'))
                if (s.Length > 0)
                    yield return $"  {s}";
        }

        static string toText(this object v,
            Func<object, object> conv = null)
        {
            if (conv != null)
                v = conv(v);
            if (v == null)
                return null;
            if (v is IDesc dv)
                return dv.getDesc();
            if (v is string)
                return $"\"{v}\"";
            if (v is Array)
                return $"[{string.Join(",", v as object[])}]";
            return v.ToString();
        }
    }

    public interface IDesc
    {
        string getDesc();
    }
}
