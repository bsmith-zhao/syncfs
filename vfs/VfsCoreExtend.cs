using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;

namespace vfs
{
    public partial class VfsCore
    {
        int checkTime;
        void markCheck()
            => checkTime = true.ticks();

        const int CheckInterval = 20 * 1000;
        const int ActiveInterval = 1 * 60 * 1000;

        public Set<FileDesc> opens = new Set<FileDesc>();

        void checkActive()
        {
            var now = true.ticks();
            if (now < checkTime + CheckInterval)
                return;
            checkTime = now;

            List<FileDesc> fsl = null;
            opens.pick(f => now > f.activeTime + ActiveInterval)
                .each(f =>
                {
                    fsl = fsl.add(f);
                });

            new
            {
                inactive = fsl?.Count ?? 0,
                thdId = true.thdId(),
                maxPad,
                maxRead,
                maxWrite
            }.debug();

            if (fsl == null)
                return;

            var fs = fsl.conv(f => f.detachFile()).ToArray();

            Task.Run(() =>
            {
                fs.each(f => f.Close());
                new
                {
                    free = fs.Length,
                    thdId = true.thdId()
                }.debug();
            });
        }

        int maxRead;
        void markRead(FileDesc fd, Stream fs,
            long offset, int count)
        {
            maxRead = maxRead.atLeast(count);

            if (count > 4 * Number.MB)
                new
                {
                    f = "read",
                    fd.path,
                    offset,
                    count,
                    fs.Length,
                    fs.Position
                }.debug();
        }

        int maxWrite;
        void markWrite(FileDesc fd, Stream fs,
            long offset, int count,
            bool append, bool coverOnly)
        {
            maxWrite = maxWrite.atLeast(count);

            if (count > 4 * Number.MB)
                new
                {
                    f = "write",
                    fd.path,
                    offset,
                    count,
                    append,
                    coverOnly,
                    fs.Length,
                    fs.Position
                }.debug();
        }

        int maxPad;
        void markPad(FileDesc fd, Stream fs,
            long offset)
        {
            var pad = (int)(offset - fs.Length);
            maxPad = maxPad.atLeast(pad);

            if (pad > 16 * Number.MB)
            {
                new
                {
                    f = "pad",
                    fd.path,
                    offset,
                    fs.Length,
                    fs.Position
                }.debug();
            }
        }
    }
}
