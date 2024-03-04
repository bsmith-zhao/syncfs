using System;
using System.Drawing;
using System.Windows.Forms;
using util.ext;

namespace util.prop
{
    public partial class PropDialog : Form
    {
        public object Args
        {
            get => propUI.SelectedObject;
            set => propUI.SelectedObject = value;
        }

        private void SetupDialog_Load(object sender, EventArgs e)
        {
            splitUI.BackColor = 
            descUI.BackColor = 25.gray();

            propUI.enhanceDesc(descUI);
        }

        public PropDialog()
        {
            InitializeComponent();

            propUI.enhanceEdit();
        }
    }
}
