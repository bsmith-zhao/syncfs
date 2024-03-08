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
        public Reposit rep;
        public RepItem item;
        public VfsCore core;
        
        public RepItem[] items;
        string[] _names;
        public string[] names => _names 
            ?? (_names = items.conv(n => n.name));

        Stream data;
        public string path => item.path;
        public bool isDir => dir != null;
        public DirItem dir => item as DirItem;

        public FileDesc(Reposit rep, RepItem item, Stream fs = null)
        {
            this.rep = rep;
            this.item = item;
            this.data = fs;
            write = this.data != null;
        }

        bool write = false;
        public Stream openRead()
        {
            if (data == null)
            {
                data = rep.readFile(path);
                write = false;
            }
            return data;
        }

        public Stream openWrite()
        {
            if(data == null || !write)
            {
                closeFile();
                data = rep.writeFile(path);
                write = true;
            }
            return data;
        }

        public void flushFile()
            => data?.Flush();

        public void closeFile()
            => this.free(ref data);

        public static void getItemInfo(
            RepItem item,
            VfsCore core,
            out FileInfo info)
        {
            info = new FileInfo();
            info.FileAttributes = (uint)(item.isDir() ? 
                FileAttributes.Directory
                : FileAttributes.Normal);
            info.ReparseTag = 0;
            info.FileSize = (ulong)item.size;
            updateAllocSize(ref info);
            info.CreationTime = (ulong)item.createTime;
            info.LastAccessTime = (ulong)item.modifyTime;
            info.LastWriteTime = (ulong)item.modifyTime;
            info.ChangeTime = info.LastWriteTime;
            info.IndexNumber = 0;
            info.HardLinks = 0;

            if (core.needBak
                && core.vfs.bak.lowEqual(item.path))
                info.FileAttributes |= (uint)FileAttributes.Encrypted;
        }

        public Int32 getInfo(out FileInfo info)
        {
            getItemInfo(item, core, out info);
            if (data != null)
            {
                info.FileSize = (ulong)data.Length;
                updateAllocSize(ref info);
            }
            return FileSystemBase.STATUS_SUCCESS;
        }

        public const int AllocUnit = 4096;

        static void updateAllocSize(ref FileInfo info)
            => info.AllocationSize = (info.FileSize+AllocUnit-1)/AllocUnit * AllocUnit;
    }

}
