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
        public LocalDirReposit rep;
        public DirectoryInfo dirInfo;

        public override IEnumerable<DirItem> enumDirs()
        {
            foreach (var sub in dirInfo.EnumerateDirectories())
            {
                if (sub.isSystem() && sub.isHidden())
                    continue;
                if (!getSubPath(sub, out var subPath))
                    continue;
                yield return new LocalDirItem
                {
                    rep = rep,
                    dirInfo = sub,
                    path = subPath,
                };
            }
        }

        bool getSubPath(FileSystemInfo sub, out string subPath)
        {
            subPath = rep.decodeName(sub);
            if (subPath != null && path != null)
                subPath = $"{path}/{subPath}";
            return subPath != null;
        }

        public override IEnumerable<T> enumFiles<T>()
        {
            foreach (var fi in dirInfo.EnumerateFiles())
            {
                if (fi.isHidden() && fi.isSystem())
                    continue;
                if (!getSubPath(fi, out var subPath))
                    continue;
                fi.Refresh();
                yield return rep.newFileNode<T>(fi, subPath);
            }
        }
    }
}
