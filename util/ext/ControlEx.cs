using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class ControlEx
    {
        public static void layoutOnce(this Control ui, Action func)
        {
            try
            {
                ui.SuspendLayout();
                func();
            }
            finally
            {
                ui.ResumeLayout();
            }
        }

        public static void setTextAsync(this Control ui, string text)
        {
            ui.runAsync(() => ui.Text = text);
        }

        public static R safeCall<R>(this Control ui, Func<R> func)
        {
            if (ui.InvokeRequired)
                return (R)ui.Invoke(func);
            else
                return func();
        }

        public static void runAsync(this Control ui, Action func)
        {
            if (ui.InvokeRequired)
                ui.BeginInvoke(func);
            else
                func();
        }

        public static void safeRun(this Control ui, Action func)
        {
            if (ui.InvokeRequired)
                ui.Invoke(func);
            else
                func();
        }
    }
}
