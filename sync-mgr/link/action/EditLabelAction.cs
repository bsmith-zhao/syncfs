using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;

namespace link
{
    public class EditLabelAction : BaseAction
    {
        public EditLabelAction()
        {
            editor.KeyDown += Edit_KeyDown;
            editor.LostFocus += Editor_LostFocus;
        }

        private void Editor_LostFocus(object sender, EventArgs e)
        {
            stop();
        }

        private void Edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cancel = true;
                ui.Focus();
            }
        }

        TextBox editor = new TextBox
        {
            BorderStyle = BorderStyle.FixedSingle,
            Multiline = true,
            Visible = false,
            AcceptsTab = false,
            WordWrap = true,
        };

        IEditLabel item;

        public bool isEdit(IEditLabel item)
            => this.item == item;

        bool cancel = false;
        public EditLabelAction start(IEditLabel item)
        {
            ui.lastPick = null;
            if (!ui.Controls.Contains(editor))
                ui.Controls.Add(editor);

            if (editor.Visible && isEdit(item))
                return this;

            this.item = item;
            var label = item.Label;

            editor.Location = ui.toViewPos(label.Pos);
            editor.Text = label.Text;
            editor.Font = label.Font;
            editor.Width = label.Size.Width;
            editor.Height = label.measureHeight("1\r\n2")
                                .max(label.Size.Height);

            editor.Show();
            editor.Focus();

            cancel = false;

            return this;
        }

        public override void stop()
        {
            if (!editor.Visible)
                return;

            editor.Hide();

            reset();

            var label = item.Label;
            item = null;
            if (!cancel && label.Text != editor.Text)
            {
                label.Text = editor.Text;
                ui.OnLabelModified(item as Item);
            }
        }
    }
}
