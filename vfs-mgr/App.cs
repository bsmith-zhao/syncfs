using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using vfs.mgr.conf;

namespace vfs.mgr
{
    public class App
    {
        public static string ConfPath => $"{"".appTrunk()}.conf";
        public static string VfsListPath => $"{"".appDir()}\\vfs-list.conf";

        public static AppOption Option = new AppOption();

        public static string VfsCmd => $"{"".appDir()}/{Option.VfsExe}";

        public static void loadConf()
        {
            if (ConfPath.fileExist())
                Option = ConfPath.readText().obj<AppOption>();
        }
    }
}
