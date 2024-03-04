using sync.work;
using System;
using System.Collections.Generic;
using util;
using util.rep;

namespace sync.sync
{
    public class DirAdjustLogic : Logic
    {
        public DirAdjustLogic(Param args) => this.args = args;

        public Param args;
        public class Param
        {
            public IDir rep;

            public List<string> adds;
            public List<string> dels;
            public List<string[]> moves;
        }

        public event Action BeginAdjust;
        public event Action<string> DeleteDir;
        public event Action<string, string> MoveDir;
        public event Action<string> CreateDir;
        public event Action EndAdjust;

        private IDir rep => args.rep;

        public override void start()
        {
            BeginAdjust?.Invoke();

            List<string> adds = args.adds;
            List<string> dels = args.dels;
            List<string[]> moves = args.moves;

            dels.Sort();
            dels.Reverse();
            foreach (var dir in dels)
            {
                DeleteDir?.Invoke(dir);

                rep.deleteDir(dir);
            }

            foreach (var move in moves)
            {
                var old = move[0];
                var @new = move[1];

                MoveDir?.Invoke(old, @new);

                rep.moveDir(old, @new);
            }

            foreach (var dir in adds)
            {
                CreateDir?.Invoke(dir);

                rep.createDir(dir);
            }

            EndAdjust?.Invoke();
        }
    }
}
