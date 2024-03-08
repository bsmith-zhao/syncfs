using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public class SyncReposit : IDir, IDisposable, IDesc
    {
        public Reposit src;

        public string match;
        public string @lock;
        public Backup backup;

        public bool locked = false;
        public string lockPath;

        Reposit writer => !locked ? src 
            : throw new Error(this, "Locked", lockPath);

        public SyncReposit open()
        {
            getMatch();

            lockPath = getLock();
            locked = lockPath != null;

            return this;
        }

        public void createDir(string path)
            => writer.createDir(path);

        public Stream createFile(string path)
            => writer.createFile(path);

        public void deleteDir(string path)
        {
            if (backup?.enable == true
                && getItem(path) is DirItem dn)
            {
                foreach(var f in dn.enumAllFiles())
                    writer.deleteFile(f.path);
            }
            writer.deleteDir(path);
        }

        public void deleteFile(string path)
        {
            if (backup?.enable == true)
            {
                var bakPath = $"{backup.dir}/{path}";
                if (backup.all)
                    bakPath = bakPath.pathAppend(writer.exist, "-r");
                else
                    bakPath = bakPath.pathBackup(backup.rev,
                        writer.exist, writer.deleteFile, "-r");

                writer.moveFile(path, bakPath);
            }
            else
                writer.deleteFile(path);
        }

        public void moveDir(string src, string dst)
            => writer.moveDir(src, dst);

        public void moveFile(string src, string dst)
            => writer.moveFile(src, dst);

        public Stream writeFile(string path)
            => writer.writeFile(path);

        public Stream readFile(string path)
            => src.readFile(path);

        public bool exist(string path)
            => src.exist(path);

        public RepItem getItem(string path)
            => src.getItem(path);

        public void Dispose()
        {
            src.Dispose();
        }

        public override string ToString()
            => src.ToString();

        string getMatch()
        {
            if (match.empty())
                return null;

            if (getLocalPath(out var localPath))
            {
                var path = $"{localPath}/{match}";
                if (File.Exists(path) || Directory.Exists(path))
                    return path.pathUnify();
            }

            return src.getItem(match)?.path
                ?? throw new Error(this, "Mismatch", match);
        }

        bool getLocalPath(out string localPath)
            => (localPath = (src as ILocalDirReposit)?.localPath) != null;

        string getLock()
        {
            if (@lock.empty())
                return null;

            if (getLocalPath(out var localPath))
            {
                var baseDir = new DirectoryInfo(localPath);
                while (baseDir != null)
                {
                    var path = $"{baseDir.FullName}\\{@lock}";
                    if (Directory.Exists(path))
                        return path.pathUnify();
                    baseDir = baseDir.Parent;
                }
            }

            if (src.getItem(@lock, out var item))
                return item.path;

            var lowLock = @lock.low();
            foreach (var p in src.enumAllDirs())
            {
                if (p.pathName().low() == lowLock)
                    return p;
            }

            return null;
        }

        public string getDesc()
            => new
            {
                src,
                bak = backup,
                @lock = lockPath,
                locked,
            }.desc<SyncReposit>();
    }
}
