using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class DragDropEx
    {
        public static T getData<T>(this DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(T));
            if (null == data)
                return default(T);
            return (T)data;
        }

        public static bool getData<T>(this DragEventArgs e, out T obj)
        {
            var data = e.Data.GetData(typeof(T));
            if (null == data)
            {
                obj = default(T);
                return false;
            }
            obj = (T)data;
            return true;
        }
    }
}
