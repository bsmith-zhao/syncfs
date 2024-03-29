﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

using FileInfo = Fsp.Interop.FileInfo;
using VolumeInfo = Fsp.Interop.VolumeInfo;

namespace vfs
{
    public partial class VfsCore
    {
        const int BuffSize = (int)(8 * Number.MB);

        void trace(Exception err, object args = null)
        {
            err.log(true.lastFunc(), args);
        }

        void init()
        {
            if (bakEnable)
                true.trylog(() => rep.createDir(vfs.bak));

            beginStreamMonitor();
            beginIOMonitor();
        }

        void delete(FileDesc fd,
            uint flag)
        {
            //new { path, flag }.debug();

            try
            {
                if (flag.markBy(CleanupDelete))
                {
                    //new { mark = "delete", path, flag }.debug();

                    fd.close();
                    var srcPath = fd.path;
                    if (bakEnable
                        && fd.item.isFile()
                        && !vfs.bak.lowEqual(srcPath.pathRoot()))
                    {
                        var bakPath = $"{vfs.bak}/{srcPath}"
                                .pathSettle(rep.exist, "-");
                        rep.moveFile(srcPath, bakPath);
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

        int overwrite(FileDesc fd,
            UInt32 attr,
            Boolean replace,
            UInt64 alloc,
            out FileInfo info)
        {
            try
            {
                fd.openWrite().SetLength(0);
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        int open(string path,
            uint option,
            uint access,
            out object desc,
            out FileInfo info)
        {
            FileDesc fd = null;
            try
            {
                var item = rep.getItem(path);
                if (item == null)
                {
                    desc = null;
                    info = default(FileInfo);
                    return STATUS_OBJECT_NAME_NOT_FOUND;
                }

                fd = new FileDesc(this, rep, item);

                desc = fd;
                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { path });
                throw;
            }
        }

        int create(string path,
            uint option,
            uint access,
            uint attr,
            byte[] security,
            ulong allocSize,
            out object desc,
            out FileInfo info)
        {
            FileDesc fd = null;
            try
            {
                if (rep.exist(path))
                {
                    desc = null;
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

                desc = fd;

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                fd?.close();
                trace(err, new { path, option });
                throw;
            }
        }

        int getSecurity(string path,
            out uint attr, ref byte[] security)
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

        int getVolume(out VolumeInfo vol)
        {
            try
            {
                vol = default(VolumeInfo);

                rep.getSpace(out var total, out var free);
                vol.TotalSize = (UInt64)total;
                vol.FreeSize = (UInt64)free;
                vol.SetVolumeLabel(vfs.name);

                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err);
                throw;
            }
        }

        void close(FileDesc fd)
        {
            try
            {
                fd.close();
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        byte[] buff = new byte[BuffSize];

        int write(FileDesc fd,
            IntPtr ptr, long offset, int count,
            bool append, bool coverOnly,
            out uint finish,
            out FileInfo info)
        {
            try
            {
                var fs = fd.openWrite();

                markWrite(fd, fs, offset, count, append, coverOnly);

                if (coverOnly)
                {
                    if (offset >= fs.Length)
                    {
                        finish = 0;
                        info = default(FileInfo);
                        return STATUS_SUCCESS;
                    }
                    count = count.atMost((int)(fs.Length - offset));
                }

                if (append)
                    fs.Position = fs.Length;
                else
                {
                    if (offset > fs.Length)
                        extend(fd, fs, offset - fs.Length);
                    fs.Position = offset;
                }

                markActualWrite(fd, fs, count);

                writeData(fs, ptr, count);

                finish = (uint)count;
                return fd.getWriteInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, offset, count, coverOnly, append });
                throw;
            }
        }

        void extend(FileDesc fd, Stream fs, long delta)
        {
            markExtend(fd, fs, delta);

            fs.extend(delta);
        }

        void writeData(Stream fs, IntPtr ptr, int total)
        {
            total.slice(BuffSize, unit =>
            {
                Marshal.Copy(ptr, buff, 0, unit);
                ptr += unit;
                fs.Write(buff, 0, unit);
            });
        }

        int read(FileDesc fd,
            IntPtr ptr,
            long offset,
            int count,
            out uint finish)
        {
            try
            {
                var fs = fd.openRead();

                markRead(fd, fs, offset, count);

                if (offset < 0 || offset >= fs.Length)
                {
                    finish = 0;
                    return STATUS_END_OF_FILE;
                }

                fs.Position = offset;

                var actual = readData(fs, ptr, count);

                markActualRead(fd, fs, count, actual);

                finish = (uint)actual;
                return STATUS_SUCCESS;
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, offset, count });
                throw;
            }
        }

        int readData(Stream fs, IntPtr ptr, int total)
        {
            return total.slice(BuffSize, 
                unit => fs.Read(buff, 0, unit),
                actual =>
                {
                    Marshal.Copy(buff, 0, ptr, actual);
                    ptr += actual;
                });
        }

        int flush(
            FileDesc fd,
            out FileInfo info)
        {
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

        public int getInfo(
            FileDesc fd,
            out FileInfo info)
        {
            try
            {
                // new {fd.path, fd.isOpen }.debug();

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path });
                throw;
            }
        }

        public int setInfo(
            FileDesc fd,
            UInt32 attr,
            UInt64 createTime,
            UInt64 accessTime,
            UInt64 writeTime,
            UInt64 changeTime,
            out FileInfo info)
        {
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

        public int setSize(
            FileDesc fd,
            long newSize,
            bool setAlloc,
            out FileInfo info)
        {
            try
            {
                //new { fd.path, newSize, setAlloc }.debug();

                Stream fs = null;
                if (!setAlloc)
                {
                    fs = fd.openWrite();
                    if (fs.Length > newSize)
                    {
                        fs.SetLength(newSize);
                    }
                }
                //else
                //{
                //    fs = fd.openWrite();
                //    fs.SetLength(newSize);
                //}

                //new
                //{
                //    fd.path,
                //    f="setSize",
                //    newSize,
                //    setSize = fs?.Length ?? 0,
                //    setAlloc
                //}.log();

                return fd.getInfo(out info);
            }
            catch (Exception err)
            {
                trace(err, new { fd?.path, newSize, setAlloc });
                throw;
            }
        }

        public Int32 checkDelete(
            FileDesc fd,
            String path)
        {
            //new { path }.debug();

            try
            {
                // when rename a dir to the same name of another dir
                // and user choose merge dir to another dir
                // need to check dir empty
                // if return NOT_EMPTY, 
                // winfsp will move dir sub items to another dir
                // if no check empty, winfsp will simply delete dir
                if (fd.item.asDir(out var dir)
                    && !dir.empty())
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

        public Int32 move(
            FileDesc fd,
            String oldPath,
            String newPath,
            Boolean replace)
        {
            //new { oldPath, newPath, replace }.debug();
            try
            {
                if (!oldPath.lowEqual(newPath))
                {
                    var newItem = rep.getItem(newPath);
                    if (newItem != null)
                        return STATUS_OBJECT_NAME_COLLISION;
                }

                var item = fd.item;
                if (item != null)
                {
                    rep.moveItem(item, newPath);
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

        public Int32 getSecurity(
            FileDesc fd,
            ref Byte[] security)
        {
            security = DefaultSecurity;
            return STATUS_SUCCESS;
        }

        public Int32 setSecurity(
            FileDesc fd,
            AccessControlSections sections,
            byte[] security)
        {
            return STATUS_SUCCESS;
        }

        public bool getDirEntry(
            FileDesc fd,
            string match,
            string marker,
            ref object context,
            out string path,
            out FileInfo info)
        {
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

            if (bakEnable
                && vfs.bak.lowEqual(item.path))
                info.FileAttributes |= (uint)FileAttributes.Compressed;
        }

        const int SectorSize = 4096;

        public void updateAllocSize(ref FileInfo info)
                => info.AllocationSize
            = (info.FileSize + SectorSize - 1) / SectorSize * SectorSize;
    }
}
