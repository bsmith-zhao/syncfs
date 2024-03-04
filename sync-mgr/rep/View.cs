using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.rep
{
    public class View : IDir, IDisposable, IDesc
    {
        public IDir src;
        public SyncReposit rep;

        public string root;
        public Filter flt;

        public View open(bool unlock)
        {
            if (unlock)
                rep.locked = false;
            return this;
        }

        public string toBasePath(string path)
            => path == null ? root 
            : (root == null ? path : $"{root}/{path}");

        public string toViewPath(string path)
            => root == null ? path
            : path.jump(root.Length + 1);

        public virtual void createDir(string path)
            => src.createDir(toBasePath(path));

        public virtual Stream createFile(string path)
            => src.createFile(toBasePath(path));

        public virtual void deleteDir(string path)
            => src.deleteDir(toBasePath(path));

        public virtual void deleteFile(string path)
            => src.deleteFile(toBasePath(path));

        public virtual bool exist(string path)
            => src.exist(toBasePath(path));

        public virtual RepItem getItem(string path)
        {
            var nd = src.getItem(toBasePath(path));
            if (nd is DirItem dn)
            {
                return new ViewDirNode
                {
                    view = this,
                    src = dn,
                    path = toViewPath(dn.path),
                };
            }
            return nd;
        }

        public virtual void moveDir(string src, string dst)
            => this.src.moveDir(toBasePath(src), toBasePath(dst));

        public virtual void moveFile(string src, string dst)
            => this.src.moveFile(toBasePath(src), toBasePath(dst));

        public virtual Stream readFile(string path)
            => src.readFile(toBasePath(path));

        public virtual Stream writeFile(string path)
            => src.writeFile(toBasePath(path));

        public void Dispose()
        {
            rep.Dispose();
        }

        public override string ToString()
            => root == null ? $"{src}" : $"{src}/<{root}>";

        public string getDesc()
            => new
            {
                src,
                root,
                flt
            }.desc<View>();
    }
}
