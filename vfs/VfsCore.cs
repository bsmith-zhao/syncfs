using Fsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using util;
using util.ext;
using util.rep;
using FileInfo = Fsp.Interop.FileInfo;
using VolumeInfo = Fsp.Interop.VolumeInfo;

namespace vfs
{
    public class VfsCore : FileSystemBase
    {
        public Reposit rep;
        public VfsArgs vfs;

        public VfsCore()
        {
            var sddl = "O:BAG:BAD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;FA;;;WD)";
            var sd = new RawSecurityDescriptor(sddl);
            DefaultSecurity = new Byte[sd.BinaryLength];
            sd.GetBinaryForm(DefaultSecurity, 0);
        }

        byte[] DefaultSecurity;

        public override Int32 Init(Object Host0)
        {
            FileSystemHost Host = (FileSystemHost)Host0;
            Host.SectorSize = 4096;
            Host.SectorsPerAllocationUnit = 1;
            Host.MaxComponentLength = 255;
            Host.FileInfoTimeout = 1000;
            Host.CaseSensitiveSearch = false;
            Host.CasePreservedNames = true;
            Host.UnicodeOnDisk = true;
            Host.PersistentAcls = true;
            Host.PostCleanupWhenModifiedOnly = true;
            Host.PassQueryDirectoryPattern = true;
            Host.FlushAndPurgeOnCleanup = true;
            Host.VolumeCreationTime = 0;
            Host.VolumeSerialNumber = 0;

            if (needBak)
                this.trylog(() => rep.createDir(vfs.bak));

            return STATUS_SUCCESS;
        }

        public override Int32 GetVolumeInfo(
            out VolumeInfo volume)
        {
            try
            {
                volume = default(VolumeInfo);

                rep.getSpace(out var total, out var free);
                volume.TotalSize = (UInt64)total;
                volume.FreeSize = (UInt64)free;
                volume.SetVolumeLabel(vfs.name);

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err);
                throw;
            }
            /*
            * DriveInfo only supports drives and does not support UNC paths.
            * It would be better to use GetDiskFreeSpaceEx here.
            */
        }

        public override Int32 GetSecurityByName(
            String path,
            out UInt32 attr/* or ReparsePointIndex */,
            ref Byte[] security)
        {
            try
            {
                attr = 0;
                var item = rep.getItem(path);
                if (item != null)
                {
                    if (item.isDir())
                        attr = (uint)FileAttributes.Directory;
                    else
                        attr = (uint)FileAttributes.Normal;
                }
                security = DefaultSecurity;
                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, new { path });
                throw;
            }
        }

        public override Int32 Create(
            String path,
            UInt32 option,
            UInt32 access,
            UInt32 attr,
            Byte[] security,
            UInt64 allocSize,
            out Object node,
            out Object desc,
            out FileInfo info,
            out String name)
        {
            FileDesc fd = null;
            try
            {
                if (rep.exist(path))
                {
                    node = default(Object);
                    desc = null;
                    name = default(String);
                    info = default(FileInfo);
                    return STATUS_OBJECT_NAME_COLLISION;
                }

                Stream fs = null;
                if (0 == (option & FILE_DIRECTORY_FILE))
                    fs = rep.createFile(path);
                else
                    rep.createDir(path);

                var item = rep.getItem(path);
                fd = new FileDesc(this, rep, item, fs);

                node = default(Object);
                desc = fd;
                name = default(String);

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                fd?.closeFile();
                trace(err, new { path, option});
                throw;
            }
        }
        public override Int32 Open(
            String path,
            UInt32 option,
            UInt32 access,
            out Object node,
            out Object desc,
            out FileInfo info,
            out String name)
        {
            FileDesc fd = null;
            try
            {
                var item = rep.getItem(path);
                if (item == null)
                {
                    desc = null;
                    node = default(Object);
                    name = default(String);
                    info = default(FileInfo);
                    return STATUS_OBJECT_NAME_NOT_FOUND;
                }

                fd = new FileDesc(this, rep, item);

                desc = fd;
                node = default(Object);
                name = default(String);
                return fd.getInfo(out info);
            }
            catch(Exception err)
            {
                fd?.closeFile();
                trace(err, new {path});
                throw;
            }
        }

        public override Int32 Overwrite(
            Object node,
            Object desc,
            UInt32 attr,
            Boolean replace,
            UInt64 alloc,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                fd.openWrite().SetLength(0);
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path});
                throw;
            }
        }

        void trace(Exception err, object args = null)
        {
            err.log(true.lastFunc(), args);
        }

        public bool needBak => vfs.bak != null;

        public override void Cleanup(
            Object node,
            Object desc,
            String path,
            UInt32 flag)
        {
            var fd = desc as FileDesc;
            try
            {
                if (0 != (flag & CleanupDelete))
                {
                    fd.closeFile();
                    var srcPath = fd.path;
                    if (needBak && vfs.bak.low() != srcPath.pathRoot().low())
                    {
                        var bakPath = $"{vfs.bak}/{srcPath}";
                        if (fd.item is DirItem dir 
                            && rep.getItem(bakPath).isDir())
                        {
                            // ignore exist empty dir
                            rep.deleteDir(dir.path);
                        }
                        else
                        {
                            bakPath = bakPath.pathSettle(rep.exist, "-");
                            rep.moveItem(fd.item, bakPath);
                        }
                    }
                    else
                    {
                        rep.deleteItem(fd.item);
                    }
                }
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        public override void Close(
            Object node,
            Object desc)
        {
            var fd = desc as FileDesc;
            try
            {
                fd.closeFile();
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        public override Int32 Read(
            Object node,
            Object desc,
            IntPtr ptr,
            UInt64 offset,
            UInt32 count,
            out UInt32 finish)
        {
            var fd = desc as FileDesc;
            try
            {
                var fs = fd.openRead();

                if (offset >= (UInt64)fs.Length)
                {
                    finish = 0;
                    return STATUS_END_OF_FILE;
                }

                fs.Position = (long)offset;

                int size = (int)count;
                var buff = getBuff(size);
                finish = (uint)fs.Read(buff, 0, size);
                Marshal.Copy(buff, 0, ptr, (int)finish);

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, offset, count});
                throw;
            }
        }

        byte[] _buff;
        byte[] getBuff(int size)
        {
            if (_buff == null || _buff.Length < size)
                _buff = new byte[size];
            return _buff;
        }

        public override Int32 Write(
            Object node,
            Object desc,
            IntPtr ptr,
            UInt64 offset,
            UInt32 count,
            Boolean append,
            Boolean cover,
            out UInt32 finish,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                var fs = fd.openWrite();

                if (cover)
                {
                    if (offset >= (UInt64)fs.Length)
                    {
                        finish = default(UInt32);
                        info = default(FileInfo);
                        return STATUS_SUCCESS;
                    }
                    if (offset + count > (UInt64)fs.Length)
                        count = (UInt32)((UInt64)fs.Length - offset);
                }

                if (append)
                    fs.Position = fs.Length;
                else
                {
                    long off = (long)offset;
                    if (off > fs.Length)
                    {
                        fs.Position = fs.Length;
                        byte[] pad = new byte[off-fs.Length];
                        fs.write(pad);
                    }
                    fs.Position = off;
                }

                int size = (int)count;
                byte[] buff = getBuff(size);
                Marshal.Copy(ptr, buff, 0, size);
                fs.Write(buff, 0, size);

                finish = count;
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, offset, count, cover, append});
                throw;
            }
        }
        public override Int32 Flush(
            Object node,
            Object desc,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                if (null == fd)
                {
                    /* we do not flush the whole volume, so just return SUCCESS */
                    info = default(FileInfo);
                    return STATUS_SUCCESS;
                }
                fd.flushFile();
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        public override Int32 GetFileInfo(
            Object node,
            Object desc,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        public override Int32 SetBasicInfo(
            Object node,
            Object desc,
            UInt32 attr,
            UInt64 createTime,
            UInt64 accessTime,
            UInt64 writeTime,
            UInt64 changeTime,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        public override Int32 SetFileSize(
            Object node,
            Object desc,
            UInt64 newSize,
            Boolean setAlloc,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                if (!setAlloc)
                {
                    var fs = fd.openWrite();
                    if (fs.Length > (long)newSize)
                        fs.SetLength((long)newSize);
                }

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, newSize, setAlloc});
                throw;
            }
        }

        //public override int SetDelete(
        //    object node, 
        //    object desc,
        //    string path, 
        //    bool isFile)
        //{
        //    new {path, isFile }.debug();
        //    return base.SetDelete(node, desc, path, isFile);
        //}

        public override Int32 CanDelete(
            Object node,
            Object desc,
            String path)
        {
            var fd = desc as FileDesc;
            try
            {
                if (fd.item is DirItem dir
                    && dir.enumItems().exist(it => true))
                {
                    return STATUS_DIRECTORY_NOT_EMPTY;
                }
                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, new { path });
                throw;
            }
        }

        public override Int32 Rename(
            Object node,
            Object desc,
            String oldPath,
            String newPath,
            Boolean replace)
        {
            var fd = desc as FileDesc;
            try
            {
                if (oldPath.low() != newPath.low())
                {
                    var newItem = rep.getItem(newPath);
                    if (newItem != null)
                        return STATUS_OBJECT_NAME_COLLISION;
                }

                var item = fd.item;
                if (item != null)
                {
                    rep.moveItem(item, newPath);
                    //if (item.isDir)
                    //    rep.moveDir(oldPath, newPath);
                    //else
                    //{
                    //    rep.moveFile(oldPath, newPath);
                    //}
                    fd.item = rep.getItem(newPath);
                }

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, oldPath, newPath, replace });
                throw;
            }
        }

        public override Int32 GetSecurity(
            Object FileNode,
            Object FileDesc0,
            ref Byte[] security)
        {
            security = DefaultSecurity;
            return STATUS_SUCCESS;
        }

        public override Int32 SetSecurity(
            Object FileNode,
            Object FileDesc0,
            AccessControlSections Sections,
            Byte[] SecurityDescriptor)
        {
            return STATUS_SUCCESS;
        }

        public override Boolean ReadDirectoryEntry(
            Object node,
            Object desc,
            String match,
            String marker,
            ref Object context,
            out String path,
            out FileInfo info)
        {
            var fd = desc as FileDesc;
            try
            {
                if (fd.items == null)
                {
                    fd.items = fd.dir?.enumItems().ToArray();
                }
                int idx;
                if (null == context)
                {
                    idx = 0;
                    if (null != marker)
                    {
                        idx = fd.items.index(n => n.name == marker);
                        if (idx >= 0)
                            idx++;
                        else
                            idx = ~idx;
                    }
                }
                else
                    idx = (int)context;
                if (fd.items.Length > idx)
                {
                    context = idx + 1;
                    path = fd.items[idx].name;
                    getItemInfo(fd.items[idx], out info);
                    return true;
                }
                else
                {
                    path = default(String);
                    info = default(FileInfo);
                    return false;
                }
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, match, marker, context });
                throw;
            }

            //var fd = desc as FileDesc;
            //try
            //{
            //    if (fd.items == null)
            //    {
            //        var items = new SortedList<string, RepItem>();
            //        fd.dir?.enumItems().each(d => items.Add(d.name, d));
            //        fd.items = items.Values.ToArray();
            //    }
            //    int idx;
            //    if (null == context)
            //    {
            //        idx = 0;
            //        if (null != marker)
            //        {
            //            idx = Array.BinarySearch(fd.names, marker);
            //            if (idx >= 0)
            //                idx++;
            //            else
            //                idx = ~idx;
            //        }
            //    }
            //    else
            //        idx = (int)context;
            //    if (fd.items.Length > idx)
            //    {
            //        context = idx + 1;
            //        path = fd.items[idx].name;
            //        FileDesc.getItemInfo(fd.items[idx], out info);
            //        return true;
            //    }
            //    else
            //    {
            //        path = default(String);
            //        info = default(FileInfo);
            //        return false;
            //    }
            //}
            //catch (Exception err)
            //{
            //    trace(err, new { fd?.path, match, marker, context });
            //    throw;
            //}
        }

        public void getItemInfo(
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

            if (needBak
                && vfs.bak.lowEqual(item.path))
                info.FileAttributes |= (uint)FileAttributes.Encrypted;
        }

        const int AllocUnit = 4096;

        public void updateAllocSize(ref FileInfo info)
                => info.AllocationSize = (info.FileSize + AllocUnit - 1) / AllocUnit * AllocUnit;
    }
}
