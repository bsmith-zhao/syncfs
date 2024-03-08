using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public abstract class DirItem : RepItem
    {
        public abstract IEnumerable<DirItem> enumDirs();
        public abstract IEnumerable<T> enumFiles<T>()
            where T : FileItem, new();

        public virtual IEnumerable<RepItem> enumItems()
            => (enumDirs() as IEnumerable<RepItem>)
            .combine(enumFiles());

        public virtual IEnumerable<DirItem> enumAllDirs()
        {
            var stack = new Stack<IEnumerable<DirItem>>();
            stack.Push(enumDirs());
            while (stack.Count > 0)
            {
                foreach (var dn in stack.Pop())
                {
                    yield return dn;
                    stack.Push(dn.enumDirs());
                }
            }
        }

        public virtual IEnumerable<T> enumAllFiles<T>()
            where T : FileItem, new()
        {
            foreach (var fn in enumFiles<T>())
                yield return fn;
            foreach (var dn in enumAllDirs())
                foreach (var fn in dn.enumFiles<T>())
                    yield return fn;
        }

        public IEnumerable<FileItem> enumFiles()
            => enumFiles<FileItem>();

        public IEnumerable<FileItem> enumAllFiles()
            => enumAllFiles<FileItem>();
    }
}
