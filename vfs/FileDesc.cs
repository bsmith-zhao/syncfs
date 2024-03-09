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

            if (write)
            {
                markActive();
                markOpen();
            }
        }

        public int activeTime;
        void markActive()
            => activeTime = true.ticks();
        void markOpen()
            => core.opens.Add(this);
        void markClose()
            => core.opens.Remove(this);

        bool write = false;
        public Stream openRead()
        {
            markActive();

            if (data == null)
            {
                data = rep.readFile(path);
                write = false;

                markOpen();
            }
            return data;
        }

        public Stream openWrite()
        {
            markActive();

            if (data == null || !write)
            {
                closeFile();
                data = rep.writeFile(path);
                write = true;

                markOpen();
            }
            return data;
        }

        public Stream detachFile()
        {
            markClose();

            var fs = this.data;
            this.data = null;
            return fs;
        }

        public void flushFile()
            => data?.Flush();

        public void closeFile()
        {
            if (data != null)
                markClose();

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
