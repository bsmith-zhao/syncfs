using System.Windows.Forms;

namespace util.ext
{
    public static class TextBoxEx
    {
        public static void msgToMeAsync(this TextBox ui)
        {
            Msg.output = ui.msgAsync;
        }

        public static void msgToMe(this TextBox ui)
        {
            Msg.output = ui.msgSync;
        }

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

        public static void msgAsync(this TextBox ui, object msg)
        {
            ui.runAsync(() => ui.addLine(msg));
        }

        public static void msgSync(this TextBox ui, object msg)
        {
            ui.safeRun(() => ui.addLine(msg));
        }

        static void addLine(this TextBox ui, object msg)
        {
            if (ui.Lines.Length > 500)
            {
                var text = ui.Text;
                var pos = text.Length / 2;
                pos = text.IndexOf("\r\n", pos) + 2;
                text = text.Substring(pos);
                ui.Text = text;
            }
            ui.AppendText((msg?.ToString() ?? "<null>") + "\r\n");
        }
    }
}
