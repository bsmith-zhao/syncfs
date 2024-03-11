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

        public Stream detachFile()
            => true.swap(ref data, null);
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

        public Stream useFile(FileDesc fd,
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

        public void beginStreamMonitor()
            => true.runByThd(streamMonitor);

        const int CheckInterval = 20 * 1000;
        const int ActiveInterval = 1 * 60 * 1000;

        void streamMonitor()
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
                        frees = opens.pick(f 
                            => now > f.activeTime + ActiveInterval)
                            .newList().each(f => opens.Remove(f))
                            .conv(f => f.detachFile()).ToArray();

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
    }
}
