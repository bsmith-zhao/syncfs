using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public class Member
    {
        FieldInfo fld;
        PropertyInfo prop;

        public Member(FieldInfo fld)
            => this.fld = fld;

        public Member(PropertyInfo prop)
            => this.prop = prop;

        public Type type
            => fld?.GetType() ?? prop?.GetType();

        public string name
            => fld?.Name ?? prop?.Name;

        public T attr<T>() where T : Attribute
            => fld?.GetCustomAttribute<T>()
            ?? prop?.GetCustomAttribute<T>();

        public bool mark<T>() where T : Attribute
            => attr<T>() != null;

        public object get(object obj)
            => fld != null ? fld.GetValue(obj)
            : prop.GetValue(obj);

        public Member set(object obj, object v)
        {
            if (fld != null)
                fld.SetValue(obj, v);
            else
                prop.SetValue(obj, v);
            return this;
        }

        public bool canSet => prop?.CanRead ?? true;
        public bool canGet => prop?.CanWrite ?? true;
        public bool canAssign(Member other)
            => type.IsAssignableFrom(other.type);
    }
}
