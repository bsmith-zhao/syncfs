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
        public static Stream openFile(this IDir dir, 
            string path, bool write)
            => write 
            ? dir.writeFile(path) 
            : dir.readFile(path);

        public static bool empty(this DirItem dir)
            => !dir.enumItems().exist(n => true);

        public static bool isDir(this RepItem item)
            => item is DirItem;

        public static bool isFile(this RepItem item)
            => item is FileItem;

        public static DirItem asDir(this RepItem item)
            => item as DirItem;

        public static bool asDir(this RepItem item,
                                out DirItem dir)
            => (dir = item as DirItem) != null;

        public static FileItem asFile(this RepItem item)
            => item as FileItem;

        public static void moveItem(this IDir dir, 
            RepItem item, string dst)
        {
            if (item is DirItem)
                dir.moveDir(item.path, dst);
            else
                dir.moveFile(item.path, dst);
        }

        public static void deleteItem(this IDir dir,
            RepItem item)
        {
            if (item is DirItem)
                dir.deleteDir(item.path);
            else
                dir.deleteFile(item.path);
        }

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
