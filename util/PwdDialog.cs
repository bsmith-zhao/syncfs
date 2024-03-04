using util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util.ext;
using util.crypt;

namespace util
{
    public partial class PwdDialog : Form
    {
        public string title;
        public Exception error;

        public PwdDialog()
        {
            InitializeComponent();
        }

        private void PwdDialog_Load(object sender, EventArgs e)
        {
            if (!title.empty())
                this.Text = $"{this.Text} - {title}";
        }

        public PwdDialog query(Func<byte[], bool> verify)
        {
            hidePwdRows(1, 2);
            okBtn.Click += (s, e) =>
            {
                try
                {
                    var pwd = pwdUI.Text.utf8();
                    if (verify(pwd))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                        showMismatch();
                }
                catch (Exception err)
                {
                    this.error = err;
                    this.Close();
                }
            };
            return this;
        }

        void showMismatch()
            => this.trans("Mismatch").dlgError();

        public PwdDialog modify(Func<byte[], bool> verify)
        {
            okBtn.Click += (s, e) =>
            {
                try
                {
                    if (!checkNewPwd())
                        return;
                    var oldPwd = pwdUI.Text.utf8();
                    if (verify(oldPwd))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                        showMismatch();
                }
                catch (Exception err)
                {
                    this.error = err;
                    this.Close();
                }
            };
            return this;
        }

        public byte[] newPwd => newPwdUI.Text.utf8();

        public PwdDialog setup()
        {
            hidePwdRows(0);
            okBtn.Click += (s, e) =>
            {
                if (!checkNewPwd())
                    return;
                this.DialogResult = DialogResult.OK;
                Close();
            };
            return this;
        }

        bool checkNewPwd()
        {
            var pwd = newPwdUI.Text;
            if (pwd != repeatPwdUI.Text)
            {
                this.trans("RepeatMismatch").dlgError();
                return false;
            }
            return pwd.Length > 0 
                || this.trans("ConfirmEmpty").confirm();
        }

        void hidePwdRows(params int[] rows)
        {
            float sum = 0;
            foreach (var idx in rows)
            {
                sum += hidePwdRow(idx);
            }
            this.Height -= (int)sum;
        }

        float hidePwdRow(int idx)
        {
            idx += 1;
            var tb = fldsLayout;
            var height = tb.RowStyles[idx].Height;
            tb.RowStyles[idx].Height = 0;
            for (int i = 0; i < tb.ColumnCount; i++)
            {
                var ui = tb.GetControlFromPosition(i, idx);
                if (null != ui)
                    ui.Visible = false;
            }
            return height;
        }

        private void pwdIcon_Click(object sender, EventArgs e)
        {
            if (pwdIcon.Image == showIcon.Image)
            {
                pwdUI.PasswordChar = '*';
                pwdIcon.Image = hideIcon.Image;
            }
            else
            {
                pwdUI.PasswordChar = new char();
                pwdIcon.Image = showIcon.Image;
            }
        }

        private void newPwdIcon_Click(object sender, EventArgs e)
        {
            if (newPwdIcon.Image == showIcon.Image)
            {
                newPwdUI.PasswordChar = '*';
                repeatPwdUI.PasswordChar = '*';
                newPwdIcon.Image = hideIcon.Image;
            }
            else
            {
                newPwdUI.PasswordChar = new char();
                repeatPwdUI.PasswordChar = new char();
                newPwdIcon.Image = showIcon.Image;
            }
        }

        private void PwdDialog_Activated(object sender, EventArgs e)
        {
            if (pwdUI.Visible)
                pwdUI.Focus();
            else
                newPwdUI.Focus();
        }
    }
}
