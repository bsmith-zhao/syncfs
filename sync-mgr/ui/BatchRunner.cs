using sync.app;
using sync.mgr.Properties;
using sync.ui;
using sync.work;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using util;
using util.ext;

namespace sync.ui
{
    public partial class BatchRunner : Form
    {
        public IEnumerable<SpaceEntry> spaces;
        public string dir;

        Color runingColor => Color.Yellow;
        Color cancelColor => Color.Gray;
        Color errorColor => Color.Red;
        Color successColor => Color.LightGreen;

        ImageList listIcons;

        private void BatchRunner_Load(object sender, EventArgs e)
        {
            actPanel.BackColor
                = actionUI.BackColor
                = statusUI.BackColor
                = Theme.ControlBack.zoom(1.2);
            actPanel.Height = actionUI.Height;

            this.Text = $"{this.Text} {Application.ProductVersion}";

            true.trydo(loadWorks);
        }

        public BatchRunner()
        {
            InitializeComponent();

            Msg.output = msgUI.asyncAppend;

            listIcons = this.newImages(32)
                .add(nameof(Resources.MasterSync), Resources.MasterSync)
                .add(nameof(Resources.RoundSync), Resources.RoundSync);

            msgUI.scrollByWheel();
            toolbar.fixBorderBug();

            this.FormClosing += BatchRunner_FormClosing;

            openDirBtn.Enabled = false;
            listUI.SelectedIndexChanged += (s, e) => 
            {
                openDirBtn.Enabled = canOpenDir;
            };
            listUI.SmallImageList = listIcons;
        }

        private void BatchRunner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thd.isActive())
            {
                e.Cancel = true;
                typeof(WorkRunner).trans("StopBeforeClose").dlgAlert();
            }
        }

        void loadWorks()
        {
            (spaces ?? enumAppSpaces()).each(sp =>
            {
                this.trans("LoadSpace", sp.name, sp.dir).msg();
                true.trydo(() =>
                {
                    var ctx = new SpaceContext(sp);
                    sp.loadConf().syncs
                    .each(sync => addWorkItem(new SyncItem
                    {
                        space = sp,
                        sync = sync,
                        context = ctx,
                    }));
                });
            });
        }

        IEnumerable<SpaceEntry> enumAppSpaces()
        {
            var wsl = App.enumSpaces();
            if (dir != null)
            {
                dir = dir.pathUnify();
                yield return wsl.first(s => s.dir.low() == dir.low())
                    ?? new SpaceEntry { dir = dir };
            }
            else if (wsl != null)
                foreach (var ws in wsl)
                    yield return ws;
        }

        BackgroundWorker thd;
        ListViewItem listItem;
        WorkItem workItem;

        ListViewGroup getGroup(WorkItem wk)
        {
            var name = wk.group;
            var grp = listUI.Groups[name];
            if (null == grp)
            {
                grp = new ListViewGroup(name, name);
                listUI.Groups.Add(grp);
            }
            return grp;
        }

        void addWorkItem(WorkItem wk)
        {
            listUI.Items.Add(new ListViewItem(new[] { wk.name, ""})
            {
                ImageKey = $"{wk.icon}",
                Tag = wk,
                Checked = wk.check,
                Group = getGroup(wk),
            });
        }

        // run in the form ui thread
        void runByIndex(int index)
        {
            if (null != listItem)
                return;

            listItem = getFreshItem(index);
            if (null == listItem)
                return;

            workItem = listItem.Tag as WorkItem;

            updateStatus(runingColor, "Begin");

            thd.run(ref thd, null, threadRun, threadEnd);
        }

        ListViewItem getFreshItem(int begin)
        {
            for (int i = begin; i < listUI.Items.Count; i++)
            {
                var vi = listUI.Items[i];
                if (isFresh(vi))
                {
                    return vi;
                }
            }
            return null;
        }

        public bool isFresh(ListViewItem vi)
        {
            return vi.Checked && vi.ForeColor != Color.Green;
        }

        public void updateStatus(Color color, string name, string msg = null)
        {
            var status = this.trans(name);
            $"[{workItem.name}]<{status}>{msg}".msg();

            var vi = listItem;
            vi.ForeColor = color;
            vi.ToolTipText = msg;
            vi.SubItems[1].Text = status;
        }

        // run in the background thread
        void threadRun()
        {
            var wk = workItem;
            wk.MsgOutput = msgUI.asyncAppend;
            wk.CheckCancel = () =>
            {
                if (thd.CancellationPending)
                    throw new UserCancel();
            };
            wk.UpdateStatus = (act, st) 
                => this.asyncCall(() =>
                {
                    actionUI.Text = act;
                    statusUI.Text = st;
                });

            wk.work();
        }

        bool errorStop = true;
        // run in the form ui thread
        void threadEnd(Exception err)
        {
            int lastIndex = listItem.Index;
            try
            {
                "".msg();
                if (null != err)
                {
                    if (err is UserCancel)
                    {
                        updateStatus(cancelColor, "Cancel");
                        return;
                    }
                    else
                    {
                        updateStatus(errorColor, "Error", err.Message);
                        if (errorStop)
                            return;
                    }
                }
                else
                {
                    updateStatus(successColor, "Finish");
                }
                "".msg();
            }
            finally
            {
                listItem = null;
            }

            runByIndex(lastIndex + 1);
        }

        bool canRun => listUI.CheckedIndices.Count > 0
                        && !thd.isActive();

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!canRun)
                return;

            if (!this.trans("ConfirmStart").confirm())
                return;

            msgUI.Clear();
            runByIndex(0);
        }

        bool canStop => thd.isActive();

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (!canStop)
                return;

            if (!this.trans("ConfirmStop").confirm())
                return;

            thd.CancelAsync();
        }

        bool canOpenDir => listUI.SelectedItems.Count > 0;

        private void openDirBtn_Click(object sender, EventArgs e)
        {
            if (!canOpenDir)
                return;

            var wk = listUI.SelectedItems[0].Tag as WorkItem;
            wk.dir.dirOpen();
        }

        private void listUI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // reset the checkbox auto inverse
            stopCheck = false;

            if (listUI.SelectedItems.Count == 0)
                return;

            var li = listUI.SelectedItems[0];
            var wi = li.Tag as WorkItem;
            if (null != li.ToolTipText)
                $"[{wi.name}][{li.SubItems[1].Text}]{li.ToolTipText}".msg();
        }

        bool stopCheck = false;
        private void listUI_MouseDown(object sender, MouseEventArgs e)
        {
            // stop checkbox auto inverse
            if (e.Clicks == 2)
                stopCheck = true;
        }

        private void listUI_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // set checkbox to previous state
            if (stopCheck)
                e.NewValue = e.CurrentValue;
        }
    }
}
