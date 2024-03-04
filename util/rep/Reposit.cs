using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.rep
{
    public abstract class Reposit : IDir, IDisposable
    {
        public abstract void getSpace(out long total, out long free);

        public abstract bool exist(string path);
        public abstract RepItem getItem(string path);

        public abstract Stream createFile(string path);
        public abstract Stream readFile(string path);
        public abstract Stream writeFile(string path);
        public abstract void deleteFile(string path);
        public abstract void moveFile(string src, string dst);

        public abstract void createDir(string path);
        public abstract void deleteDir(string path);
        public abstract void moveDir(string src, string dst);

        public virtual void close()
        {
        }

        protected string[] splitPath(string path, out string name)
        {
            var nodes = path?.Split('/', '\\');
            name = (nodes?.Length > 0) ? nodes[nodes.Length - 1] : null;
            if (name?.Length == 0)
                name = null;
            return nodes;
        }

        public void Dispose()
        {
            close();
        }
    }
}
