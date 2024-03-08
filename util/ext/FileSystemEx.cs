using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class FileSystemEx
    {
        public static string appPath(this bool src)
            => Application.ExecutablePath.pathUnify();

        public static string appTrunk(this bool src)
            => appPath(src).pathTrunk();

        public static string appDir(this bool src)
            => Application.StartupPath.pathUnify();

        public static bool fsExist(this string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public static string fsSettle(this string path, string sep = null)
        {
            return path.pathSettle(fsExist, sep);
        }

        public static string fsReloc(this string path, 
            Func<string, bool> exist, 
            Action<string, string> move)
        {
            var newPath = path.pathSettle(exist);
            move(path, newPath);
            return newPath;
        }

        public static bool dirExist(this string path)
            => !path.empty() && Directory.Exists(path);

        public static void dirCreate(this string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void dirDelete(this string path, bool recursive = false)
        {
            Directory.Delete(path, recursive);
        }

        public static void dirOpen(this string path)
        {
            path = path.Replace("/", "\\");
            Process.Start("explorer.exe", path);
        }

        public static void drvSpace(this string path, out long total, out long free)
        {
            var di = new DriveInfo(path);
            total = di.TotalSize;
            free = di.AvailableFreeSpace;
        }
    }
}
