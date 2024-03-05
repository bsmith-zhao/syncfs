using System;
using System.Drawing;
using System.Windows.Forms;
using util.ext;
using util.prop;

namespace util
{
    public partial class SetupDialog : Form
    {
        public object Args
        {
            get => propUI.SelectedObject;
            set => propUI.SelectedObject = value;
        }

        private void SetupDialog_Load(object sender, EventArgs e)
        {
            splitUI.BackColor 
                = descUI.BackColor 
                = Theme.ControlBack.zoom(1.1);

            propUI.enhanceDesc(descUI);
        }

        public SetupDialog()
        {
            InitializeComponent();

            propUI.enhanceEdit();
        }
    }
}
