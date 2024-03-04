using sync.app;
using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;
using util.rep;

namespace sync.ui
{
    public partial class ViewBrowser : Form
    {
        public IDir rep;
        public IDir view
        {
            get => viewPanel.view;
            set => viewPanel.view = value;
        }

        private void SyncRepManager_Load(object sender, EventArgs e)
        {
        }

        public ViewBrowser()
        {
            InitializeComponent();

            mgrBtn.Click += ManageBtn_Click;

            this.Shown += (s, e) => viewPanel.listUI.autoSpan();

            viewPanel.MsgVisable = true;
        }

        ToolStripButton mgrBtn => viewPanel.manageBtn;

        private void ManageBtn_Click(object sender, EventArgs e)
        {
            repSplit.Visible 
                = repPanel.Visible 
                = mgrBtn.Checked;

            if (repPanel.Visible)
            {
                repPanel.view = rep;
                repPanel.load();
            }
        }
    }
}
