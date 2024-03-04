using sync.hash;
using sync.work;
using System;
using System.Collections.Generic;
using System.Linq;
using util;
using util.ext;
using util.rep;

namespace sync.sync
{
    public class FileDiffLogic : Logic
    {
        public FileDiffLogic(Param args) 
            => this.args = args;

        public Param args;
        public class Param
        {
            public IDir src;
            public IDir dst;
            public List<HashItem> srcFiles;
            public List<HashItem> dstFiles;
            public Compare comp;
        }

        public event Action<int> BeginParse;
        public event Action<int> UpdateParse;
        public event Action EndParse;

        public event Action<HashItem, HashItem> BeginCompare;
        public event Action<long> CompareUpdate;
        public event Action EndCompare;

        public List<HashItem> srcFiles => args.srcFiles;
        public List<HashItem> dstFiles => args.dstFiles;
        public int srcCount => srcFiles.Count;
        public int dstCount => dstFiles.Count;

        public List<HashItem> incrs = new List<HashItem>();
        public List<HashItem> lacks = new List<HashItem>();
        public List<HashItem[]> moves = new List<HashItem[]>();

        public IDir srcRep => args.src;
        public IDir dstRep => args.dst;

        public override void start()
        {
            BeginParse?.Invoke(srcCount);

            var dstCodeMap = dstFiles.toCodeMap();
            foreach (var src in srcFiles.toCodeMap().Values)
            {
                if (dstCodeMap.pop(src.code, out var dst))
                {
                    var cnt = Math.Max(src.copyCount, dst.copyCount);
                    if (cnt == 1)
                    {
                        if (isSameContent(src, dst))
                        {
                            if (!src.samePath(dst))
                            {
                                moves.Add(new HashItem[] { src, dst });
                            }
                        }
                        else
                        {
                            lacks.Add(dst);
                            incrs.Add(src);
                        }
                    }
                    else
                    {
                        // init src/dst matrix
                        var srcs = new HashItem[cnt]
                                .set(0, src)
                                .set(1, src.copys);
                        var dsts = new HashItem[cnt]
                                .set(0, dst)
                                .set(1, dst.copys);

                        // evaluate all match scores
                        var ms = new List<Match>();
                        srcs.each((si, s) => dsts.each((di, d) =>
                        {
                            ms.Add(new Match
                            {
                                s = si,
                                d = di,
                                v = relate(s, d)
                            });
                        }));
                        // sort score by descend
                        ms.Sort();

                        // choose the max score match
                        Match[] match = new Match[cnt];
                        bool[] picks = new bool[cnt];
                        int remain = cnt;
                        foreach (var v in ms)
                        {
                            if (match[v.s] != null || picks[v.d])
                                continue;
                            match[v.s] = v;
                            picks[v.d] = true;
                            remain--;
                            if (remain == 0)
                                break;
                        }

                        // process matchs to incr/lack/move
                        for (int i = 0; i < match.Length; i++)
                        {
                            var subSrc = srcs[i];
                            var subDst = dsts[match[i].d];
                            if (null != subSrc && null != subDst)
                            {
                                if (match[i].v > 1)
                                {
                                    if (!subSrc.samePath(subDst))
                                    {
                                        moves.Add(new HashItem[] { subSrc, subDst });
                                    }
                                }
                                else
                                {
                                    lacks.Add(subDst);
                                    incrs.Add(subSrc);
                                }
                            }
                            else if (null != subSrc && null == subDst)
                            {
                                incrs.Add(subSrc);
                            }
                            else if (null == subSrc && null != subDst)
                            {
                                lacks.Add(subDst);
                            }
                        }
                    }
                }
                else
                {
                    incrs.add(src).add(src.copys);
                }
                UpdateParse?.Invoke(src.copyCount);
            }
            foreach (var dst in dstCodeMap.Values)
            {
                lacks.add(dst).add(dst.copys);
            }

            EndParse?.Invoke();
        }

        int relate(HashItem src, HashItem dst)
        {
            if (null == src || null == dst
                || !isSameContent(src, dst))
            {
                return 1;
            }
            return 5000 + (int)(similar(src, dst) * 100);
        }

        double similar(HashItem src, HashItem dst)
        {
            var srcName = src.name;
            var dstName = dst.name;
            // the same folder name, means change file name
            if (src.dir == dst.dir)
            {
                // 0-1, more to 1 means the file name more likely
                return 3 + LDMaker.similar(srcName, dstName) * 2;
            }
            // the same file name, means move file
            if (srcName == dstName)
            {
                return 3;
            }
            // other cases
            return LDMaker.similar(srcName, dstName) * 2;
        }

        bool isSameContent(HashItem src, HashItem dst)
            => args.comp.isSameData(srcRep, src,
                dstRep, dst,
                BeginCompare, CompareUpdate, EndCompare,
                checkCancel);

        public class Match : IComparable<Match>
        {
            public int s;
            public int d;
            public int v;

            public int CompareTo(Match o)
                => o.v.CompareTo(v);
        }
    }
}
