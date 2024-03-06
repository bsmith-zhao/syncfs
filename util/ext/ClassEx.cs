﻿using System;
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

        public static string thisFunc(this object src)
            => src.callTrace(-2);

        public static string lastFunc(this object src)
            => src.callTrace(-3);

        public static string callTrace(this object src, int level)
        {
            try
            {
                if (level < 0)
                    level = -level;
                return new StackFrame(level).GetMethod().Name;
            }
            catch { }
            return null;
        }
    }
}
