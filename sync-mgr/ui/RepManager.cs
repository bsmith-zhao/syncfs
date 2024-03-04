using sync.app;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util.rep;

namespace sync.ui
{
    public partial class RepManager : Form
    {
        public IDir rep
        {
            get => repPanel.view;
            set => repPanel.view = value;
        }

        private void RepManager_Load(object sender, EventArgs e)
        {

        }

        public RepManager()
        {
            InitializeComponent();

            repPanel.MsgVisable = true;
        }
    }
}
