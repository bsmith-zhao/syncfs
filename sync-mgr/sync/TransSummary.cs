using sync.hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;

namespace sync.sync
{
    public class TransSummary
    {
        const string RootDir = "";

        Dictionary<string, DirSum> dirs
            = new Dictionary<string, DirSum>();

        public static DirNode parse(FileDiffLogic lgc)
            => new TransSummary().parse(lgc.incrs, lgc.lacks, lgc.moves);

        public static DirNode parse(Transfer<HashItem> trans)
            => new TransSummary().parse(trans.adds, trans.dels, trans.moves);

        public DirNode parse(List<HashItem> adds, 
            List<HashItem> dels, List<HashItem[]> moves)
        {
            adds.each(u =>
            {
                getDir(u).create.add(u);
            });

            dels.each(u =>
            {
                getDir(u).delete.add(u);
            });

            var moveSum = new Sum();
            foreach (var mv in moves)
            {
                var src = mv[0];
                var dst = mv[1];
                if (src.path.pathDir()?.ToLower()
                    == dst.path.pathDir()?.ToLower())
                {
                    getDir(src).rename.add(src);
                }
                else
                {
                    getDir(src).moveOut.add(src);
                    getDir(dst).moveIn.add(dst);
                }
            }

            DirNode root = new DirNode();
            dirs.Values.each(root.add);

            return root;
        }

        DirSum getDir(HashItem u)
        {
            var dir = u.path.pathDir() ?? RootDir;
            return dir.get(ref dirs, () => new DirSum { dir = dir });
        }

        public class DirSum
        {
            public string dir;
            Sum cr;
            public Sum create => cr ?? (cr = new Sum());
            Sum del;
            public Sum delete => del ?? (del = new Sum());
            Sum rn;
            public Sum rename => rn ?? (rn = new Sum());
            Sum mi;
            public Sum moveIn => mi ?? (mi = new Sum());
            Sum mo;
            public Sum moveOut => mo ?? (mo = new Sum());

            public override string ToString()
                => new string[]
                {
                    cr?.str(s=>$"+{s}"),
                    del?.str(s=>$"-{s}"),
                    rn?.str(s=>$"r{s}"),
                    mi?.str(s=>$"m+{s}"),
                    mo?.str(s=>$"m-{s}"),
                }.exclude(s => s == null).join(",");
        }

        public class Sum
        {
            int count;
            long size;

            public void add(HashItem u)
            {
                count++;
                size += u.size;
            }

            public override string ToString()
                => size.byteSize();
        }

        public class DirNode
        {
            public string name;
            public DirSum sum;
            public Dictionary<string, DirNode> nodes;

            public override string ToString()
                => $"{name}{sum?.str(s => $"({s})")}";

            public void add(DirSum sum)
            {
                var sub = this;
                if (sum.dir != RootDir)
                {
                    sum.dir.pathSplit().each(nd =>
                        sub = nd.get(ref sub.nodes,
                                () => new DirNode { name = nd }));
                }
                sub.sum = sum;
            }
        }
    }
}
