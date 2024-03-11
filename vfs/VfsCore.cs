using Fsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;
using FileInfo = Fsp.Interop.FileInfo;
using VolumeInfo = Fsp.Interop.VolumeInfo;

namespace vfs
{
    public partial class VfsCore : FileSystemBase
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

        public override Int32 Init(Object args)
        {
            var host = args as FileSystemHost;
            host.SectorSize = SectorSize;
            host.SectorsPerAllocationUnit = 1;
            host.MaxComponentLength = 255;
            host.FileInfoTimeout = 2000;
            host.CaseSensitiveSearch = false;
            host.CasePreservedNames = true;
            host.UnicodeOnDisk = true;
            host.PersistentAcls = false;
            host.PostCleanupWhenModifiedOnly = true;
            host.PassQueryDirectoryPattern = true;
            host.FlushAndPurgeOnCleanup = true;
            host.VolumeCreationTime = 0;
            host.VolumeSerialNumber = 0;

            init();

            return STATUS_SUCCESS;
        }

        public override Int32 GetVolumeInfo(
            out VolumeInfo vol)
        {
            return getVolume(out vol);
        }

        public override Int32 GetSecurityByName(
            String path,
            out UInt32 attr/* or ReparsePointIndex */,
            ref Byte[] security)
        {
            return getSecurity(path, 
                out attr, ref security);
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
            node = null;
            name = null;

            return create(path, 
                option, access, attr, 
                security, allocSize, 
                out desc, out info);
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
            node = null;
            name = null;

            return open(path, 
                option, access,
                out desc, out info);
        }

        public override Int32 Overwrite(
            Object node,
            Object desc,
            UInt32 attr,
            Boolean replace,
            UInt64 alloc,
            out FileInfo info)
        {
            return overwrite(desc as FileDesc, 
                attr, replace, alloc, 
                out info);
        }

        public bool bakEnable => vfs.bak != null;

        public override void Cleanup(
            Object node,
            Object desc,
            String path,
            UInt32 flag)
        {
            delete(desc as FileDesc, flag);
        }

        public override void Close(
            Object node,
            Object desc)
        {
            close(desc as FileDesc);
        }

        public override Int32 Read(
            Object node,
            Object desc,
            IntPtr ptr,
            UInt64 offset,
            UInt32 count,
            out UInt32 finish)
        {
            return read(desc as FileDesc,
                ptr, (long)offset, (int)count,
                out finish);
        }

        public override Int32 Write(
            Object node,
            Object desc,
            IntPtr ptr,
            UInt64 offset,
            UInt32 count,
            Boolean append,
            Boolean coverOnly,
            out UInt32 finish,
            out FileInfo info)
        {
            return write(desc as FileDesc,
                ptr, (long)offset, (int)count,
                append, coverOnly, out finish, out info);
        }

        public override Int32 Flush(
            Object node,
            Object desc,
            out FileInfo info)
        {
            return flush(desc as FileDesc, out info);
        }

        public override Int32 GetFileInfo(
            Object node,
            Object desc,
            out FileInfo info)
        {
            return getInfo(desc as FileDesc, out info);
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
            return setInfo(desc as FileDesc, 
                attr, 
                createTime, accessTime, 
                writeTime, changeTime, 
                out info);
        }

        public override Int32 SetFileSize(
            Object node,
            Object desc,
            UInt64 newSize,
            Boolean setAlloc,
            out FileInfo info)
        {
            return setSize(desc as FileDesc, 
                (long)newSize, setAlloc, 
                out info);
        }

        public override Int32 CanDelete(
            Object node,
            Object desc,
            String path)
        {
            return checkDelete(desc as FileDesc, path);
        }

        public override Int32 Rename(
            Object node,
            Object desc,
            String oldPath,
            String newPath,
            Boolean replace)
        {
            return move(desc as FileDesc, 
                oldPath, newPath, 
                replace);
        }

        public override Int32 GetSecurity(
            Object node,
            Object desc,
            ref Byte[] security)
        {
            security = DefaultSecurity;
            return STATUS_SUCCESS;
        }

        public override Int32 SetSecurity(
            Object node,
            Object desc,
            AccessControlSections sections,
            Byte[] security)
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
            return getDirEntry(desc as FileDesc, 
                match, marker, 
                ref context, 
                out path, out info);
        }
    }
}
