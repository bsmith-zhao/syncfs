using sync;
using sync.app;
using sync.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;

namespace sync_bat
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (new FileLogger())
            {
                // create ui msg receiver
                var form = new BatchRunner
                {
                    dir = args.item(0),
                };

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
