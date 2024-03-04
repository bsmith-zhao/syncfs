using sync.mgr.Properties;
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
using static sync.sync.TransSummary;

namespace sync.sync
{
    public partial class TransSumPanel : UserControl
    {
        public DirNode[] roots;

        private void TransferDialog_Load(object sender, EventArgs e)
        {
            roots.each(dn =>
            {
                var tn = addNode(null, dn);
                expandSubs(tn);
                tn.Expand();
            });
        }

        ImageList icons;
        const string DirIcon = "dir";

        public TransSumPanel(params DirNode[] roots)
        {
            InitializeComponent();

            this.roots = roots;

            icons = this.newImages(32)
                .add(DirIcon, Resources.Dir);

            //treeUI.Scrollable = false;
            treeUI.ImageList = icons;
            treeUI.AfterExpand += TreeUI_AfterExpand;

            (this as UserControl).theme();
        }

        TreeNode addNode(TreeNode pn, DirNode dn)
        {
            var tn = new TreeNode
            {
                Text = $"{dn}",
                Tag = dn,
            }.icon(DirIcon);
            (pn?.Nodes ?? treeUI.Nodes).Add(tn);
            return tn;
        }

        void expandSubs(TreeNode pn)
            => (pn.Tag as DirNode).nodes?.Values
            .each(d => addNode(pn, d));

        private void TreeUI_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Nodes.eachNode(tn =>
            {
                if (tn.Nodes.Count == 0)
                    expandSubs(tn);
            });
        }
    }
}
