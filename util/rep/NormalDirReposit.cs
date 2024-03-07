using util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public class NormalDirReposit : LocalDirReposit
    {
        public const string Type = "NormalDir";

        public override string ToString()
            => $"{Type}@{repPath}";

        public NormalDirReposit(string path)
        {
            repPath = path.pathUnify();
            pathSkip = repPath.Length + 1;
        }

        protected override string rootPath => $"{repPath}/";

        string repPath;
        int pathSkip;

        public override bool exist(string path)
        {
            path = toFullPath(path);
            return File.Exists(path) 
                || Directory.Exists(path);
        }

        public override void createDir(string path)
        {
            Directory.CreateDirectory(toFullPath(path));
        }

        public override void moveDir(string src, string dst)
        {
            var srcLoc = toFullPath(src);
            if (src.ToLower() == dst.ToLower())
            {
                if (src.pathName() == dst.pathName())
                    return;
                srcLoc = relocDir(srcLoc);
            }
            var dstLoc = toFullPath(dst);
            Directory.CreateDirectory(dstLoc.pathDir());
            Directory.Move(srcLoc, dstLoc);
        }

        string relocDir(string path)
        {
            int idx = 0;
            while (idx++ < 100)
            {
                var newPath = $"{path}{idx}";
                if (Directory.Exists(newPath) == false
                    && File.Exists(newPath) == false)
                {
                    Directory.Move(path, newPath);
                    return newPath;
                }
            }
            throw new Error(this, "RelocateOverflow", idx);
        }

        public override void deleteDir(string path)
        {
            path = toFullPath(path);
            if (Directory.Exists(path) == false)
                throw new Error(this, "DirNotExist", path);
            Directory.Delete(path, true);
        }

        public override void moveFile(string src, string dst)
        {
            var dstLoc = toFullPath(dst);
            Directory.CreateDirectory(dstLoc.pathDir());
            File.Move(toFullPath(src), dstLoc);
        }

        public override void deleteFile(string path)
        {
            path = toFullPath(path);
            if (File.Exists(path))
                File.Delete(path);
            else
                throw new Error(this, "FileNotExist", path);
        }

        public override Stream createFile(string path)
        {
            path = toFullPath(path);
            Directory.CreateDirectory(path.pathDir());
            return new FileStream(path,
                FileMode.CreateNew,
                fileAccess(write : true),
                fileShare(write : true));
        }

        protected override Stream openFile(string path, bool write)
            => new FileStream(toFullPath(path),
                            FileMode.Open,
                            fileAccess(write),
                            fileShare(write));

        protected override DirectoryInfo addSubDir(DirectoryInfo dir, 
                                                    string name) 
            => dir.CreateSubdirectory(name);

        public override string parsePath(FileSystemInfo fi) 
            => fi?.FullName.TrimEnd('\\', '/').jump(pathSkip)
            ?.Replace("\\", "/");

        string toFullPath(string path) 
            => $"{repPath}/{path}";

        //public override string parseName(string name, 
        //    FileSystemInfo item, string dir) 
        //    => name;
    }
}
