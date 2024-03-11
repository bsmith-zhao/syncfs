using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class ClassEx
    {
        public static string fullName(this MemberInfo m)
            => m == null ? null
            : $"{m.DeclaringType?.FullName}.{m.Name}";

        public static string shortName(this MemberInfo m)
            => m == null ? null
            : $"{m.DeclaringType?.Name}.{m.Name}";

        public static object @new(this Type type)
            => Activator.CreateInstance(type);

        public static string thisFunc(this bool src)
            => src.callTrace(-1);

        public static string lastFunc(this bool src)
            => src.callTrace(-2);

        public static string callTrace(this bool src, int level)
        {
            try
            {
                if (level < 0)
                    level = -level;
#if DEBUG
                level ++;
#endif
                var func = new StackFrame(level).GetMethod();
                return $"{func.ReflectedType.Name}.{func.Name}";
            }
            catch { }
            return null;
        }
    }
}
