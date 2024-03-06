using System;
using System.Collections.Generic;
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
    }
}
