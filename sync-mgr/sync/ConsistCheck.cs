using sync.work;
using System;
using System.Collections.Generic;
using util;
using util.ext;

namespace sync.sync
{
    public class ConsistCheck : Agent
    {
        public ConsistCheck(ConsistCheckLogic.Param args)
            => this.args = args;

        public ConsistCheckLogic.Param args;
        public ConsistCheckLogic lgc;
        public override void work()
        {
            lgc = new ConsistCheckLogic(args);

            lgc.BeginCheck += Lgc_BeginCheck;
            lgc.EndCheck += Lgc_EndCheck;
            lgc.CheckUpdate += Lgc_CheckUpdate;

            lgc.DiffName += Task_DiffNameFound;
            lgc.DiffData += Task_DiffDataFound;
            lgc.LackFile += Task_LackFileFound;
            lgc.MoreFile += Task_MoreFileFound;

            lgc.LackDir += Task_LackDirFound;
            lgc.MoreDir += Task_MoreDirFound;

            lgc.BeginCompare += Lgc_BeginCompare;
            lgc.CompareUpdate += Lgc_CompareUpdate;
            lgc.EndCompare += Lgc_EndCompare;

            run(lgc, args, after:()=> 
            {
                int sum = 0;
                var items = new List<object>();
                items.add("Compare").add(compCount);
                foreach (var fld in lgc.result.GetType().GetFields())
                {
                    int cnt = (int)fld.GetValue(lgc.result);
                    sum += cnt;
                    items.add(fld.Name).add(cnt);
                }
                showResult(items.ToArray());

                if (sum != 0)
                    throw new Error(this, "Fail", sum);
            });
        }

        Counter checkTimer;
        private void Lgc_BeginCheck()
        {
            checkTimer = new Counter
            { TotalCount = args.srcFiles.Count };
            checkTimer.TimeIsUp += CheckTimer_TimeIsUp;

            checkTimer.trigger();
        }

        private void CheckTimer_TimeIsUp(Counter t)
        {
            var status = t.CountInfo;
            if (compTimer != null)
                status = $"{compTimer.SizePair}, {compTimer.SizePercent} - {status}";

            updateStatus(action, status);
        }

        private void Lgc_CheckUpdate()
        {
            checkTimer.addCount();
        }

        private void Lgc_EndCheck()
        {
            action = null;
            checkTimer.trigger();
        }

        Counter compTimer;
        string action;
        int compCount = 0;
        private void Lgc_BeginCompare(hash.HashItem src, hash.HashItem dst)
        {
            compCount++;

            action = $"<compare>{src.path}";
            log(action);

            compTimer = new Counter { TotalSize = src.size };
            checkTimer.update();
        }

        private void Lgc_CompareUpdate(long delta)
        {
            compTimer.addSize(delta);
            checkTimer.update();
        }

        private void Lgc_EndCompare()
        {
            action = null;
            compTimer = null;
            checkTimer.update();
        }

        private void Task_MoreDirFound(string dst)
        {
            log($"<more dir>{dst}");
        }

        private void Task_LackDirFound(string src)
        {
            log($"<lack dir>{src}");
        }

        private void Task_MoreFileFound(string dst)
        {
            log($"<more file>{dst}");
        }

        private void Task_LackFileFound(string src)
        {
            log($"<lack file>{src}");
        }

        private void Task_DiffDataFound(string src, string dst)
        {
            log($"<diff data>{src} - {dst}");
        }

        private void Task_DiffNameFound(string src, string dst)
        {
            log($"<diff name>{src} - {dst}");
        }
    }
}
