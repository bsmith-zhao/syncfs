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
            foreach (var dir in dirInfo.EnumerateDirectories())
                if (parseItem(dir, out var path))
                    yield return new LocalDirItem(rep, dir, path);
        }

        public override IEnumerable<T> enumFiles<T>()
        {
            foreach (var file in dirInfo.EnumerateFiles())
                if (parseItem(file, out var path))
                    yield return rep.newFileItem<T>(file, path);
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
                        yield return rep.newFileItem<FileItem>(file, path);
                }
            }
        }

        //T newFileItem<T>(FileInfo file, string path)
        //    where T : FileItem, new()
        //{
        //    return rep.newFileItem<T>(file, path);
        //}

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
