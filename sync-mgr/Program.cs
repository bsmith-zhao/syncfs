using sync.app;
using sync.ui;
using System;
using System.Windows.Forms;
using util;
using util.ext;

namespace sync
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (new FileLogger())
            {
                // create ui msg receiver
                var form = new SyncManager();

                // load lang files
                Lang.init();

                // load app conf
                App.init();

                // run ui form as app
                form.launch();
            }
        }
    }
}
