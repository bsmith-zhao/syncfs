using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace util.prop
{
    public class PwdView : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var srv = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (srv != null)
            {
                var ui = new TextBox
                {
                    Text = $"{value}",
                    ReadOnly = true,
                };
                srv.DropDownControl(ui);
            }
            return value;
        }
    }
}
