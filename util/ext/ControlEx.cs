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

        public static void asyncSetText(this Control ui, string text)
        {
            ui.asyncCall(() => ui.Text = text);
        }

        public static void asyncCall(this Control ui, Action func)
        {
            if (ui.InvokeRequired)
                ui.BeginInvoke(func);
            else
                func();
        }

        public static void syncCall(this Control ui, Action func)
        {
            if (ui.InvokeRequired)
                ui.Invoke(func);
            else
                func();
        }
    }
}
