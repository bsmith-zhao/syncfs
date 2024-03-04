using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using util;
using util.ext;

namespace test
{
    public partial class TestManager : Form
    {
        private void TestManager_Load(object sender, EventArgs e)
        {
            treeUI.ExpandAll();
        }

        public TestManager()
        {
            InitializeComponent();

            logUI.msgToMeAsync();
            Status.output = statUI.setTextAsync;

            toolbar.fixBorderBug();

            treeUI.NodeMouseDoubleClick += TreeUI_NodeMouseDoubleClick;
        }

        private void TreeUI_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            run(new TreeNode[] { e.Node });
        }

        public TestManager addTest(Type type)
        {
            var nodes = treeUI.Nodes;
            var items = type.FullName.Split('.');
            TreeNode tn = null;
            foreach (var item in items)
            {
                if (!nodes.get(item, out tn))
                {
                    tn = nodes.Add(item);
                }
                nodes = tn.Nodes;
            }
            tn.Tag = type;
            return this;
        }

        BackgroundWorker thd;

        List<TreeNode> getTests()
        {
            var tests = new List<TreeNode>();
            treeUI.each(tn =>
            {
                if (tn.Checked && tn.Tag is Type)
                {
                    tests.Add(tn);
                }
            });
            return tests;
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            if (thd.isActive())
                return;

            run(getTests());
        }

        void run(IEnumerable<TreeNode> tests)
        {
            if (thd.isActive())
            {
                $"test thread is running!!".msg();
                return;
            }

            thd.run(ref thd, () =>
            {
                $"[{DateTime.Now}]<begin>".msg();
                foreach (var tn in tests)
                {
                    if (!(tn.Tag is Type cls))
                        continue;

                    $"\r\n<test>{cls.FullName}".msg();

                    var func = cls.GetMethod("test");
                    if (null == func)
                        throw new Exception($"no test func in {cls.FullName}");

                    var obj = cls.@new();
                    func.Invoke(obj, null);
                }
            },
            err =>
            {
                if (null != err)
                {
                    $"\r\n[{DateTime.Now}]<fail>{err.Message}\r\n{err.StackTrace}".msg();
                    if (null != err.InnerException)
                        $"<inner>{err.InnerException}".msg();
                }
                else
                    $"\r\n[{DateTime.Now}]<pass>".msg();
            });
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            //var dlg = new CommonOpenFileDialog { IsFolderPicker = true};
            //dlg.ShowDialog();
            //dlg.FileName.log();

            var dlg = new PickDirDialog();
            if (dlg.ShowDialog() == true)
            {
                dlg.ResultPath.msg();
            }
        }
    }
}
