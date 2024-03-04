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
    public partial class TransferDialog : Form
    {
        public IDir src;
        public string[] srcFiles;
        public IDir dst;
        public string dstDir;

        BackgroundWorker thd;
        private void TransferDialog_Load(object sender, EventArgs e)
        {
            thd.run(ref thd, transfer, end);
        }

        public TransferDialog()
        {
            InitializeComponent();

            this.FormClosing += TransferDialog_FormClosing;
        }

        private void TransferDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thd.isActive())
                e.Cancel = true;
        }

        byte[] buff = new byte[4.mb()];
        void transfer()
        {
            var tc = new Counter
            {
                TotalCount = srcFiles.Length,
                TotalSize = srcFiles.Sum(p => src.getItem(p).size),
            };
            foreach (var srcPath in srcFiles)
            {
                tc.addCount();
                var dstPath = dstDir.pathMerge(srcPath.pathName());
                actionUI.setTextAsync($"<copy>{dstPath}");
                using (var fin = src.readFile(srcPath))
                using (var fout = dst.createFile(dstPath))
                {
                    var fc = new Counter { TotalSize = fin.Length };
                    fc.TimeIsUp += (t) 
                        => statusUI.setTextAsync($"{fc.SpeedInfo} - {tc.FullInfo}");
                    fc.trigger();
                    fin.copyTo(fout, delta =>
                    {
                        tc.addSize(delta);
                        fc.addSize(delta);
                        if (thd.CancellationPending)
                            throw new ManualCancel();
                    }, buff);
                    fc.trigger();
                }
            }
            statusUI.setTextAsync(tc.FullInfo);
        }

        void end(Exception err)
        {
            if (err is ManualCancel)
            {
                this.Close();
            }
            else if (err != null)
                err.Message.dlgError();

            actionUI.Text = this.trans("Finish");
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if (!thd.isActive())
            {
                this.Close();
                return;
            }
            if (!this.trans("ConfirmStop").confirm())
                return;

            thd.CancelAsync();
        }

        class ManualCancel : Exception { }
    }
}
