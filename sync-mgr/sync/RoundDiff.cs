using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sync.work;

namespace sync.sync
{
    public class RoundDiff : Agent
    {
        public RoundDiffLogic.Param args;
        public RoundDiff(RoundDiffLogic.Param args)
            => this.args = args;

        public RoundDiffLogic lgc;
        public override void work()
        {
            lgc = new RoundDiffLogic(args);
            run(lgc, args);
        }
    }
}
