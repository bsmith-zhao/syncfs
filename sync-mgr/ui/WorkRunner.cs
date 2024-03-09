using System;
using System.ComponentModel;
using System.Windows.Forms;
using util;
using util.ext;
using sync.app;
using sync.work;
using System.Drawing;

namespace sync
{
    public partial class WorkRunner : Form
    {
        public WorkItem item;

        public WorkRunner()
        {
            InitializeComponent();

            msgUI.scrollByWheel();

            Msg.output = msgUI.asyncAppend;
        }

        private void WorkRunner_Load(object sender, System.EventArgs e)
        {
            actPanel.BackColor
                = actionUI.BackColor
                = statusUI.BackColor
                = Theme.ControlBack.zoom(1.2);
            actPanel.Height = actionUI.Height;

            resSplit.BackColor = actPanel.BackColor;
            transPanel.BackColor = msgUI.BackColor;

            this.Text = $"{this.Text} [{item.name}]";

            true.trydo(()=> 
            {
                parseBtn.Enabled = item.canParse;
            });

            transPanel.ControlAdded += TransPanel_ControlAdded;
        }

        private void TransPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Dock = DockStyle.Fill;
        }

        class ManualCancel : Exception { }
        BackgroundWorker thd;

        void run(Action func)
        {
            if (thd.isActive() || !this.trans("ConfirmRun").confirm())
                return;

            transPanel.Controls.Clear();
            msgUI.Clear();
            $"<{this.trans("Begin")}>".msg();

            parseBtn.Enabled = false;
            workBtn.Enabled = false;
            stopBtn.Enabled = true;

            item.PanelOutput = transPanel;
            item.MsgOutput = msgUI.asyncAppend;
            item.CheckCancel = () =>
            {
                if (thd.CancellationPending)
                    throw new ManualCancel();
            };
            item.UpdateStatus = (action, status)
                => this.asyncCall(() =>
                {
                    actionUI.Text = action;
                    statusUI.Text = status;
                });

            thd.run(ref thd, func, 
            err => 
            {
                $"".msg();
                if (err is ManualCancel)
                    $"<{this.trans("Cancel")}>".msg();
                else if (err != null)
                    $"<{this.trans("Error")}>{err.Message}".msg();
                else
                    $"<{this.trans("Finish")}>".msg();

                parseBtn.Enabled = item.canParse;
                workBtn.Enabled = true;
                stopBtn.Enabled = false;
            });
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            run(item.parse);
        }

        private void workBtn_Click(object sender, EventArgs e)
        {
            run(item.work);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (!thd.isActive() || !this.trans("ConfirmStop").confirm())
                return;

            thd.CancelAsync();
        }

        private void WorkRunner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thd.isActive())
            {
                e.Cancel = true;
                this.trans("StopBeforeClose").dlgAlert();
            }
        }
    }
}
