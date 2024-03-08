using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public class LocalDirItem : DirItem
    {
        public LocalDirItem(LocalDirReposit rep,
            DirectoryInfo dir,
            string path)
        {
            this.rep = rep;
            this.dirInfo = dir;
            this.path = path;
        }

        LocalDirReposit rep;
        DirectoryInfo dirInfo;

        public override IEnumerable<DirItem> enumDirs()
        {
            foreach (var sub in dirInfo.EnumerateDirectories())
                if (parseItem(sub, out var path))
                    yield return new LocalDirItem(rep, sub, path);
        }

        public override IEnumerable<T> enumFiles<T>()
        {
            foreach (var fi in dirInfo.EnumerateFiles())
                if (parseItem(fi, out var path))
                    yield return newFileItem<T>(fi, path);
        }

        public override IEnumerable<RepItem> enumItems()
        {
            foreach (var item in dirInfo.EnumerateFileSystemInfos())
            {
                if (parseItem(item, out var path))
                {
                    if (item is DirectoryInfo dir)
                        yield return new LocalDirItem(rep, dir, path);
                    else if (item is FileInfo file)
                        yield return newFileItem<FileItem>(file, path);
                }
            }
        }

        T newFileItem<T>(FileInfo file, string path)
            where T : FileItem, new()
        {
            file.Refresh();
            return rep.newFileItem<T>(file, path);
        }

        bool decodePath(FileSystemInfo item, out string itemPath)
        {
            itemPath = rep.decodeName(item);
            if (itemPath != null && path != null)
                itemPath = $"{path}/{itemPath}";
            return itemPath != null;
        }

        bool parseItem(FileSystemInfo item, out string path)
        {
            path = null;
            if (item.isHidden() && item.isSystem())
                return false;
            return decodePath(item, out path);
        }
    }
}
