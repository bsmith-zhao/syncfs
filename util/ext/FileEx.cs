using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class FileEx
    {
        public static long createTime(this FileInfo fi)
            => fi.CreationTimeUtc.ToFileTime();

        public static long writeTime(this FileInfo fi)
            => fi.LastWriteTimeUtc.ToFileTime();

        public static bool fileExist(this string path)
            => !path.empty() && File.Exists(path);

        public static void fileLink(this string path, string target)
        {
            path = path.Replace("/", "\\");

            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            var shortcut = shell.CreateShortcut(path);
            shortcut.TargetPath = target;
            shortcut.Arguments = "";
            shortcut.WorkingDirectory = path.Substring(0, path.LastIndexOf("\\"));
            shortcut.Save();
        }

        public static string readText(this string path)
        {
            return File.ReadAllText(path);
        }

        public static void bakSaveTo(this string text, string path)
        {
            var tmp = $"{path}.tmp";
            File.WriteAllText(tmp, text);
            var bak = $"{path}.bak";
            if (File.Exists(bak))
                File.Delete(bak);
            if (File.Exists(path))
                File.Move(path, bak);
            File.Move(tmp, path);
        }

        public static void saveTo(this string text, string path)
        {
            File.WriteAllText(path, text);
        }

        public static void saveTextTo<T>(this IEnumerable<T> iter, string path)
        {
            using (var fout = File.CreateText(path))
            {
                foreach (var obj in iter)
                    fout.WriteLine(obj?.ToString() ?? "");
            }
        }

        public static List<string> readList(this string path)
        {
            var list = new List<string>();
            if (path.fileExist())
            {
                using (var fin = File.OpenText(path))
                {
                    string row;
                    while (fin.readRow(out row))
                        list.Add(row);
                }
            }
            return list;
        }

        public static bool isDir(this FileSystemInfo fi)
            => fi.Attributes.HasFlag(FileAttributes.Directory);
        public static bool isHidden(this FileSystemInfo fi)
            => fi.Attributes.HasFlag(FileAttributes.Hidden);
        public static bool isSystem(this FileSystemInfo fi)
            => fi.Attributes.HasFlag(FileAttributes.System);
    }
}
