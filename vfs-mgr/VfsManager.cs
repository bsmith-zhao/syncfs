using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.crypt;
using util.ext;
using util.prop;
using util.rep;
using util.rep.aead;
using vfs.mgr.conf;
using vfs.mgr.Properties;

namespace vfs.mgr
{
    public partial class VfsManager : Form
    {
        public VfsManager()
        {
            InitializeComponent();

            Msg.output = msgUI.asyncAppend;

            toolbar.fixBorderBug();

            listUI.SelectedIndexChanged += (s, e) =>
            {
                var conf = selConf;
                if (conf?.getRepConf() is IDynamicConf dc)
                    dc.OnActive();
                propUI.SelectedObject = conf;
                propUI.ExpandAllGridItems();
            };
            listUI.SelectedIndexChanged += (s, e) => updateBtns();
            listUI.MouseDoubleClick += ListUI_MouseDoubleClick;

            propUI.enhanceEdit((s, e) =>
            {
                updateSelItem();
                saveVfsList();
            });
            propUI.enhanceDesc(descUI);
            propUI.PropertySort = PropertySort.NoSort;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ListUI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var tag = selTag;
            if (tag == null)
                return;

            if (tag.mount == null)
            {
                mount();
            }
            else
            {
                unmount();
            }
        }

        ListViewItem selItem => listUI.selItem();
        VfsTag selTag => selItem?.Tag as VfsTag;
        VfsConf selConf => selTag?.conf;
        RepConf selRepConf => selConf?.getRepConf();

        private void VfsManager_Load(object sender, EventArgs e)
        {
            this.trydo(App.loadConf);

            updateTitle();
            languageBtn.initLang(translate);
            toolbar.adjustBtns();

            descSplit.BackColor
                = descUI.BackColor
                = Theme.ControlBack.zoom(1.1);

            var grp = vfsGroup;
            grp = mountGroup;

            this.trydo(loadVfsList);
            loadMounts();

            updateBtns();
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

        void updateBtns()
        {
            deleteBtn.Enabled = canDelete;
            mountBtn.Enabled = canMount;
            unmountBtn.Enabled = canUnmount;
            modifyPwdBtn.Enabled = canModifyPwd;
            openDirBtn.Enabled = canOpenDir;
        }

        void loadVfsList()
        {
            if (App.VfsListPath.fileExist())
                App.VfsListPath.readText().obj<VfsConf[]>()
                    .each(addVfsItem);
        }

        void updateSelItem()
        {
            var item = selItem;
            var conf = selConf;
            if (item == null || conf == null)
                return;
            item.label(1, conf.mountInfo())
                .label(3, conf.sourcPath());
        }

        string newKey()
        {
            var key = DateTime.Now.ToString("MMddHHmmss.fff");
            int idx = 1;
            while (getTag(key, out var tag))
            {
                key = $"{key}.{idx++}";
            }
            return key;
        }

        bool getTag(string key, out VfsTag tag)
            => (tag = listUI.Items[key]?.Tag as VfsTag) != null;

        void addVfsItem(VfsConf conf)
        {
            var labels = new string[]
            {
                null,
                conf.mountInfo(),
                conf.Type.ToString(),
                conf.sourcPath(),
            };
            var item = new ListViewItem(labels);
            var tag = new VfsTag
            {
                item = item,
                conf = conf,
            };
            item.Tag = tag;

            vfsGroup.add(item);
        }

        void addAeadFs()
        {
            var repConf = new AeadRepConf();

            if (!repConf.createRep(out var dir))
                return;

            var conf = new VfsConf
            {
                Path = "V:",
                Name = dir.pathName().discard(s => s.empty()) ?? "VFS",
                Type = RepType.AeadFS,
                Source = repConf
            };

            addVfsItem(conf);

            saveVfsList();
        }

        void saveVfsList()
        {
            vfsGroup.Items.conv<ListViewItem, VfsConf>(it => (it.Tag as VfsTag).conf)
                .jsonIndent().saveTo(App.VfsListPath);
        }

        private void addAeadFSBtn_Click(object sender, EventArgs e)
        {
            addAeadFs();
        }

        bool canModifyPwd => selRepConf?.canModifyPwd() == true;

        private void modifyPwdBtn_Click(object sender, EventArgs e)
        {
            if (!canModifyPwd)
                return;

            selRepConf.modifyPwd();
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

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            loadMounts();
        }

        VfsTag getTag(ListViewItem item)
            => item.Tag as VfsTag;

        void loadMounts()
        {
            MountInfo.enumMounts().each(m =>
            {
                if (listUI.Items.exist<ListViewItem>(it 
                    => getTag(it).mount?.proc.Id == m.proc.Id))
                    return;

                if (listUI.Items.conv<ListViewItem, VfsTag>(it=>getTag(it))
                    .first(t => t.mount == null
                    && t.conf.Path == m.path
                    && t.conf.Name == m.name
                    && t.conf.Type == m.type
                    && t.conf.sourcPath() == m.src, out var tag))
                {
                    tag.mount = m;
                    updateMountStatus(tag);
                    awaitMountEnd(tag.mount, () => 
                    {
                        tag.mount = null;
                        updateMountStatus(tag);
                    });
                    return;
                }

                addMountItem(m);
            });
        }

        void addMountItem(MountInfo mount)
        {
            var labels = new string[]
            {
                mount.info,
                null,
                mount.type.ToString(),
                mount.src,
            };
            var item = new ListViewItem(labels)
            {
                ForeColor = Color.LightGreen,
            };
            var tag = new VfsTag
            {
                item = item,
                mount = mount,
            };
            item.Tag = tag;

            mountGroup.add(item);

            awaitMountEnd(mount, item.Remove);
        }

        void vfsError(string err)
            => err.msg();

        void vfsMsg(string info)
            => info.msg();

        ListViewGroup _mountGroup;
        ListViewGroup mountGroup 
            => _mountGroup ?? (_mountGroup = listUI.group(this.trans("MountList")));

        ListViewGroup _vfsGroup;
        ListViewGroup vfsGroup
            => _vfsGroup ?? (_vfsGroup = listUI.group(this.trans("VFSList")));

        MountInfo selMount => selTag?.mount;

        bool canMount => selItem != null && selMount == null;

        private void mountBtn_Click(object sender, EventArgs e)
        {
            mount();
        }

        void mount()
        {
            if (!canMount)
                return;

            try
            {
                var tag = selTag;
                var conf = tag.conf;
                var repArgs = conf.newRepArgs();
                var args = new VfsArgs
                {
                    path = conf.Path,
                    name = conf.Name,
                    type = conf.Type,
                    src = conf.sourcPath(),
                };
                args.mount(App.VfsCmd, repArgs, before: p =>
                {
                    tag.mount = new MountInfo
                    {
                        args = args,
                        proc = p,
                    };
                    updateMountStatus(tag);
                }, after: p =>
                {
                    tag.mount = null;
                    updateMountStatus(tag);
                }, stdout: vfsMsg, stderr: vfsError);
            }
            catch (CancelPwd) { }
            catch (Exception err)
            {
                err.handle();
            }
        }

        void updateMountStatus(VfsTag tag)
        {
            var item = tag.item;
            var mount = tag.mount;
            item.ForeColor = mountColor(mount);
            item.label(0, mount?.info);

            updateBtns();
        }

        void awaitMountEnd(MountInfo mount, Action end)
            => mount.proc.awaitEnd(p => end());

        Color mountColor(MountInfo m)
            => m != null ? Color.Gold : listUI.ForeColor;

        bool canUnmount => selMount != null;

        private void unmountBtn_Click(object sender, EventArgs e)
        {
            unmount();
        }

        void unmount()
        {
            if (canUnmount && this.trans("ConfirmUnmount", selMount.info).confirm())
                selMount.proc.Kill();
        }

        bool canDelete => selConf != null;

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (!canDelete)
                return;
            if (!this.trans("CofirmDelete").confirm())
                return;

            listUI.SelectedItems
                .newList<ListViewItem>()
                .each(it => it.Remove());
            saveVfsList();
        }

        bool canOpenDir => selMount != null;

        private void openDirBtn_Click(object sender, EventArgs e)
        {
            if (canOpenDir)
                selMount.path.dirOpen();
        }
    }
}
