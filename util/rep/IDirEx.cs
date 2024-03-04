using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public static class IDirEx
    {
        public static bool getItem(this IDir src, string path, out RepItem item)
            => (item = src?.getItem(path)) != null;

        public static DirItem getDir(this IDir src, string path)
            => src?.getItem(path) as DirItem;

        public static DirItem getRoot(this IDir src)
            => getDir(src, null);

        public static bool getDir(this IDir src, 
            string path, out DirItem dir)
            => (dir = src.getDir(path)) != null;

        public static IEnumerable<string> enumDirs(this IDir src, string path = null)
        => (src.getDir(path)?.enumDirs().conv(d => d.path)).usable();

        public static IEnumerable<RepItem> enumItems(this IDir src, string path = null)
            => src.getDir(path)?.enumItems();

        public static IEnumerable<string> enumAllDirs(this IDir src, string path = null)
        => (src.getDir(path)?.enumAllDirs().conv(d => d.path)).usable();

        public static IEnumerable<T> enumFiles<T>(this IDir src, string path = null)
            where T : FileItem, new()
            => (src.getDir(path)?.enumFiles<T>()).usable();

        public static IEnumerable<FileItem> enumFiles(this IDir src, string path = null)
            => src.enumFiles<FileItem>(path);

        public static IEnumerable<T> enumAllFiles<T>(this IDir src, string path = null)
            where T : FileItem, new()
            => (src.getDir(path)?.enumAllFiles<T>()).usable();

        public static IEnumerable<FileItem> enumAllFiles(this IDir src, string path = null)
            => src.enumAllFiles<FileItem>(path);
    }
}
