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
        public static string appPath(this object src)
            => Application.ExecutablePath.pathUnify();

        public static string appTrunk(this object src)
            => appPath(src).pathTrunk();

        public static string appDir(this object src)
            => Application.StartupPath.pathUnify();

        public static string fsBackup(this string path, 
            int total, Func<string, bool> exist, 
            Action<string> delete, string sep = null)
        {
            total = total.minLimit(2);
            var trk = path.pathTrunk();
            var ext = path.pathExt();
            string newPath = null;
            int idx = 0;
            while (idx++ < total)
            {
                newPath = makePath(trk, sep, idx, ext);
                if (!exist(newPath))
                    break;
            }
            if (idx > total)
            {
                idx = 1;
                newPath = makePath(trk, sep, idx, ext);
                delete(newPath);
            }
            var delPath = makePath(trk, sep, idx==total?1:(idx + 1), ext);
            if (exist(delPath))
                delete(delPath);

            return newPath;
        }

        static string makePath(string trk, string sep, int idx, string ext)
            => $"{trk}{sep}{idx}{ext}";

        public static void fsPush(this string path, int cnt, 
            Func<string, bool> exist, Action<string> delete, 
            Action<string, string> move, string sep = null)
        {
            var trk = path.pathTrunk();
            var ext = path.pathExt();
            var lastPath = $"{trk}{sep}{cnt}{ext}";
            if (exist(lastPath))
                delete(lastPath);
            while (cnt > 1)
            {
                var bakPath = $"{trk}{sep}{cnt - 1}{ext}";
                if (exist(bakPath))
                    move(bakPath, $"{trk}{sep}{cnt}{ext}");
                cnt--;
            }
            if (exist(path))
                move(path, $"{trk}{sep}{1}{ext}");
        }

        public static string fsSettle(this string path, 
            Func<string, bool> exist, string sep = null)
        {
            if (!exist(path))
                return path;
            return path.fsAppend(exist, sep);
        }

        public static string fsAppend(this string path, 
            Func<string, bool> exist, string sep = null)
        {
            var trk = path.pathTrunk();
            var ext = path.pathExt();
            int idx = 1;
            while (idx < 1000)
            {
                var newPath = makePath(trk, sep, idx++, ext);
                if (!exist(newPath))
                    return newPath;
            }
            throw new Error(typeof(Path), "AppendOverflow", idx);
        }

        public static bool fsExist(this string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public static string fsSettle(this string path, string sep = null)
        {
            return fsSettle(path, fsExist, sep);
        }

        public static string fsReloc(this string path, 
            Func<string, bool> exist, 
            Action<string, string> move)
        {
            var newPath = fsSettle(path, exist);
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
