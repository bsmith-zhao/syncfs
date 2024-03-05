using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

using Fsp;
using VolumeInfo = Fsp.Interop.VolumeInfo;
using FileInfo = Fsp.Interop.FileInfo;
using System.Collections;
using util.rep;
using util.ext;
using util;

namespace vfs
{
    public class VfsCore : FileSystemBase
    {
        public Reposit rep;
        public string mount;
        public string label;

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
                volume.SetVolumeLabel(label);

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, "GetVolumeInfo");
                throw;
            }
            /*
            * DriveInfo only supports drives and does not support UNC paths.
            * It would be better to use GetDiskFreeSpaceEx here.
            */
        }

        public override Int32 GetSecurityByName(
            String path,
            out UInt32 attrs/* or ReparsePointIndex */,
            ref Byte[] security)
        {
            try
            {
                attrs = 0;
                var item = rep.getItem(path);
                if (item != null)
                {
                    if (item.isDir)
                        attrs = (uint)FileAttributes.Directory;
                    else
                        attrs = (uint)FileAttributes.Normal;
                }
                security = DefaultSecurity;
                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, "GetSecurityByName");
                throw;
            }
        }

        public override Int32 Create(
            String path,
            UInt32 option,
            UInt32 access,
            UInt32 attrs,
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
                fd = new FileDesc(rep, item, fs);

                node = default(Object);
                desc = fd;
                name = default(String);

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                fd?.closeFile();
                trace(err, "Create");
                throw;
            }
        }
        public override Int32 Open(
            String path,
            UInt32 options,
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

                fd = new FileDesc(rep, item);

                desc = fd;
                node = default(Object);
                name = default(String);
                return fd.getInfo(out info);
            }
            catch(Exception err)
            {
                fd?.closeFile();
                trace(err, "Open");
                throw;
            }
        }

        public override Int32 Overwrite(
            Object node,
            Object desc,
            UInt32 attrs,
            Boolean replace,
            UInt64 alloc,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                fd.openWrite().SetLength(0);
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, "Overwrite");
                throw;
            }
        }

        void trace(Exception err, string func)
            => $"<{func}>{err.Message}".log();

        public override void Cleanup(
            Object node,
            Object desc,
            String path,
            UInt32 flags)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                if (0 != (flags & CleanupDelete))
                {
                    fd.closeFile();
                    if (fd.isDir)
                        rep.deleteDir(fd.item.path);
                    else
                        rep.deleteFile(fd.item.path);
                }
            }
            catch (Exception err)
            {
                trace(err, "Cleanup");
                throw;
            }
        }

        public override void Close(
            Object node,
            Object desc)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                fd.closeFile();
            }
            catch (Exception err)
            {
                trace(err, "Close");
                throw;
            }
        }

        public override Int32 Read(
            Object node,
            Object desc,
            IntPtr buffPtr,
            UInt64 offset,
            UInt32 total,
            out UInt32 finish)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                var fs = fd.openRead();

                if (offset >= (UInt64)fs.Length)
                {
                    finish = 0;
                    return STATUS_END_OF_FILE;
                }

                fs.Position = (long)offset;

                int count = (int)total;
                var buff = getBuff(count);
                finish = (uint)fs.Read(buff, 0, count);
                Marshal.Copy(buff, 0, buffPtr, (int)finish);

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, "Read");
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
            IntPtr buffPtr,
            UInt64 offset,
            UInt32 count,
            Boolean writeToEnd,
            Boolean constrainedIo,
            out UInt32 finish,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;

                var fs = fd.openWrite();

                if (constrainedIo)
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

                if (writeToEnd)
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
                Marshal.Copy(buffPtr, buff, 0, size);
                fs.Write(buff, 0, size);

                finish = count;
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, "Write");
                throw;
            }
        }
        public override Int32 Flush(
            Object node,
            Object desc,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
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
                trace(err, "Flush");
                throw;
            }
        }

        public override Int32 GetFileInfo(
            Object node,
            Object desc,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, "GetFileInfo");
                throw;
            }
        }

        public override Int32 SetBasicInfo(
            Object FileNode,
            Object FileDesc0,
            UInt32 FileAttributes,
            UInt64 CreationTime,
            UInt64 LastAccessTime,
            UInt64 LastWriteTime,
            UInt64 ChangeTime,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)FileDesc0;
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, "SetBasicInfo");
                throw;
            }
        }

        public override Int32 SetFileSize(
            Object node,
            Object desc,
            UInt64 newSize,
            Boolean setAllocSize,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                
                if (!setAllocSize)
                {
                    var fs = fd.openWrite();
                    if (fs.Length > (long)newSize)
                        fs.SetLength((long)newSize);
                }

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, "SetFileSize");
                throw;
            }
        }

        public override Int32 CanDelete(
            Object node,
            Object desc,
            String path)
        {
            return STATUS_SUCCESS;
        }

        public override Int32 Rename(
            Object node,
            Object desc,
            String path,
            String newPath,
            Boolean replaceIfExists)
        {
            try
            {
                var fd = desc as FileDesc;

                var item = rep.getItem(path);
                if (item != null)
                {
                    if (item.isDir)
                        rep.moveDir(path, newPath);
                    else
                    {
                        rep.moveFile(path, newPath);
                    }
                    fd.item = rep.getItem(newPath);
                }

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, "Rename");
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
            String pattern,
            String marker,
            ref Object context,
            out String path,
            out FileInfo info)
        {
            try
            {
                FileDesc fd = (FileDesc)desc;
                if (fd.items == null)
                {
                    var items = new SortedList<string, RepItem>();
                    fd.dir?.enumItems().each(d => items.Add(d.name, d));
                    fd.items = items.Values.ToArray();
                }
                int idx;
                if (null == context)
                {
                    idx = 0;
                    if (null != marker)
                    {
                        idx = Array.BinarySearch(fd.names, marker);
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
                    FileDesc.getItemInfo(fd.items[idx], out info);
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
                trace(err, "ReadDirectory");
                throw;
            }
        }
    }
}
