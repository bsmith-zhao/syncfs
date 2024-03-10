using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace vfs
{
    public partial class FileDesc
    {
        public int activeTime;
        public bool isOpen => data != null;

        public void detachFile(out Stream fs)
        {
            fs = this.data;
            this.data = null;
        }
    }

    public partial class VfsCore
    {
        Set<FileDesc> opens = new Set<FileDesc>();

        public void lockdo(Action func)
        {
            lock (this)
            {
                func();
            }
        }

        public void openItem(FileDesc fd)
        {
            if (fd.isOpen)
            {
                lock (this)
                {
                    opens.Add(fd);
                }
            }
        }

        public Stream openFile(FileDesc fd,
            Func<Stream> func)
        {
            lock (this)
            {
                if (!fd.isOpen)
                    opens.Add(fd);
                fd.activeTime = true.timeTicks();
                return func();
            }
        }

        public void closeItem(FileDesc fd, Action func)
        {
            lock (this)
            {
                if (fd.isOpen)
                    opens.Remove(fd);
                func();
            }
        }

        Thread monThd;

        public void beginMonitor()
        {
            if (monThd != null)
                return;
            monThd = new Thread(closeFreeFiles);
            monThd.Start();
        }

        const int CheckInterval = 20 * 1000;
        const int ActiveInterval = 1 * 60 * 1000;

        void closeFreeFiles()
        {
            try
            {
                while (true)
                {
                    true.sleep(CheckInterval);

                    var now = true.timeTicks();
                    Stream[] frees;
                    int active = 0;
                    lock (this)
                    {
                        var idles = opens.pick(f
                            => now > f.activeTime + ActiveInterval)
                            .newList();

                        frees = idles.conv(f =>
                        {
                            opens.Remove(f);
                            f.detachFile(out var fs);
                            return fs;
                        }).ToArray();

                        active = opens.Count;
                    }

                    //new
                    //{
                    //    now,
                    //    free = frees.Length,
                    //    active,
                    //    maxRead,
                    //    maxWrite,
                    //    maxPad
                    //}.debug();

                    frees.each(f => f?.Close());
                }
            }
            catch (Exception err)
            {
                trace(err);
            }
        }

        int maxRead;
        void markRead(FileDesc fd, Stream fs,
            long offset, int count)
        {
            maxRead = maxRead.atLeast(count);
        }

        int maxWrite;
        void markWrite(FileDesc fd, Stream fs,
            long offset, int count,
            bool append, bool coverOnly)
        {
            maxWrite = maxWrite.atLeast(count);
        }

        int maxPad;
        void markPad(FileDesc fd, Stream fs,
            long offset)
        {
            maxPad = maxPad.atLeast((int)(offset - fs.Length));
        }
    }
}
