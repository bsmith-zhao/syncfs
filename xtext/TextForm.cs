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
    public partial class TextForm : Form
    {
        public TextForm()
        {
            InitializeComponent();

            msgUI.msgToMeAsync();

            toolbar.fixBorderBug();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void NoteForm_Load(object sender, EventArgs e)
        {
        }

        const string TextFilter = "Extend Text (*.xtext)|*.xtext|All Files (*.*)|*.*";

        bool modify = false;

        private void openBtn_Click(object sender, EventArgs e)
        {
            if (modify && !"Open will drop changes, are you sure?".confirm())
                return;

            if (!this.pickFile(out var path, TextFilter))
                return;

            openText(path);
        }

        void openText(string path)
        {
            var txt = ExtendText.load(path);

            byte[] pwd;
            if (!path.queryPwd(p => txt.decrypt(pwd = p)))
                return;

            byte[] data = txt.getData();
            textUI.Text = data.utf8();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {

        }

        private void saveAsBtn_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        void saveAs()
        {
            if (!this.saveFile(out var path, filter: TextFilter))
                return;

            saveNote(path);
        }

        void saveNote(string path)
        {
            if (!path.setPwd(out var pwd))
                return;

            var conf = App.Option.ExtendText.createText();
            var pds = App.Option.PwdDerive.newPwdDerives(App.Option.ExtendText.PwdDerives);

            conf.setData(textUI.Text.utf8());

            conf.save(path, pwd, pds);
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
    }
}
