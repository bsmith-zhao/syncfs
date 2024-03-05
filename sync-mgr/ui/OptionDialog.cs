using sync.app;
using System;
using System.Drawing;
using System.Windows.Forms;
using util;
using util.ext;
using util.prop;

namespace xtext
{
    public partial class OptionDialog : Form
    {
        public string Path;
        AppOption args;
        public AppOption Args
        {
            get => args;
            set => propUI.SelectedObject = args = value;
        }

        private void SetupDialog_Load(object sender, EventArgs e)
        {
            toolbar.adjustBtns(100);

            descSplit.BackColor
                = descUI.BackColor
                = Theme.ControlBack.zoom(1.1);
        }

        public OptionDialog()
        {
            InitializeComponent();

            toolbar.fixBorderBug();

            propUI.enhanceEdit((s,e) => saveConf(Args));
            propUI.enhanceDesc(descUI);
        }

        void saveConf(AppOption conf)
        {
            this.trydo(() => 
            {
                conf = conf ?? new AppOption();
                conf.jsonIndent().bakSaveTo(Path);
                this.Args = conf;
            });
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            if (!this.trans("ConfirmReset").confirm())
                return;

            saveConf(null);
        }

        private void openDirBtn_Click(object sender, EventArgs e)
        {
            Path.pathDir().dirOpen();
        }

        private void evalPbkdf2Btn_Click(object sender, EventArgs e)
        {
            evalPwdHash(() => Args.PBKDF2.create()
                            .genKey("123abc".utf8(), 32));
        }

        private void evalArgon2idBtn_Click(object sender, EventArgs e)
        {
            evalPwdHash(() => Args.Argon2id.create()
                            .genKey("123abc".utf8(), 32));
        }

        void evalPwdHash(Action func)
        {
            try
            {
                var begin = DateTime.UtcNow;
                func();
                var span = $"{DateTime.UtcNow - begin}";
                this.trans("PwdHashTime", span).dlgInfo();
            }
            catch (Exception err)
            {
                err.Message.dlgAlert();
            }
        }

        private void evalAeadFSBtn_Click(object sender, EventArgs e)
        {
            evalPwdHash(()=> 
            {
                var key = "123abc".utf8();
                Args.AeadFS.PwdDerives.each(kg 
                    => key = Args.newKeyGen(kg).genKey(key, 32));
            });
        }
    }
}
