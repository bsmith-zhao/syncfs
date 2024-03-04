using util;
using sync.hash;
using sync.sync;
using sync.work;
using System.Collections.Generic;

namespace sync.sync
{
    public class FileTransfer : Agent
    {
        public FileTransfer(FileTransferLogic.Param args) 
            => this.args = args;

        public FileTransferLogic lgc;
        public FileTransferLogic.Param args;

        public override void work()
        {
            lgc = new FileTransferLogic(args);

            lgc.BeginTransfer += Task_BeginTransfer;
            lgc.EndTransfer += Task_EndTransfer;
            lgc.BeginDeleteFile += Task_BeginDeleteFile;
            lgc.EndDeleteFile += Task_EndDeleteFile;
            lgc.BeginMoveFile += Task_BeginMoveFile;
            lgc.EndMoveFile += Task_EndMoveFile;
            lgc.BeginCopyFile += Task_BeginCopyFile;
            lgc.CopyFileUpdate += Task_CopyFileUpdate;
            lgc.EndCopyFile += Task_EndCopyFile;

            run(lgc, args, before:()=> 
            {
                showResult("source", $"{args.src}",
                            "target", $"{args.dst}");
            }, after: () =>
            {
                showResult("add", args.adds.Count,
                            "delete", args.dels.Count,
                            "move", args.moves.Count);
            });
        }

        Counter copyTimer;
        string action;
        private void Task_BeginCopyFile(HashItem src)
        {
            action = $"<copy>{src.path}";
            log(action);

            copyTimer = new Counter
            {
                TotalSize = src.size
            };
            transTimer.update();
        }

        private void Task_CopyFileUpdate(long delta)
        {
            copyTimer.addSize(delta);
            transTimer.addSize(delta);
        }

        private void Task_EndCopyFile()
        {
            copyTimer = null;
            transTimer.addCount();
        }

        private void Task_BeginMoveFile(HashItem old, string @new)
        {
            action = $"<move>{old.path} -> {@new}";
            log(action);

            transTimer.update();
        }

        private void Task_EndMoveFile()
        {
            transTimer.addCount();
        }

        private void Task_BeginDeleteFile(HashItem dst)
        {
            action = $"<delete>{dst.path}";
            log(action);

            transTimer.update();
        }

        private void Task_EndDeleteFile()
        {
            transTimer.addCount();
        }

        Counter transTimer;
        private void Task_BeginTransfer(int count, long size)
        {
            transTimer = new Counter
            { TotalCount = count, TotalSize = size };
            transTimer.TimeIsUp += TransTimer_TimeIsUp;
            transTimer.trigger();
        }

        private void Task_EndTransfer()
        {
            action = null;
            transTimer.trigger();
        }

        private void TransTimer_TimeIsUp(Counter t)
        {
            var status = $"{t.AvgSpeed}, {t.SizePair}, {t.CountPair}, {t.SizePercent}";
            if (copyTimer != null)
                status = $"{copyTimer.SpeedInfo} - {status}";
            updateStatus(action, status);
        }
    }
}
