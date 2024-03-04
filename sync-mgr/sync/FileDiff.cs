using System.Collections.Generic;
using util;
using sync.hash;
using sync.sync;
using sync.work;
using System.Linq;

namespace sync.sync
{
    public class FileDiff : Agent
    {
        public List<HashItem> mores => lgc.incrs;
        public List<HashItem> lacks => lgc.lacks;
        public List<HashItem[]> moves => lgc.moves;

        public FileDiff() { }
        public FileDiff(FileDiffLogic.Param args) => this.args = args;

        public FileDiffLogic.Param args;
        public FileDiffLogic lgc;
        public override void work()
        {
            lgc = new FileDiffLogic(args);

            lgc.BeginParse += Task_BeginParse;
            lgc.UpdateParse += Task_UpdateParse;
            lgc.EndParse += Task_EndParse;

            lgc.BeginCompare += Task_BeginContentCompare;
            lgc.EndCompare += Task_EndContentCompare;
            lgc.CompareUpdate += Task_ContentCompareUpdate;

            run(lgc, args, after: ()=> 
            {
                showResult("compare", compCount,
                            "more", lgc.incrs.Count,
                            "lack", lgc.lacks.Count,
                            "move", lgc.moves.Count);
            });
        }

        Counter compTimer;
        string action;
        int compCount = 0;
        private void Task_BeginContentCompare(HashItem src, HashItem dst)
        {
            compCount++;

            action = $"<compare>{src.path} - {dst.path}";
            log(action);

            compTimer = new Counter { TotalSize = src.size };
            diffTimer.update();
        }

        private void Task_ContentCompareUpdate(long size)
        {
            compTimer.addSize(size);
            diffTimer.update();
        }

        private void Task_EndContentCompare()
        {
            action = null;
            compTimer = null;
            diffTimer.update();
        }

        Counter diffTimer;
        private void Task_BeginParse(int count)
        {
            diffTimer = new Counter
            {
                TotalCount = count,
            };
            diffTimer.TimeIsUp += DiffTimer_timeIsUp;
            diffTimer.trigger();
        }

        private void Task_UpdateParse(int count)
        {
            diffTimer.addCount(count);
        }

        private void Task_EndParse()
        {
            action = null;
            diffTimer.trigger();
        }

        private void DiffTimer_timeIsUp(Counter t)
        {
            var status = t.CountInfo;
            if (compTimer != null)
                status = $"{compTimer.SizePair}, {compTimer.SizePercent} - {status}";

            updateStatus(action, status);
        }
    }
}
