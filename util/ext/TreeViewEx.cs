using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class TreeViewEx
    {
        public static void drawOnce(this TreeView ui, Action func)
        {
            try
            {
                ui.BeginUpdate();
                func();
            }
            finally
            {
                ui.EndUpdate();
            }
        }

        public static void doOnSub(this TreeNode tn, string name, Action<TreeNode> func)
        {
            if (tn.get(name, out var sn))
                func(sn);
        }

        public static void remove(this TreeNode pn, string name)
        {
            if (pn.get(name, out var sn))
                sn.Remove();
        }

        public static bool get(this TreeNodeCollection ns, string text, out TreeNode sub)
        {
            sub = null;
            foreach (TreeNode sn in ns)
            {
                if (sn.Text == text)
                {
                    sub = sn;
                    return true;
                }
            }
            return false;
        }

        public static bool get(this TreeNode pn, string label, out TreeNode sub)
        {
            return pn.Nodes.get(label, out sub);
        }

        public static bool get(this TreeView tv, string label, out TreeNode sub)
        {
            return tv.Nodes.get(label, out sub);
        }

        public static void eachNode(this TreeView tree, Action<TreeNode> func)
        {
            tree.Nodes.eachNode(func);
        }

        public static void eachNode(this TreeNode tn, Action<TreeNode> func)
        {
            func(tn);
            tn.Nodes.eachNode(func);
        }

        public static void eachNode(this TreeNodeCollection tns, Action<TreeNode> func)
        {
            foreach (TreeNode tn in tns)
            {
                func(tn);
                if (tn.Nodes.Count > 0)
                    eachNode(tn.Nodes, func);
            }
        }

        public static bool existNode(this TreeNodeCollection tns, Func<TreeNode, bool> func)
        {
            foreach (TreeNode tn in tns)
            {
                if (func(tn))
                    return true;
                if (tn.Nodes.Count > 0 && existNode(tn.Nodes, func))
                    return true;
            }
            return false;
        }

        public static void toggle(this TreeNode tn)
        {
            if (tn.IsExpanded)
                tn.Collapse(true);
            else
                tn.Expand();
        }

        public static bool getNode(this TreeView tree, int x, int y, out TreeNode node)
        {
            node = getNode(tree, x, y);
            return null != node;
        }

        public static TreeNode getNode(this TreeView tree, int x, int y)
        {
            var pt = tree.PointToClient(new Point { X = x, Y = y });
            return tree.GetNodeAt(pt);
        }

        public static bool moveDown(this TreeView tree)
            => moveDown(tree, out var tn);

        public static bool moveUp(this TreeView tree)
            => moveUp(tree, out var tn);

        public static void moveNode(this TreeView tree, bool up, Action<TreeNode> func)
        {
            if (up)
                tree.moveUp(func);
            else
                tree.moveDown(func);
        }

        public static void moveDown(this TreeView tree, Action<TreeNode> func)
        {
            if (tree.moveDown(out var tn))
                func(tn);
        }

        public static bool moveDown(this TreeView tree, out TreeNode node)
        {
            node = tree.SelectedNode;
            if (null == node)
                return false;

            var pns = node.Parent?.Nodes ?? tree.Nodes;
            var pos = node.Index;
            if (pos == pns.Count - 1)
                return false;

            node.Remove();
            pns.Insert(pos + 1, node);
            tree.SelectedNode = node;

            return true;
        }

        public static void moveUp(this TreeView tree, Action<TreeNode> func)
        {
            if (tree.moveUp(out var tn))
                func(tn);
        }

        public static bool moveUp(this TreeView tree, out TreeNode node)
        {
            node = tree.SelectedNode;
            if (null == node)
                return false;

            var pns = node.Parent?.Nodes ?? tree.Nodes;
            var pos = node.Index;
            if (pos == 0)
                return false;

            node.Remove();
            pns.Insert(pos - 1, node);
            tree.SelectedNode = node;

            return true;
        }

        public static bool treeEmpty(this TreeNode tn)
            => null == tn || tn.Nodes.Count == 0;
        public static bool isRoot(this TreeNode tn)
            => tn != null && tn.Parent == null;
        public static bool isFirst(this TreeNode tn)
            => tn?.Index == 0;
        public static bool isLast(this TreeNode tn)
        {
            if (null == tn)
                return false;
            var pns = tn.Parent?.Nodes ?? tn.TreeView.Nodes;
            return tn.Index == pns.Count - 1;
        }

        public static bool getDragNode(this DragEventArgs e, out TreeNode tn)
        {
            tn = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            return null != tn;
        }

        public static TreeNode root(this TreeNode tn)
        {
            while (tn.Parent != null)
            {
                tn = tn.Parent;
            }
            return tn;
        }

        public static TreeNode icon(this TreeNode tn, string img)
        {
            tn.ImageKey = img;
            tn.SelectedImageKey = img;
            return tn;
        }

        public static TreeNode label(this TreeNode tn, string txt)
        {
            tn.Text = txt;
            return tn;
        }

        public static void clear(this TreeNode tn)
            => tn.Nodes.Clear();

        public static void add(this TreeNode pn, TreeNode tn)
            => pn.Nodes.Add(tn);
    }
}
