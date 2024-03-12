using Fsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.rep;

namespace vfs
{
    public class VfsService : Service
    {
        public Reposit rep;
        public VfsArgs vfs;

        public VfsService() : base("VfsService")
        {
        }

        FileSystemHost host;

        protected override void OnStart(String[] Args)
        {
            var host = new FileSystemHost(new VfsCore
            {
                rep = rep,
                vfs = vfs,
            });
            if (0 > host.Mount(vfs.path, Synchronized: true))
                throw new IOException("cannot mount file system");
            this.host = host;
        }

        protected override void OnStop()
        {
            host.Unmount();
            host = null;
        }
    }
}
