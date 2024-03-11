using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using util;
using util.ext;

namespace vfs
{
    public partial class VfsCore
    {
        void markPad(FileDesc a, Stream b, long c) { }
        void markRead(FileDesc a, Stream b, long c, int d) { }
        void markActualRead(FileDesc a, Stream b, int c, int d) { }
        void markWrite(FileDesc a, Stream b, long c, int d, bool e, bool f) { }
        void markActualWrite(FileDesc fd, Stream fs, int count) { }
        void beginIOMonitor() { }
    }

    //public partial class VfsCore
    //{
    //    int maxRead;
    //    void markRead(FileDesc fd, Stream fs,
    //        long offset, int count)
    //    {
    //    }

    //    void markActualRead(FileDesc fd, Stream fs,
    //        int count, int actual)
    //    {
    //        maxRead = maxRead.atLeast(count);
    //        readSize += actual;
    //        readCount++;
    //    }

    //    int maxWrite;

    //    void markWrite(FileDesc fd, Stream fs,
    //        long offset, int count,
    //        bool append, bool coverOnly)
    //    {
    //    }

    //    public long readSize;
    //    public int readCount;

    //    public int writeCount;
    //    public long writeSize;

    //    void markActualWrite(FileDesc fd, Stream fs, int count)
    //    {
    //        maxWrite = maxWrite.atLeast(count);

    //        writeCount++;
    //        writeSize += count;
    //    }

    //    int maxPad;
    //    void markPad(FileDesc fd, Stream fs,
    //        long offset)
    //    {
    //        maxPad = maxPad.atLeast((int)(offset - fs.Length));
    //    }

    //    void beginIOMonitor()
    //        => true.runByThd(ioMonitor);

    //    void ioMonitor()
    //    {
    //        int begin = true.timeTicks();
    //        while (true)
    //        {
    //            true.sleep(10000);

    //            var now = true.timeTicks();
    //            var span = now - begin;

    //            var readSpeed = readSize * 1000 / span;
    //            var writeSpeed = writeSize * 1000 / span;

    //            new
    //            {
    //                now,
    //                readCount,
    //                readSize = readSize.byteSize(),
    //                readSpeed = readSpeed.byteSize(),
    //                writeCount,
    //                writeSize = writeSize.byteSize(),
    //                writeSpeed = writeSpeed.byteSize(),
    //                maxRead = maxRead.byteSize(),
    //                maxWrite = maxWrite.byteSize(),
    //            }.debug();

    //            begin = now;
    //            readCount = 0;
    //            readSize = 0;
    //            writeCount = 0;
    //            writeSize = 0;
    //        }
    //    }
    //}
}
