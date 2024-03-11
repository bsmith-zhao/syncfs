using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fsp;
using FileInfo = Fsp.Interop.FileInfo;
using util.rep;
using util.ext;
using util;

namespace vfs
{
    public partial class FileDesc
    {
        public VfsCore core;

        public Reposit rep;
        public RepItem item;

        public RepItem[] items;

        Stream data;

        public string path => item.path;
        public DirItem dir => item.asDir();

        public FileDesc(VfsCore core,
            Reposit rep, RepItem item,
            Stream fs = null)
        {
            this.core = core;
            this.rep = rep;
            this.item = item;
            this.data = fs;
            writeMode = this.data != null;

            core.openItem(this);
        }

        bool writeMode = false;
        bool canWrite => writeMode && data != null;
        Stream useFile(bool write)
        {
            return core.useFile(this, () =>
            {
                if (write && !canWrite)
                    true.free(ref data);
                this.writeMode = write;
                if (data != null)
                    return data;
                return data = rep.openFile(path, write);
            });
        }

        public Stream openRead()
            => useFile(write: false);

        public Stream openWrite()
            => useFile(write: true);

        public void flushFile()
            => core.lockdo(() 
                => data?.Flush());

        public void close()
            => core.closeItem(this, () 
                => true.free(ref data));

        public Int32 getInfo(out FileInfo info)
        {
            core.getItemInfo(item, out info);
            if (data != null)
            {
                info.FileSize = (ulong)data.Length;
                core.updateAllocSize(ref info);
            }
            return FileSystemBase.STATUS_SUCCESS;
        }
    }

}
