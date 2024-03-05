using System;
using System.Drawing;
using System.Windows.Forms;
using util;
using util.ext;
using util.prop;

namespace util
{
    public partial class OptionDialog : Form
    {
        public string Path;

        Type type;
        object _args;
        public object Args
        {
            get => _args;
            set
            {
                propUI.SelectedObject = _args = value;
                type = value.GetType();
            }
        }

        public T getArgs<T>() => (T)Args;

        private void OptionDialog_Load(object sender, EventArgs e)
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

        public OptionDialog addBtn(string name, Image icon,
            Action func)
        {
            var label = nameof(OptionDialog).transUIFld(name);

            var btn = new ToolStripButton();
            btn.Image = icon;
            btn.ImageTransparentColor = Color.Magenta;
            btn.Name = name;
            //btn.Size = new Size(153, 67);
            btn.Text = label;
            btn.TextImageRelation = TextImageRelation.ImageAboveText;
            btn.Click += (s, e) => func();

            toolbar.Items.Add(btn);

            return this;
        }

        void saveConf(object conf = null)
        {
            this.trydo(() => 
            {
                conf = conf ?? type.@new();
                conf.jsonIndent().bakSaveTo(Path);
                this.Args = conf;
            });
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            if (!this.trans("ConfirmReset").confirm())
                return;

            saveConf();
        }

        private void openDirBtn_Click(object sender, EventArgs e)
        {
            Path.pathDir().dirOpen();
        }
    }
}
