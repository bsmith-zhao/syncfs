using System;
using System.Collections.Generic;
using util;
using util.ext;

namespace sync.app
{
    public class App
    {
        public static string ConfPath 
            = $"{true.appTrunk()}.conf";

        public static string SpaceListPath 
            = $"{true.appDir()}/space-list.conf";

        public static AppOption Option = new AppOption();

        public static void init()
        {
            try
            {
                if (ConfPath.fileExist())
                    Option = ConfPath.readText().obj<AppOption>();
            }
            catch (Exception err)
            {
                typeof(AppOption).trans("LoadFail", ConfPath, err.Message).msg();
            }
        }

        public static IEnumerable<SpaceEntry> enumSpaces()
        {
            if (SpaceListPath.fileExist())
                foreach (var sp in SpaceListPath.readText().obj<SpaceEntry[]>())
                    yield return sp;
        }

        public static void saveSpaces(IEnumerable<SpaceEntry> sps)
            => sps.jsonIndent().bakSaveTo(SpaceListPath);
    }
}
