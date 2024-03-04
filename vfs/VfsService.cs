﻿using Fsp;
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
        public string mount;
        public string label;

        public VfsService() : base("VfsService")
        {
        }

        FileSystemHost host;

        protected override void OnStart(String[] Args)
        {
            var host = new FileSystemHost(new VfsCore
            {
                rep = rep,
                mount = mount,
                label = label
            });
            if (0 > host.Mount(mount, null, true, 0))
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
