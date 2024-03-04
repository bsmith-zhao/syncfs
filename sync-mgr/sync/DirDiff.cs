using sync.work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;

namespace sync.sync
{
    public class DirDiff : Agent
    {
        public DirDiffLogic.Param args;
        public DirDiffLogic lgc;

        public DirDiff(DirDiffLogic.Param args) 
            => this.args = args;

        public override void work()
        {
            lgc = new DirDiffLogic { args = args};

            run(lgc, args, after: () =>
            {
                showResult("more", lgc.incrs.Count,
                            "lack", lgc.lacks.Count,
                            "move", lgc.moves.Count);
            });
        }
    }
}
