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

namespace vfs
{
    public class FileDesc
    {
        public Reposit rep;
        public RepItem item;
        
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
        }

        public Int32 getInfo(out FileInfo info)
        {
            getItemInfo(item, out info);
            if (data != null)
            {
                info.FileSize = (ulong)data.Length;
                updateAllocSize(ref info);
            }
            return FileSystemBase.STATUS_SUCCESS;
        }

        static void updateAllocSize(ref FileInfo info)
            => info.AllocationSize = (info.FileSize+AllocUnit-1)/AllocUnit * AllocUnit;

        public const int AllocUnit = 4096;

        //public static void ThrowIoExceptionWithHResult(Int32 HResult)
        //{
        //    throw new IOException(null, HResult);
        //}
        //public static void ThrowIoExceptionWithWin32(Int32 Error)
        //{
        //    ThrowIoExceptionWithHResult(unchecked((Int32)(0x80070000 | Error)));
        //}
        //public static void ThrowIoExceptionWithNtStatus(Int32 Status)
        //{
        //    ThrowIoExceptionWithWin32((Int32)FileSystemBase.Win32FromNtStatus(Status));
        //}
    }

}
