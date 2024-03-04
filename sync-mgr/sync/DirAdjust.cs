using sync.work;
using util;

namespace sync.sync
{
    public class DirAdjust : Agent
    {
        public DirAdjust(DirAdjustLogic.Param args) 
            => this.args = args;

        public DirAdjustLogic lgc;
        public DirAdjustLogic.Param args;

        public override void work()
        {
            lgc = new DirAdjustLogic(args);

            lgc.CreateDir += Logic_CreateDir;
            lgc.DeleteDir += Logic_DeleteDir;
            lgc.MoveDir += Logic_MoveDir;

            run(lgc, args, before:()=> 
            {
                showResult("target", $"{args.rep}");
            }, after:() => 
            {
                showResult("add", args.adds.Count,
                            "delete", args.dels.Count,
                            "move", args.moves.Count);
            });
        }

        private void Logic_MoveDir(string src, string dst)
        {
            log($"<move>{src} -> {dst}");
        }

        private void Logic_DeleteDir(string src)
        {
            log($"<delete>{src}");
        }

        private void Logic_CreateDir(string src)
        {
            log($"<create>{src}");
        }
    }
}
