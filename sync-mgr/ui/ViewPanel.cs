using sync.app;
using sync.mgr.Properties;
using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;
using util.rep;

namespace sync.ui
{
    public partial class ViewPanel : UserControl
    {
        IDir vw;
        public IDir view
        {
            get => vw;
            set
            {
                vw = value;
                updateUI();
            }
        }

        bool isRep => vw is Reposit;
        void updateUI()
        {
            createDirBtn.Visible
                = importBtn.Visible
                = exportBtn.Visible
                = deleteBtn.Visible
                    = editLabelBtn.Visible
                    = treeUI.AllowDrop
                    = treeUI.LabelEdit
                    = listUI.LabelEdit
                    = isRep;
            manageBtn.Visible = !isRep;
        }

        bool readOnly => !isRep;

        public bool MsgVisable
        {
            set
            {
                msgUI.Visible = value;
                msgSplit.Visible = value;
            }
        }

        TreeNode root => treeUI.Nodes[0];
        TreeNode selNode => treeUI.SelectedNode;
        string selDir => getPath(selNode);
        string getPath(TreeNode tn)
            => tn?.FullPath.Substring(root.Text.Length).TrimStart('/');
        bool treeActive => treeUI.Focused;
        bool listActive => listUI.Focused;
        ListViewItem selItem => listUI.selItem();
        bool isDir(ListViewItem it) => it?.SubItems.Count == 1;
        bool isFile(ListViewItem it) => it?.SubItems.Count > 1;
        string getPath(ListViewItem it) => selDir.pathMerge(it.Text);
        ListView.SelectedListViewItemCollection selItems => listUI.SelectedItems;

        private void SyncRepView_Load(object sender, EventArgs e)
        {
            toolbar.adjustBtns();

            load();
        }

        public void load()
        {
            if (view == null || treeUI.Nodes.Count > 0)
                return;

            var rn = new TreeNode
            {
                Text = $"{view}",
            }.icon(DirIcon);
            treeUI.Nodes.Add(rn);

            reloadDirs(root);
            root.Expand();
        }

        ImageList icons;
        const string DirIcon = "dir";
        const string FileIcon = "file";

        public ViewPanel()
        {
            InitializeComponent();

            Msg.output = msgUI.asyncAppend;

            icons = this.newImages(32)
                .add(DirIcon, Resources.Dir)
                .add(FileIcon, Resources.File);

            toolbar.fixBorderBug();

            treeUI.ImageList = icons;
            treeUI.PathSeparator = "/";
            treeUI.HideSelection = false;
            treeUI.ShowNodeToolTips = true;
            treeUI.LabelEdit = true;
            treeUI.AllowDrop = true;
            treeUI.AfterExpand += TreeUI_AfterExpand;
            treeUI.AfterSelect += TreeUI_AfterSelect;
            treeUI.AfterLabelEdit += TreeUI_AfterLabelEdit;
            treeUI.BeforeLabelEdit += TreeUI_BeforeLabelEdit;
            treeUI.DragEnter += treeUI_DragEnter;
            treeUI.DragOver += treeUI_DragOver;
            treeUI.DragLeave += treeUI_DragLeave;
            treeUI.DragDrop += treeUI_DragDrop;
            treeUI.KeyDown += TreeUI_KeyDown;

            listUI.SmallImageList = icons;
            listUI.ShowItemToolTips = true;
            listUI.View = System.Windows.Forms.View.Details;
            listUI.FullRowSelect = true;
            listUI.HideSelection = false;
            listUI.LabelEdit = true;
            listUI.AfterLabelEdit += ListUI_AfterLabelEdit;
            listUI.MouseDoubleClick += ListUI_MouseDoubleClick;
            listUI.ItemDrag += ListUI_ItemDrag;
            listUI.KeyDown += ListUI_KeyDown;

            treeUI.AfterSelect += (s, e) => updateBtns();
            treeUI.LostFocus += (s, e) => updateBtns();
            treeUI.GotFocus += (s, e) => updateBtns();
            listUI.LostFocus += (s, e) => updateBtns();
            listUI.GotFocus += (s, e) => updateBtns();
            listUI.SelectedIndexChanged += (s, e) => updateBtns();
        }

        private void TreeUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                delete();
        }

        private void ListUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                upperDir();
            else if (e.KeyCode == Keys.Delete)
                delete();
        }

        bool canDropOnNode(DragEventArgs e)
        {
            return e.getData<ListView.SelectedListViewItemCollection>(out var items)
                && treeUI.getNode(e.X, e.Y, out var dn)
                && dn != selNode
                && canMoveIntoDir(dn, items);
        }

        bool canMoveIntoDir(TreeNode dn, ListView.SelectedListViewItemCollection items)
        {
            while (null != dn)
            {
                if (dn.Parent == selNode)
                    return !items.exist<ListViewItem>(it => dn.Text == it.Text);
                dn = dn.Parent;
            }
            return true;
        }

        TreeNode dragOverNode;
        void setTreeDropStyle(DragEventArgs e)
        {
            e.Effect = canDropOnNode(e) ? DragDropEffects.Move : DragDropEffects.None;

            var tn = treeUI.getNode(e.X, e.Y);
            if (tn != dragOverNode)
            {
                if (dragOverNode != null)
                    dragOverNode.BackColor = treeUI.BackColor;

                if (tn != null)
                    tn.BackColor = Color.DarkGreen;

                dragOverNode = tn;
            }
        }

        void clearTreeDropStyle()
        {
            if (dragOverNode != null)
                dragOverNode.BackColor = treeUI.BackColor;
            dragOverNode = null;
        }

        private void treeUI_DragEnter(object sender, DragEventArgs e)
        {
            setTreeDropStyle(e);
        }

        private void treeUI_DragOver(object sender, DragEventArgs e)
        {
            setTreeDropStyle(e);
        }

        private void treeUI_DragLeave(object sender, EventArgs e)
        {
            clearTreeDropStyle();
        }

        private void treeUI_DragDrop(object sender, DragEventArgs e)
        {
            true.trydo(() => 
            {
                clearTreeDropStyle();

                if (!canDropOnNode(e))
                    return;

                e.getData<ListView.SelectedListViewItemCollection>(out var items);
                treeUI.getNode(e.X, e.Y, out var dn);

                var srcDir = selDir;
                var dstDir = getPath(dn);
                foreach (var vi in items.toList<ListViewItem>())
                {
                    var name = vi.Text;
                    var srcPath = srcDir.pathMerge(name);
                    var dstPath = dstDir.pathMerge(name);
                    if (isFile(vi))
                        view.moveFile(srcPath, dstPath);
                    else
                    {
                        view.moveDir(srcPath, dstPath);
                        if (selNode.get(name, out var sn))
                            sn.Remove();
                        var tn = addDirNode(dn, dstPath);
                        if (dn.IsExpanded)
                            reloadDirs(tn);
                    }
                    vi.Remove();
                }
            });
        }

        private void ListUI_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.DoDragDrop(listUI.SelectedItems, DragDropEffects.Move);
        }

        private void ListUI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            enterDir();
        }

        bool canEnter => isDir(selItem);
        void enterDir()
        {
            if (!canEnter)
                return;

            if (selNode.get(selItem.Text, out var sn))
                treeUI.SelectedNode = sn;
        }

        private void ListUI_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;

            e.CancelEdit = true;
            var it = listUI.Items[e.Item];
            if (!unifyName(e.Label, out var name)
                || it.Text == name)
                return;

            true.trydo(() => 
            {
                var old = selDir.pathMerge(it.Text);
                var path = selDir.pathMerge(name);
                if (isFile(it))
                {
                    view.moveFile(old, path);
                }
                else
                {
                    view.moveDir(old, path);
                    if (selNode.get(it.Text, out var sub))
                        sub.Text = name;
                }
                it.Text = name;
            });
        }

        private void TreeUI_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.isRoot())
                e.CancelEdit = true;
        }

        private void TreeUI_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
                return;

            e.CancelEdit = true;
            var tn = e.Node;
            if (!unifyName(e.Label, out var name)
                || tn.Text == name)
                return;

            true.trydo(() => 
            {
                var old = getPath(tn);
                var path = getPath(tn.Parent).pathMerge(name);
                view.moveDir(old, path);
                tn.Text = name;
            });
        }

        static HashSet<char> ics => Path.GetInvalidFileNameChars().ToHashSet();
        bool unifyName(string name, out string unify)
        {
            var cs = name.ToCharArray();
            int n = cs.Length;
            while (n-- > 0)
            {
                if (ics.Contains(cs[n]))
                    cs[n] = '-';
            }
            unify = new string(cs).Trim();
            return unify.Length > 0;
        }

        void updateBtns()
        {
            upLevelBtn.Enabled = canUpLevel;
            refreshBtn.Enabled = canRefresh;
            createDirBtn.Enabled = canCreateDir;
            deleteBtn.Enabled = canDelete;
            exportBtn.Enabled = canExport;
            editLabelBtn.Enabled = canEditLabel;
        }

        private void TreeUI_AfterSelect(object sender, TreeViewEventArgs e)
        {
            true.trydo(() => reloadItems(e.Node));
        }

        private void TreeUI_AfterExpand(object sender, TreeViewEventArgs e)
        {
            true.trydo(() => expandDirs(e.Node));
        }

        void reloadDirs(TreeNode pn)
        {
            treeUI.drawOnce(() => 
            {
                pn.clear();
                var path = getPath(pn);
                view.enumDirs(path).each(d => addDirNode(pn, d));
            });
        }

        TreeNode addDirNode(TreeNode pn, string dir)
        {
            var tn = new TreeNode(dir.pathName())
                    .icon(DirIcon);
            pn.add(tn);
            return tn;
        }

        void expandDirs(TreeNode pn)
        {
            foreach (TreeNode tn in pn.Nodes)
            {
                if (tn.Tag != null)
                    continue;
                reloadDirs(tn);
                tn.Tag = 1;
            }
        }

        void reloadItems(TreeNode pn)
        {
            listUI.drawOnce(() => 
            {
                listUI.Items.Clear();
                var path = getPath(pn);
                view.enumDirs(path).each(addDirItem);
                view.enumFiles(path).each(addFileItem);
            });
        }

        public static string time(FileItem f)
            => DateTime.FromFileTime(f.createTime.atLeast(f.modifyTime)).text();

        void addFileItem(FileItem file)
        {
            var flds = new string[]
                {
                    file.path.pathName(),
                    $"{file.size.byteSize()}",
                    $"{file.size.ToString("#,##0")}",
                    $"{time(file)}"
                };
            var it = new ListViewItem(flds, FileIcon)
            {
                ToolTipText = flds.join("\r\n"),
            };
            listUI.Items.Add(it);
        }

        void addDirItem(string dir)
        {
            var it = new ListViewItem(dir.pathName(), DirIcon);
            listUI.Items.Add(it);
        }

        bool canRefresh => selNode != null;
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            refresh();
        }

        void refresh()
        {
            if (!canRefresh)
                return;

            true.trydo(() =>
            {
                reloadDirs(selNode);
                if (selNode.IsExpanded)
                    expandDirs(selNode);
                reloadItems(selNode);
            });
        }

        bool canUpLevel => selNode?.Parent != null;

        private void upLevelBtn_Click(object sender, EventArgs e)
        {
            upperDir();
        }

        void upperDir()
        {
            if (!canUpLevel)
                return;

            treeUI.SelectedNode = selNode.Parent;
        }

        bool canCreateDir => selNode != null;
        private void createDirBtn_Click(object sender, EventArgs e)
        {
            true.trydo(createDir);
        }

        void createDir()
        {
            if (!canCreateDir)
                return;

            var path = selDir.pathMerge("newDir");
            path = path.pathSettle(view.exist);
            view.createDir(path);

            addDirNode(selNode, path);
            addDirItem(path);
        }

        bool canDelete => !readOnly 
            && ((treeActive && selNode != null && selNode != root)
            || (listActive && selItem != null));

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            delete();
        }

        void delete()
        {
            true.trydo(()=>
            {
                if (!canDelete)
                    return;

                if (treeActive)
                {
                    var dir = selDir;
                    if (!this.trans("ConfrimDeleteDir", dir).confirm()
                        || !this.trans("RepeatConfrimDeleteDir", dir).confirm())
                        return;
                    view.deleteDir(selDir);
                    selNode.Remove();
                }
                else if (listActive)
                {
                    var cnt = listUI.SelectedItems.Count;
                    if (!this.trans("ConfrimDeleteItems", cnt).confirm()
                        || !this.trans("RepeatConfrimDeleteItems", cnt).confirm())
                        return;
                    var items = listUI.SelectedItems.toList<ListViewItem>();
                    var dir = selDir;
                    foreach (ListViewItem it in items)
                    {
                        var path = dir.pathMerge(it.Text);
                        if (isFile(it))
                        {
                            view.deleteFile(path);
                        }
                        else
                        {
                            view.deleteDir(path);
                            selNode.remove(it.Text);
                        }
                        it.Remove();
                    }
                }
            });
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            if (!true.pickFiles(out var paths))
                return;

            new TransferDialog
            {
                src = new NormalDirReposit(paths[0].pathDir()),
                srcFiles = paths.conv(p => p.pathName()),
                dst = view,
                dstDir = selDir,
            }.dialog();

            refresh();
        }

        bool canExport => selItems.exist<ListViewItem>(it => isFile(it));
        private void exportBtn_Click(object sender, EventArgs e)
        {
            if (!canExport)
                return;
            if (!true.pickDir(out var dir))
                return;

            var srcFiles = selItems.conv<ListViewItem, string>(it => 
                            isFile(it)?getPath(it):null)
                            .ToArray().pathUnify();
            var dlg = new TransferDialog
            {
                src = view,
                srcFiles = srcFiles,
                dst = new NormalDirReposit(dir),
            };
            dlg.dialog();
        }

        bool canEditLabel => (treeActive && !selNode.isRoot())
                            || (listActive && selItem != null);
        private void editLabelBtn_Click(object sender, EventArgs e)
        {
            if (!canEditLabel)
                return;

            if (treeActive)
                selNode.BeginEdit();
            else if (listActive)
                selItem.BeginEdit();
        }
    }
}
