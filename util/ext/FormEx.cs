using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class FormEx
    {
        public static void launch(this Form form)
        {
            form.trans();
            form.Font = SystemFonts.MessageBoxFont;
            form.theme();
            Application.Run(form);
        }

        public static bool dialog(this Form dlg, Control owner = null)
        {
            using (dlg)
            {
                dlg.trans();
                dlg.ShowInTaskbar = false;
                if (null != owner)
                    dlg.Font = owner.Font;
                else
                    dlg.Font = SystemFonts.MessageBoxFont;
                dlg.theme();
                dlg.StartPosition = FormStartPosition.CenterScreen;
                return dlg.ShowDialog() == DialogResult.OK;
            }
        }

        const BindingFlags All = BindingFlags.Public 
                                | BindingFlags.NonPublic 
                                | BindingFlags.Instance;

        public static void setValues(this Control form, object bean)
        {
            var formType = form.GetType();
            var beanType = bean.GetType();
            foreach (var fld in beanType.GetFields())
            {
                var formFld = formType.GetField(fld.Name + "Fld");
                if (null == formFld) continue;

                setFieldValue(formFld.GetValue(form), fld.GetValue(bean));
            }
            
            foreach (var fld in beanType.GetProperties(All))
            {
                var formFld = formType.GetField(fld.Name + "Fld");
                if (fld.CanRead == false || null == formFld) continue;

                setFieldValue(formFld.GetValue(form), fld.GetValue(bean));
            }
        }

        public static void setFieldValue(object ui, object value)
        {
            if (ui is TextBox)
            {
                (ui as TextBox).Text = value?.ToString() ?? "";
            }
            else if (ui is CheckBox)
            {
                try
                {
                    (ui as CheckBox).Checked = bool.Parse(value.ToString());
                }
                catch
                {
                    (ui as CheckBox).Checked = false;
                }
            }
        }
    }
}
