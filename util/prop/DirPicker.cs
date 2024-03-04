using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace util
{
    public class DirPicker : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var srv = provider.GetService(typeof(IWindowsFormsEditorService));
            if (srv != null && this.pickDir(out var dir))
                return dir;
            return value;
        }
    }
}
