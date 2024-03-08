using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public abstract class LocalDirReposit : Reposit, ILocalDirReposit
    {
        public override void getSpace(out long total, out long free)
        {
            $"{rootPath}/".drvSpace(out total, out free);
        }

        public override bool exist(string path)
            => locateToItem(path, out var realPath) != null;

        public override RepItem getItem(string path)
        {
            if (!locateToItem(path, out var item, out var realPath))
                return null;
            else if (item is DirectoryInfo dir)
                return new LocalDirItem(this, dir, realPath);
            else
                return newFileItem<FileItem>(
                    new FileInfo(item.FullName),
                    realPath);
        }

        public T newFileItem<T>(FileInfo file, string path)
            where T : FileItem, new()
            => new T
            {
                path = path,
                size = getFileSize(file),
                createTime = file.createTime(),
                modifyTime = file.writeTime(),
            };

        public virtual string decodeName(FileSystemInfo item)
            => item.Name;

        protected T getSubItem<T>(IEnumerable<T> items,
            string name, out string realName)
            where T : FileSystemInfo
        {
            realName = null;
            name = name.low();
            foreach (var f in items)
                if ((realName = decodeName(f)).low() == name)
                    return f;
            return null;
        }

        protected DirectoryInfo getSubDir(DirectoryInfo dir, 
            string name, out string realName)
            => getSubItem(dir.EnumerateDirectories(), name, out realName);

        protected FileInfo getSubFile(DirectoryInfo dir, 
            string name)
            => getSubItem(dir.EnumerateFiles(), name, out var realName);

        protected FileSystemInfo getSubItem(DirectoryInfo dir, 
            string name, out string realName)
            => getSubItem(dir.EnumerateFileSystemInfos(), name, out realName);

        protected bool locateToName(string path,
            out DirectoryInfo dir,
            out string realDir,
            out string name)
            => (dir = locateToParent(path, out name, out realDir)) != null
            && name != null;

        protected bool locateToFile(string path, out FileInfo file)
            => (file = locateToFile(path)) != null;

        protected FileInfo locateToFile(string path)
        {
            if (locateToName(path, out var dir, out var realDir, out var name))
            {
                return getSubFile(dir, name);
            }
            return null;
        }

        protected bool locateToDir(string path, out DirectoryInfo dir)
            => (dir = locateToDir(path)) != null;

        protected DirectoryInfo locateToDir(string path)
        {
            if (locateToName(path, out var dir, out var realDir, out var name))
            {
                return getSubDir(dir, name, out var realName);
            }
            return dir;
        }

        protected bool locateToItem(string path, 
            out FileSystemInfo item, out string realPath)
            => (item = locateToItem(path, out realPath)) != null;

        protected FileSystemInfo locateToItem(string path, out string realPath)
        {
            if (locateToName(path, out var dir, out var realDir, out var name))
            {
                var item = getSubItem(dir, name, out var realName);
                realPath = mergePath(realDir, realName);
                return item;
            }
            realPath = realDir;
            return dir;
        }

        string mergePath(string dir, string name)
        {
            if (dir == null)
                return name;
            if (name == null)
                return dir;
            return $"{dir}/{name}";
        }

        protected abstract DirectoryInfo addSubDir(DirectoryInfo dir,
            string name);

        protected abstract string rootPath { get; }

        public virtual string localPath => rootPath;

        DirectoryInfo _rootDir;
        protected DirectoryInfo rootDir
            => _rootDir ?? (_rootDir = new DirectoryInfo(rootPath));

        protected DirectoryInfo locateToParent(string path,
                                    out string name,
                                    out string realDir,
                                    bool create = false)
        {
            var nodes = splitPath(path, out name);
            var dir = rootDir;
            realDir = null;
            for (int i = 0; i < nodes?.Length - 1; i++)
            {
                if (nodes[i].Length == 0)
                    continue;
                var sub = getSubDir(dir, nodes[i], out var realName);
                if (null != sub)
                {
                    dir = sub;
                }
                else if (create)
                {
                    dir = addSubDir(dir, nodes[i]);
                    realName = nodes[i];
                }
                else
                    return null;

                if (realDir == null)
                    realDir = realName;
                else
                    realDir = $"{realDir}/{realName}";
            }
            return dir;
        }

        protected virtual long getFileSize(FileInfo fi)
            => fi.Length;

        protected abstract Stream openFile(string path, bool write);

        public override Stream writeFile(string path)
            => openFile(path, write: true);

        public override Stream readFile(string path)
            => openFile(path, write: false);

        protected FileAccess fileAccess(bool write)
            => write ? FileAccess.ReadWrite : FileAccess.Read;

        protected FileShare fileShare(bool write)
            => (write ? FileShare.Read : FileShare.ReadWrite) | FileShare.Delete;
    }
}
