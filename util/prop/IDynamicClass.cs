using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.prop
{
    public interface IDynamicClass
    {
        void OnCreate();
        void OnActive();
        bool OnChange(object owner, string fld, object old);
    }
}
