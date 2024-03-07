using System.Windows.Forms;

namespace util.ext
{
    public static class TextBoxEx
    {
        public static void scrollByWheel(this TextBox ui)
        {
            ui.MouseWheel += (s, e) => 
            {
                if (e.Delta > 0)
                    SendKeys.Send("{UP}");
                else
                    SendKeys.Send("{DOWN}");
            };
        }

        public static void asyncAppend(this TextBox ui, object msg)
        {
            ui.asyncCall(() => ui.addRow(msg));
        }

        public static void syncAppend(this TextBox ui, object msg)
        {
            ui.syncCall(() => ui.addRow(msg));
        }

        static void addRow(this TextBox ui, object msg)
        {
            if (ui.Lines.Length > 500)
            {
                var text = ui.Text;
                var pos = text.Length / 2;
                pos = text.IndexOf("\r\n", pos) + 2;
                text = text.Substring(pos);
                ui.Text = text;
            }
            ui.AppendText($"{msg}\r\n");
        }
    }
}
