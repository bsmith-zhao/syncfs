using link;
using sync.app;
using sync.app.conf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;
using util.prop;
using util.rep;
using vfs;
using System.Security.Cryptography;
using sync.mgr.Properties;
using sync.work;

namespace sync.ui
{
    public partial class SyncManager : Form
    {
        string newId()
            => $"{selSpace.dir}/{DateTime.Now.ToString("mmss")}"
                .fsSettle().pathName();

        TreeNode selNode
        {
            get => treeUI.SelectedNode;
            set => treeUI.SelectedNode = value;
        }

        TreeNode spaceNode => selNode?.root();
        SpaceTag spaceTag => spaceNode?.Tag as SpaceTag;
        SpaceEntry selSpace => spaceTag?.space;

        SyncTag syncTag => selNode?.Tag as SyncTag;
        ArgsConf syncConf => syncTag?.conf;

        bool treeActive => treeUI.Focused;
        bool graphActive => linkUI.Focused;

        LinkViewGraph spaceGraph
        {
            get => spaceTag?.graph;
            set => spaceTag.graph = value;
        }
        List<Item> pickItems => spaceGraph?.Picks;
        Item pickItem => spaceGraph?.LastPick;

        RepTag repTag => pickItem?.Tag as RepTag;
        ArgsConf repConf => repTag?.conf;

        ArgsConf ec;
        ArgsConf editConf
        {
            set => value.update(ref ec, () =>
            {
                true.trydo(() =>
                {
                    ec?.OnActive?.Invoke();
                    propUI.SelectedObject = ec?.Args;
                });
            });
        }

        private void SyncManager_Load(object sender, EventArgs e)
        {
            linkUI.BackColor = Theme.ControlBack;

            descSplit.BackColor
                = descUI.BackColor
                = Theme.ControlBack.zoom(1.1);

            updateTitle();
            languageBtn.initLang(translate);
            toolbar.adjustBtns();

            true.trydo(loadSpaceList);
        }

        void updateTitle()
            => this.Text = $"{this.Text} {Application.ProductVersion}";

        void translate()
        {
            toolbar.eachItem(b => b.AutoSize = true);
            this.trans();
            toolbar.adjustBtns();
            descUI.Text = "";
            updateTitle();
        }

        EventAgent<TreeViewEventArgs> treeSelectEvent;
        EventAgent<TreeViewEventArgs> treeCheckEvent;

        ImageList icons96;
        ImageList icons80;
        ImageList treeIcons;

        public SyncManager()
        {
            InitializeComponent();

            Msg.output = msgUI.asyncAppend;
            util.Debug.output = msgUI.asyncAppend;

            icons96 = this.newImages(96)
                .add(nameof(RepType.AeadFS), Resources.AeadFS96)
                .add(nameof(RepType.NormalDir), Resources.NormalDir96);

            icons80 = this.newImages(80).add("View", Resources.View80);

            treeIcons = this.newImages(32)
                    .add(nameof(Resources.SpaceOpen), Resources.SpaceOpen)
                    .add(nameof(Resources.SpaceClose), Resources.SpaceClose)
                    .add(nameof(Resources.MasterSync), Resources.MasterSync)
                    .add(nameof(Resources.RoundSync), Resources.RoundSync);

            toolbar.fixBorderBug();

            treeSelectEvent = new EventAgent<TreeViewEventArgs>(treeNodeSelected);
            treeCheckEvent = new EventAgent<TreeViewEventArgs>(treeNodeChecked);

            treeUI.LabelEdit = true;
            treeUI.CheckBoxes = true;
            treeUI.ImageList = treeIcons;
            treeUI.HideSelection = false;
            treeUI.AfterSelect += treeSelectEvent.trigger;
            treeUI.GotFocus += TreeUI_GotFocus;
            treeUI.AfterCheck += treeCheckEvent.trigger;
            treeUI.AfterLabelEdit += TreeUI_AfterLabelEdit;
            treeUI.NodeMouseDoubleClick += TreeUI_NodeMouseDoubleClick;
            treeUI.KeyDown += TreeUI_KeyDown;

            linkUI.ItemsMoved += GramUI_ItemsMoved;
            linkUI.GotFocus += GramUI_GotFocus;
            linkUI.PicksChanged += GramUI_PicksChanged;
            linkUI.LabelModified += GramUI_LabelModified;
            linkUI.MouseDoubleClick += GramUI_MouseDoubleClick;
            linkUI.KeyDown += LinkUI_KeyDown;

            propUI.PropertySort = PropertySort.Categorized;
            propUI.enhanceEdit(PropUI_PropertyValueChanged);
            propUI.enhanceDesc(descUI);

            treeUI.AfterSelect += (s, e) => updateBtns();
            treeUI.GotFocus += (s, e) => updateBtns();
            treeUI.LostFocus += (s, e) => updateBtns();
            treeUI.AfterCheck += (s, e) => updateBtns();

            linkUI.GotFocus += (s, e) => updateBtns();
            linkUI.LostFocus += (s, e) => updateBtns();
            linkUI.PicksChanged += () => updateBtns();

            this.FormClosing += SyncManager_FormClosing;
        }

        IEnumerable<Process> allVfs()
        {
            foreach (TreeNode tn in treeUI.Nodes)
            {
                if (tn.Tag is SpaceTag st && st.graph != null)
                {
                    foreach (var it in st.graph.Items)
                        if (it.Tag is RepTag rt && rt.vfs != null)
                            yield return rt.vfs;
                }
            }
        }

        bool vfsActive => allVfs().exist(v => v != null);

        private void SyncManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vfsActive && !this.trans("ConfirmUnmountClose").confirm())
                e.Cancel = true;
            else
                allVfs().each(killVfs);
        }

        private void TreeUI_KeyDown(object sender, KeyEventArgs e)
        {
            keyCmd(e);
        }

        private void LinkUI_KeyDown(object sender, KeyEventArgs e)
        {
            keyCmd(e);
        }

        void keyCmd(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                copy();
            else if (e.Control && e.KeyCode == Keys.V)
                paste();
            else if (e.KeyCode == Keys.Delete)
                delete();
        }

        private void GramUI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (pickItem.canBeView())
                browse();
            else if (pickItem?.Tag is SyncTag)
                runSync();
        }

        private void TreeUI_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            runSync();
        }

        private void GramUI_LabelModified(link.Item it)
        {
            true.trydo(saveWorkTree);
        }

        private void TreeUI_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
                return;
            selNode.Text = e.Label;
            saveByNode(selNode);
        }

        private void treeNodeChecked(object s, TreeViewEventArgs e)
        {
            var tn = e.Node;
            if (tn.isSpace() && tn.Nodes.Count > 0)
            {
                treeCheckEvent.suspend(() =>
                    tn.Nodes.eachNode(n => n.Checked = tn.Checked));

                true.trydo(() => saveWorkTree(tn));
            }
            true.trydo(() => saveByNode(tn));
        }

        void updateBtns()
            => true.trydo(() => toolbar.layoutOnce(updateToolbar));

        void updateToolbar()
        {
            deleteBtn.Enabled = canDelete;
            addAeadFSBtn.Enabled = canAddRep;
            addNormalDirBtn.Enabled = canAddRep;
            createViewBtn.Enabled = canAddView;
            linkViewBtn.Enabled = canLinkView;
            masterSyncBtn.Enabled = canAddSync;
            roundSyncBtn.Enabled = canAddSync;
            openDirBtn.Enabled = canOpenDir;
            editLabelBtn.Enabled = canEditLabel;
            upBtn.Enabled = canMoveUp;
            downBtn.Enabled = canMoveDown;
            runBtn.Enabled = canRun;
            batchRunBtn.Enabled = canBatchRun;
            copyArgsBtn.Enabled = canCopyArgs;
            pasteArgsBtn.Enabled = canPasteArgs;
            spreadArgsBtn.Enabled = canSpreadArgs;
            mountBtn.Enabled = canMount;
            unmountBtn.Enabled = canUnmount;
            modifyPwdBtn.Enabled = canModifyPwd;
            browseFilesBtn.Enabled = canBrowse;
            manageFilesBtn.Enabled = canManage;
        }

        private void GramUI_GotFocus(object sender, EventArgs e)
        {
        }

        private void TreeUI_GotFocus(object sender, EventArgs e)
        {
        }

        private void GramUI_PicksChanged()
        {
            editConf = (pickItem?.Tag as ConfTag)?.conf;
            if (pickItem?.Tag is SyncTag st)
                treeSelectEvent.suspend(() => treeUI.SelectedNode = st.node);
        }

        private void PropUI_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            true.trydo(() =>
            {
                ec.save();

                var it = e.ChangedItem;
                if (ec.OnChange?.Invoke(it.owner(), it.Label) == true)
                    propUI.Refresh();
            });
        }

        private void GramUI_ItemsMoved(Point a, Point b)
        {
            true.trydo(saveWorkTree);
        }

        private void treeNodeSelected(object s, TreeViewEventArgs e)
        {
            if (spaceGraph == null)
                true.trydo(loadSpace);

            updateEditors();
        }

        void updateEditors()
        {
            linkUI.Graph = spaceGraph;
            spaceGraph?.pickItem(syncTag?.link);
            editConf = syncConf;
        }

        void addViewLink(INode src, INode dst)
        {
            var lk = new LineLink(src, dst)
            {
                Line = viewLine,
                Tag = new ViewLinkTag(),
            };
            spaceGraph.addItem(lk);
        }

        void loadSpace()
        {
            var ws = spaceTag.space.loadConf();
            spaceGraph = new LinkViewGraph();

            var nds = new Dictionary<string, INode>();
            ws.reps.each(e => nds[e.id] = addRepNode(e));
            ws.views.each(e => nds[e.id] = addViewNode(e));
            ws.views.exclude(e => e.src == null)
                .each(e => addViewLink(nds[e.src], nds[e.id]));
            treeCheckEvent.suspend(() =>
                ws.syncs.each(e =>
                    addSyncLink(e, nds[e.src], nds[e.dst])));

            spaceNode.icon("SpaceOpen").ExpandAll();
        }

        INode addRepNode(RepEntry en)
        {
            var tag = new RepTag
            {
                entry = en,
            };
            var conf = new ArgsConf
            {
                path = selSpace.entryPath(en.id),
                type = RepConf.cls(en.type),
                OnActive = tag.OnActive,
                OnChange = tag.OnChange,
            };
            tag.conf = conf;
            var nd = new RectNode(new Size(96, 96))
            {
                Pos = en.pos,
                Text = en.name,
                Font = linkUI.Font,
                Image = icons96.Images[$"{en.type}"],
                Tag = tag,
            };
            spaceGraph.addItem(nd);

            return nd;
        }

        void loadSpaceList()
        {
            treeCheckEvent.suspend(()
                => App.enumSpaces()
                .each(ws => addSpaceNode(ws)));
        }

        TreeNode addSpaceNode(SpaceEntry ws)
        {
            var tn = new TreeNode
            {
                Text = ws.name,
                ToolTipText = ws.dir,
                Tag = new SpaceTag { space = ws },
            }.icon("SpaceClose");
            treeUI.Nodes.Add(tn);

            return tn;
        }

        private void addDirRepBtn_Click(object sender, EventArgs e)
        {
            true.trydo(() => addRep(RepType.NormalDir));
        }

        bool canAddRep => spaceGraph != null;

        void addRep(RepType type)
        {
            if (!canAddRep)
                return;

            var conf = RepConf.create(type);
            var name = conf.createRep();
            if (name == null)
                return;

            var repId = $"rep{newId()}";
            selSpace.createEntry(repId, conf);

            var en = new RepEntry
            {
                id = repId,
                type = type,
                name = name,
                pos = new Point(100, 100),
            };
            addRepNode(en);

            saveWorkTree();
        }

        private void addAeadRepBtn_Click(object sender, EventArgs e)
        {
            true.trydo(() => addRep(RepType.AeadFS));
        }

        private void addSpaceBtn_Click(object sender, EventArgs e)
        {
            true.trydo(addSpace);
        }

        public void addSpace()
        {
            if (!true.pickDir(out var dir))
                return;

            if (treeUI.Nodes.exist<TreeNode>(tn
                => tn.spaceTag().space.dir.low() == dir.low()))
            {
                this.trans("SpaceExist", dir).dlgError();
                return;
            }

            var sp = new SpaceEntry
            {
                dir = dir,
                name = dir,
            };

            sp.createConf();
            addSpaceNode(sp);

            saveSpaceList();
        }

        IEnumerable<SpaceEntry> enumSpaces()
            => treeUI.Nodes.conv<TreeNode, SpaceEntry>(tn =>
            {
                var sp = tn.spaceTag().space;
                sp.name = tn.Text;
                return sp;
            });

        void saveSpaceList()
            => App.saveSpaces(enumSpaces());

        bool canDelete => (treeActive && null != selNode)
                        || (graphActive && null != pickItem);
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            delete();
        }

        IEnumerable<Process> spaceVfs
            => spaceTag?.graph?.Items
            .pick(e => e.Tag is RepTag rt && rt.vfs != null)
            .conv(e => (e.Tag as RepTag).vfs);

        void delete()
        {
            if (!canDelete)
                return;

            true.trydo(() =>
            {
                var space = this.selSpace;
                if (treeActive)
                {
                    if (selNode.isSpace())
                    {
                        if (!this.trans("ConfirmRemoveSpace", space.name).confirm())
                            return;

                        spaceVfs.each(killVfs);

                        selNode.Remove();
                        saveSpaceList();

                        if (selNode == null)
                            updateEditors();
                    }
                    else
                    {
                        if (!this.trans("ConfrimDeleteSync", selNode.Text).confirm())
                            return;

                        var lk = selNode.syncTag().link;
                        spaceGraph.delItem(lk, OnDeleteItem);

                        saveWorkTree();
                    }
                }
                else if (graphActive)
                {
                    if (!this.trans("ConfirmDeletePicks", pickCount).confirm())
                        return;

                    var graph = this.spaceGraph;
                    graph.drawOnce(() =>
                        new List<Item>(graph.Picks)
                        .each(it => graph.delItem(it, OnDeleteItem)));

                    saveWorkTree();
                }
                editConf = null;
            });
        }

        void OnDeleteItem(IItem it)
        {
            if (it.Tag is IResTag rt)
                true.trydo(() =>
                    $"{selSpace.dir}/{rt.id}".dirDelete(true));

            if (it.Tag is SyncTag st)
                st.node.Remove();

            if (it.Tag is RepTag rpt && rpt.vfs != null)
                killVfs(rpt.vfs);
        }

        void killVfs(Process p)
        {
            true.trydo(() => 
            {
                p.Kill();
                p.WaitForExit(200);
            });
        }

        bool canOpenDir => (treeActive && (selNode.isSpace() || selNode?.Tag is IResTag))
                        || (graphActive && pickItem?.Tag is IResTag);

        private void openDirBtn_Click(object sender, EventArgs e)
        {
            true.trydo(openDir);
        }

        void openDir()
        {
            if (!canOpenDir)
                return;
            if (treeActive)
            {
                if (selNode.isSpace())
                    selSpace.dir.dirOpen();
                else
                    openDirById((selNode.Tag as IResTag).id);
            }
            else if (graphActive)
                openDirById(pickItem.idTag().id);
        }

        void openDirById(string id)
            => $"{selSpace.dir}/{id}".dirOpen();

        bool canRun => activeTag is SyncTag;

        private void runBtn_Click(object sender, EventArgs e)
        {
            runSync();
        }

        void runSync()
        {
            if (!canRun)
                return;

            TreeNode tn = null;
            if (treeActive)
                tn = selNode;
            else if (graphActive)
                tn = (pickItem.Tag as SyncTag).node;
            else
                return;

            var st = tn.root().Tag as SpaceTag;
            var wi = new SyncItem
            {
                space = st.space,
                sync = (tn.Tag as SyncTag).entry,
            };
            true.msgRetain(() =>
            {
                var dlg = new WorkRunner { item = wi };
                dlg.dialog(this);
            });
        }

        bool canBatchRun => treeUI.Nodes.existNode(tn => tn.Nodes.Count > 0);

        private void batchRunBtn_Click(object sender, EventArgs e)
        {
            batchRun();
        }

        void batchRun()
        {
            if (!canBatchRun)
                return;

            true.msgRetain(() => new BatchRunner
            {
                spaces = treeUI.Nodes
                .pick<TreeNode>(tn => tn.Nodes.Count > 0)
                .conv(tn => tn.spaceTag().space)
            }.dialog(this));
        }

        bool canMoveUp => null != selNode && !selNode.isFirst();
        private void moveUpBtn_Click(object sender, EventArgs e)
        {
            if (!canMoveUp)
                return;
            true.trydo(() 
                => treeSelectEvent.suspend(() 
                => treeUI.moveUp(saveByNode)));
        }

        bool canMoveDown => null != selNode && !selNode.isLast();
        private void moveDownBtn_Click(object sender, EventArgs e)
        {
            if (!canMoveDown)
                return;
            true.trydo(() 
                => treeSelectEvent.suspend(() 
                => treeUI.moveDown(saveByNode)));
        }

        static CustomLineCap syncCap =
            new AdjustableArrowCap(6, 8, true) { MiddleInset = 4 };
        static Line masterLine = new Line { EndCap = syncCap };
        static Line interactLine = new Line { BeginCap = syncCap, EndCap = syncCap };
        static Line syncLine(SyncType type)
            => type == SyncType.MasterSync ? masterLine : interactLine;

        IEnumerable<Link> syncLinks(INode nd)
            => nd.links(lk => lk.Tag is SyncTag);

        Link syncLink(INode nd)
            => nd.links(lk => lk.Tag is SyncTag).first();

        string hashJson(INode nd)
            => syncLink(nd)?.syncTag().hash.json();

        IEnumerable<Link> allSyncLinks(INode nd)
            => nd.allLinks(lk => lk.Tag is SyncTag);

        void addSyncLink(INode src, INode dst, SyncType type)
        {
            var hjs = hashJson(src);
            if (null != hjs)
            {
                if (hjs != hashJson(dst))
                {
                    true.trydo(() =>
                    {
                        foreach (var lk in allSyncLinks(dst))
                        {
                            var tg = lk.syncTag();
                            tg.hash = hjs.obj<HashConf>();
                            tg.conf.save();
                        }
                    });
                }
            }
            else
                hjs = hashJson(dst);

            var syncId = $"sync{newId()}";
            var conf = new SyncConf
            {
                Type = $"{type}",
            };
            if (hjs != null)
                conf.Hash = hjs.obj<HashConf>();
            selSpace.createEntry(syncId, conf);

            var en = new SyncEntry
            {
                id = syncId,
                type = type,
                name = $"{(src as RectNode).Text} - {(dst as RectNode).Text}",
            };
            var tn = addSyncLink(en, src, dst);
            spaceNode.Expand();
            selNode = tn;

            saveWorkTree();
        }

        TreeNode addSyncLink(SyncEntry en, INode src, INode dst, Action<SyncConf> init = null)
        {
            ArgsConf conf = new ArgsConf
            {
                path = selSpace.entryPath(en.id),
                type = typeof(SyncConf),
            };
            var link = new LineLink(src, dst)
            {
                Line = syncLine(en.type)
            };
            spaceGraph.addItem(link);

            var tn = new TreeNode
            {
                Text = en.name,
                Checked = en.check,
            }.icon($"{en.type}");
            spaceNode.Nodes.Add(tn);

            var tag = new SyncTag
            {
                conf = conf,
                link = link,
                entry = en,
                node = tn,
            };
            tn.Tag = link.Tag = tag;
            conf.OnChange = tag.OnChange;

            return tn;
        }

        void saveWorkTree() => saveWorkTree(selNode);

        void saveWorkTree(TreeNode sn)
        {
            sn = (sn ?? selNode).root();
            var tag = sn.spaceTag();
            var syncs = sn.Nodes.conv<TreeNode, SyncEntry>(tn =>
            {
                var tg = tn.syncTag();
                var lk = tg.link;
                var en = tg.entry;
                en.src = lk.Source.idTag().id;
                en.dst = lk.Target.idTag().id;
                en.name = tn.Text;
                en.check = tn.Checked;
                return en;
            }).ToArray();

            tag.space.saveConf(new SpaceConf
            {
                reps = tag.getReps(),
                views = tag.getViews(),
                syncs = syncs,
            });
        }

        void saveByNode(TreeNode tn)
        {
            if (tn.isSpace())
                saveSpaceList();
            else
                saveWorkTree(tn);
        }

        bool canAddSync => pickCount == 2
                && pick0.canBeView() && pick1.canBeView()
                && !pick0.isLinked(pick1)
                && pick0.rootView() != pick1.rootView();

        private void addMasterBtn_Click(object sender, EventArgs e)
        {
            if (!canAddSync)
                return;

            true.trydo(() => addSyncLink(pick0 as INode, 
                pick1 as INode, SyncType.MasterSync));
        }

        private void addInterBtn_Click(object sender, EventArgs e)
        {
            if (!canAddSync)
                return;

            true.trydo(() => addSyncLink(pick0 as INode,
                pick1 as INode, SyncType.RoundSync));
        }

        bool canEditLabel => (treeActive && null != selNode)
                            || (graphActive && pickItem is IEditLabel);
        private void editLabelBtn_Click(object sender, EventArgs e)
        {
            if (!canEditLabel)
                return;

            if (treeActive)
                selNode.BeginEdit();
            else if (graphActive)
                linkUI.beginEdit(pickItem);
        }

        object activeTag => graphActive ? pickItem?.Tag
                        : (treeActive ? selNode?.Tag : null);

        bool canCopyArgs => activeTag is IConfTag;
        IConfTag copyTag = null;

        private void copyBtn_Click(object sender, EventArgs e)
        {
            copy();
        }

        void copy()
        {
            if (!canCopyArgs)
                return;

            copyTag = activeTag as IConfTag;
        }

        bool canPasteArgs => copyTag != null
                        && activeTag is IConfTag 
                        && activeTag != copyTag;

        private void pasteBtn_Click(object sender, EventArgs e)
        {
            paste();
        }

        void paste()
        {
            if (!canPasteArgs)
                return;

            true.trydo(() =>
            {
                var src = copyTag.conf.Args.jcopy();
                var conf = (activeTag as IConfTag).conf;
                if (conf.Args.paste(src, fld
                    => conf.OnChange?.Invoke(conf.Args, fld)))
                {
                    conf.save();
                    propUI.Refresh();
                }
            });
        }

        bool canSpreadArgs => pickItem?.Tag is SyncTag;

        private void spreadBtn_Click(object sender, EventArgs e)
        {
            if (!canSpreadArgs)
                return;
            if (!this.trans("ConfirmSpread").confirm())
                return;

            true.trydo(() => 
            {
                var plk = pickItem as Link;
                string js = null;
                foreach (var lk in plk.Source.allLinks(lk => lk.Tag is SyncTag))
                {
                    if (lk == plk)
                        continue;
                    js = js ?? plk.syncTag().args.json();
                    var conf = lk.syncTag().conf;
                    conf.Args.paste(js.obj<SyncConf>());
                    conf.save();
                }
            });
        }

        void tryPwd(Action func)
        {
            try
            {
                func();
            }
            catch (UserCancel) { }
            catch (Exception err)
            {
                err.notify();
            }
        }

        bool canBrowse => pickItem.canBeView() 
                            && pickItem.rootView().isRep();

        util.rep.View openView()
            => new SpaceContext(selSpace)
            .openView(pickItem.idTag().id);

        Reposit openRep()
            => new SpaceContext(selSpace)
            .openRep(pickItem.rootView().idTag().id);

        private void viewRepBtn_Click(object sender, EventArgs e)
        {
            browse();
        }

        void browse()
        {
            if (!canBrowse)
                return;

            tryPwd(() =>
            {
                using (var view = openView())
                {
                    true.msgRetain(() =>
                    {
                        new ViewBrowser()
                        {
                            rep = view.rep.src,
                            view = view,
                        }.dialog();
                    });
                }
            });
        }

        bool canManage => canBrowse;

        private void manageRepBtn_Click(object sender, EventArgs e)
        {
            if (!canManage)
                return;

            tryPwd(() =>
            {
                using (var rep = openRep())
                {
                    true.msgRetain(() =>
                    {
                        new RepManager()
                        {
                            rep = rep,
                        }
                        .dialog();
                    });
                }
            });
        }

        bool canMount => repTag != null && repTag.vfs == null;

        private void mountBtn_Click(object sender, EventArgs e)
        {
            mount();
        }

        void updateMountStatus(Process p, RectNode node, LinkViewGraph graph)
        {
            var mount = p != null;
            mountBtn.Enabled =
                    !(unmountBtn.Enabled = mount);
            node.Label.Brush = mount ? Brushes.Gold 
                : Brushes.LightGray;
            graph.redraw();
        }

        void mount()
        {
            if (!canMount)
                return;

            tryPwd(() =>
            {
                var tag = pickItem.repTag();
                var conf = tag.conf.Args as RepConf;

                var repArgs = conf.newRepArgs();

                var cmd = $"{true.appDir()}/{App.Option.VfsExe}";
                var args = new VfsArgs
                {
                    path = conf.Mount.Path,
                    name = conf.Mount.Name,
                    type = tag.entry.type,
                    src = conf.getSource(),
                    bak = conf.Backup.getFolder(),
                };

                var node = pickItem as RectNode;
                var graph = spaceTag.graph;

                args.mount(cmd, repArgs, before: p =>
                {
                    updateMountStatus(tag.vfs = p, node, graph);
                },
                after: p =>
                {
                    updateMountStatus(tag.vfs = null, node, graph);
                },
                stderr: s => s.msg(),
                stdout: s => s.msg());
            });
        }

        bool canUnmount => repTag?.vfs != null;

        private void unmountBtn_Click(object sender, EventArgs e)
        {
            if (canUnmount)
                true.trydo(repTag.vfs.Kill);
        }

        bool canModifyPwd => repTag?.canModifyPwd == true;

        private void modifyPwdBtn_Click(object sender, EventArgs e)
        {
            if (!canModifyPwd)
                return;

            true.trydo(() => 
            {
                if (repTag.args.modifyPwd())
                    propUI.Refresh();
            });
        }

        private void optionBtn_Click(object sender, EventArgs e)
        {
            var dlg = new util.OptionDialog
            {
                Path = App.ConfPath,
                Args = App.Option,
            };
            dlg.addBtn("evalPBKDF2Btn", Resources.EvalTime, () =>
            {
                "".dlgEvalTime(dlg.getArgs<AppOption>()
                .PBKDF2.evalTime);
            });
            dlg.addBtn("evalArgon2idBtn", Resources.EvalTime, () =>
            {
                "".dlgEvalTime(dlg.getArgs<AppOption>()
                .Argon2id.evalTime);
            });
            dlg.addBtn("evalPwdHashBtn", Resources.EvalTime, () =>
            {
                "".dlgEvalTime(() =>
                {
                    var key = "123abc".utf8();
                    var args = dlg.getArgs<AppOption>();
                    args.newPwdDerives(args.AeadFS.PwdDerives)
                    .each(kg => key = kg.deriveKey(key, 32));
                });
            });
            dlg.dialog();
            App.Option = dlg.Args as AppOption;
        }

        Item pick0 => pickItems[0];
        Item pick1 => pickItems[1];
        int? pickCount => pickItems?.Count;

        static CustomLineCap viewCap =
            new AdjustableArrowCap(4, 5, true)
            {
                MiddleInset = 2
            };
        static Line viewLine = new Line
        {
            EndCap = viewCap,
            Style = DashStyle.Dash,
            Width = 1,
        };

        bool canAddView => (pickCount == 1 && pick0.canBeView());

        private void addViewBtn_Click(object sender, EventArgs e)
        {
            if (!canAddView)
                return;

            true.trydo(() => 
            {
                var viewId = $"view{newId()}";
                var conf = new ViewConf();
                selSpace.createEntry(viewId, conf);

                var nd = pickItem as RectNode;
                ViewEntry en = new ViewEntry
                {
                    id = viewId,
                    name = nd.Text,
                    pos = new Point(nd.X + 200, nd.Y + (nd.isRep() ? 8 : 0)),
                };
                var dst = addViewNode(en);
                addViewLink(nd, dst);

                saveWorkTree();
            });
        }

        INode addViewNode(ViewEntry en)
        {
            var prop = new ArgsConf
            {
                path = selSpace.entryPath(en.id),
                type = typeof(ViewConf),
            };
            var nd = new RectNode(new Size(80, 80))
            {
                Pos = en.pos,
                Text = en.name,
                Font = linkUI.Font,
                Image = icons80.Images["View"],
                Tag = new ViewTag
                {
                    conf = prop,
                    entry = en,
                },
            };
            spaceGraph.addItem(nd);

            return nd;
        }

        bool canLinkView => pickCount == 2
                && pick0.canBeView() && pick1.isView()
                && !pick1.isRefed() && !pick1.isLinked(pick0);

        private void linkViewBtn_Click(object sender, EventArgs e)
        {
            if (!canLinkView)
                return;

            true.trydo(() => 
            {
                addViewLink(pick0 as INode, pick1 as INode);

                saveWorkTree();
            });
        }
    }
}
