using sync.work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace sync.sync
{
    public class DirDiffLogic : Logic
    {
        public Param args;
        public class Param
        {
            public IDir src;
            public IDir dst;
        }

        public List<string> incrs;
        public List<string> lacks;
        public List<string[]> moves;

        public override void start()
        {
            incrs = new List<string>();
            moves = new List<string[]>();

            var dstLowMap = args.dst.lowAllDirMap();
            foreach (var src in args.src.enumAllDirs())
            {
                if (dstLowMap.pop(src.low(), out var dst))
                {
                    if (src.pathName() != dst.pathName())
                        moves.Add(new string[] { src, dst });
                }
                else
                    incrs.Add(src);
            };

            lacks = new List<string>(dstLowMap.Values);
        }
    }
}
