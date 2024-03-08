using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace util.prop.edit
{
    public class EncodeDropList : UITypeEditor
    {
        static string[] encs = new string[] 
        {
            "utf-8",
            "gbk"
        };

        public override UITypeEditorEditStyle
            GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            var srv = provider.GetService(
                typeof(IWindowsFormsEditorService))
                as IWindowsFormsEditorService;
            if (srv != null)
            {
                var ui = new ListBox().theme();
                ui.Items.AddRange(encs);
                ui.MouseClick += (s, e) =>
                {
                    if (ui.SelectedIndex >= 0)
                        value = ui.SelectedItem;
                    srv.CloseDropDown();
                };
                srv.DropDownControl(ui);
            }
            return value;
        }
    }
}
