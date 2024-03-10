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
    public class FileDesc
    {
        public VfsCore core;

        public Reposit rep;
        public RepItem item;

        public RepItem[] items;

        Stream data;
        public string path => item.path;
        public bool isDir => item.isDir();
        public DirItem dir => item.asDir();

        public FileDesc(VfsCore core,
            Reposit rep, RepItem item,
            Stream fs = null)
        {
            this.core = core;
            this.rep = rep;
            this.item = item;
            this.data = fs;
            write = this.data != null;
        }

        bool write = false;
        Stream openFile(bool write)
        {
            closeFile();
            this.write = write;
            return data = rep.openFile(path, write);
        }

        public Stream openRead()
            => data ?? openFile(write: false);

        bool writeMode => data != null && write;

        public Stream openWrite()
            => writeMode ? data : openFile(write: true);

        public void flushFile()
            => data?.Flush();

        public void closeFile()
        {
            this.free(ref data);
        }

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
