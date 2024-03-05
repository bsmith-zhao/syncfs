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
using util.rep.aead;
using xtext.Properties;

namespace xtext
{
    public partial class XTextEditor : Form
    {
        public XTextEditor()
        {
            InitializeComponent();

            msgUI.msgToMeAsync();

            toolbar.fixBorderBug();

            textUI.TextChanged += (s, e) =>
            {
                modify = true;
            };

            textUI.KeyDown += (s, e) =>
            {
                if (e.CtrlS())
                {
                    if (e.Shift)
                        saveAs();
                    else if (modify)
                        save();
                }
            };

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += (s, e) =>
            {
                if (!modify)
                    return;

                if ("Close will discard changes, are you sure?".confirm()
                    && "Double confirm discard changes?".confirm())
                    return;

                e.Cancel = true;
            };
        }

        private void NoteForm_Load(object sender, EventArgs e)
        {
        }

        const string FileFilter = "Extend Text (*.xtext)|*.xtext|All Files (*.*)|*.*";

        bool _mod = false;
        bool modify
        {
            get => _mod;
            set
            {
                value.update(ref _mod, updateTitle);
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            this.trydo(open);
        }

        void open()
        {
            if (modify && !"Open will discard changes, are you sure?".confirm())
                return;

            if (!this.pickFile(out var path, FileFilter))
                return;

            openText(path);
        }

        string loadText(string path, byte[] pwd)
        {
            var txt = TextFile.load(path);
            if (!txt.decrypt(pwd))
                return null;
            return txt.getData().utf8();
        }

        void openText(string path)
        {
            var txt = TextFile.load(path);

            byte[] pwd = null;
            if (!path.queryPwd(p => txt.decrypt(pwd = p)))
                return;

            textUI.Text = txt.getData().utf8();

            filePath = path;
            filePwd = pwd;
            modify = false;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            this.trydo(save);
        }

        void save()
        {
            var path = filePath;
            if (path == null)
            {
                if (!"".saveFile(out path, FileFilter))
                    return;
            }

            saveText(path);
        }

        private void saveAsBtn_Click(object sender, EventArgs e)
        {
            this.trydo(saveAs);
        }

        void saveAs()
        {
            if (!this.saveFile(out var path, filter: FileFilter))
                return;

            saveText(path);
        }

        byte[] _fpwd;
        byte[] filePwd
        {
            get => _fpwd;
            set
            {
                value.update(ref _fpwd, updateTitle);
            }
        }
        string _fpath;
        string filePath
        {
            get => _fpath;
            set
            {
                value.update(ref _fpath, updateTitle);
            }
        }

        void updateTitle()
        {
            var mark = modify ? "*" : "";
            this.Text = $"{this.Name} [{filePath}] <{filePwd?.GetHashCode()}> {mark}";
        }

        void saveText(string path)
        {
            byte[] pwd = filePwd;
            if (pwd == null)
            {
                if (!path.setPwd(out var p))
                    return;
                pwd = p;
            }

            var conf = App.Option.ExtendText.createText();
            var pds = App.Option.PwdDerive.newPwdDerives(App.Option.ExtendText.PwdDerives);

            conf.setData(textUI.Text.utf8());

            conf.save(path, pwd, pds);

            var savedText = loadText(path, pwd);
            msgUI.Text = savedText;
            if (textUI.Text != savedText)
            {
                "Verify failed!!".dlgAlert();
                return;
            }

            filePwd = pwd;
            filePath = path;
            modify = false;
        }

        private void optionBtn_Click(object sender, EventArgs e)
        {
            var dlg = new OptionDialog
            {
                Path = App.ConfPath,
                Args = App.Option,
            };
            dlg.addBtn("evalPBKDF2Btn", Resources.EvalTime, ()=> 
            {
                "".dlgEvalTime(dlg.getArgs<AppOption>()
                .PwdDerive.PBKDF2.evalTime);
            });
            dlg.addBtn("evalArgon2idBtn", Resources.EvalTime, () =>
            {
                "".dlgEvalTime(dlg.getArgs<AppOption>()
                .PwdDerive.Argon2id.evalTime);
            });
            dlg.addBtn("evalPwdHashBtn", Resources.EvalTime, () =>
            {
                "".dlgEvalTime(() =>
                {
                    var key = "123abc".utf8();
                    var args = dlg.getArgs<AppOption>();
                    args.PwdDerive.newPwdDerives(args.ExtendText.PwdDerives)
                    .each(kg => key = kg.deriveKey(key, 32));
                });
            });
            dlg.dialog();
            App.Option = dlg.Args as AppOption;
        }

        private void setPwdBtn_Click(object sender, EventArgs e)
        {
            if ("".setPwd(out var pwd))
                filePwd = pwd;
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            if (modify && !"New will discard changes, are you sure?".confirm())
                return;

            textUI.Text = "";

            filePath = null;
            modify = false;
        }
    }
}
