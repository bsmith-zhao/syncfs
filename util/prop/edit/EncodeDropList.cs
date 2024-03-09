using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static util.prop.adjust.UnifyEncode;

namespace util.prop.edit
{
    public class EncodeDropList : UITypeEditor
    {
        public static string[] Encodes = new string[] 
        {
            UTF8, GBK
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
                ui.Items.AddRange(Encodes);
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
