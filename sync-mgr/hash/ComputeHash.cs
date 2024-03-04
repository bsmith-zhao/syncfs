using sync.work;
using sync.sync;
using util;
using System;
using util.ext;

namespace sync.hash
{
    public class ComputeHash : Agent
    {
        public ComputeHash() { }
        public ComputeHash(ComputeHashLogic.Param args) 
            => this.args = args;

        public ComputeHashLogic.Param args;
        public ComputeHashLogic lgc;
        public override void work()
        {
            lgc = new ComputeHashLogic(args);

            lgc.BeginCompute += Task_BeginCompute;
            lgc.SkipComputeFile += Task_SkipComputeFile;
            lgc.BeginComputeFile += Task_BeginComputeFile;
            lgc.ComputeFileUpdate += Task_ComputeFileUpdate;
            lgc.EndComputeFile += Task_EndComputeFile;
            lgc.EndCompute += Task_EndCompute;

            run(lgc, args, before:()=> 
            {
                showResult("source", $"{args.src}");
            }, after:() => 
            {
                showResult("total", lgc.files.Count,
                            "update", lgc.updateCount,
                            "reduce", lgc.reduceCount);
            });
        }

        string action = null;
        Counter actTimer;
        private void Task_BeginComputeFile(HashItem unit)
        {
            action = $"<compute>{unit.path}";
            log(action);

            actTimer = new Counter { TotalSize = unit.size };
            hashTimer.update();
        }

        private void Task_ComputeFileUpdate(long delta)
        {
            actTimer.addSize(delta);
            hashTimer.update();
        }

        private void Task_EndComputeFile(HashItem unit)
        {
            actTimer = null;
            action = null;
            hashTimer.addCount();
        }

        private void Task_SkipComputeFile(HashItem unit)
        {
            hashTimer.addCount();
        }

        Counter hashTimer;
        private void Task_BeginCompute(int count)
        {
            hashTimer = new Counter
            {
                TotalCount = count,
            };
            hashTimer.TimeIsUp += HashTimer_timeIsUp;
            hashTimer.trigger();
        }

        private void Task_EndCompute()
        {
            action = null;
            hashTimer.trigger();
        }

        private void HashTimer_timeIsUp(Counter t)
        {
            var status = t.CountInfo;
            if (actTimer != null)
                status = $"{actTimer.SizePair}, {actTimer.SizePercent} - {status}";

            updateStatus(action, status);
        }
    }
}
