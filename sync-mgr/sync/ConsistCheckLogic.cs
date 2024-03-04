using System;
using System.Collections.Generic;
using util;
using util.ext;
using sync.hash;
using sync.sync;
using sync.work;
using util.rep;

namespace sync.sync
{
    public class ConsistCheckLogic : Logic
    {
        public ConsistCheckLogic(Param args)
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

        public Result result;
        public class Result
        {
            public int diffName;
            public int diffData;

            public int lackFile;
            public int moreFile;

            public int lackDir;
            public int moreDir;
        }

        public event Action BeginCheck;

        public event Action<HashItem, HashItem> BeginCompare;
        public event Action<long> CompareUpdate;
        public event Action EndCompare;

        public event Action CheckUpdate;

        public event Action<string, string> DiffName;
        public event Action<string, string> DiffData;

        public event Action<string> LackFile;
        public event Action<string> MoreFile;

        public event Action<string> LackDir;
        public event Action<string> MoreDir;

        public event Action EndCheck;

        public IDir srcRep => args.src;
        public IDir dstRep => args.dst;

        public List<string> dstDirs;

        public override void start()
        {
            result = new Result();

            BeginCheck?.Invoke();
            {
                var dstLowMap = args.dstFiles.toMap(u=>u.lowPath);
                foreach (var src in args.srcFiles)
                {
                    var dst = dstLowMap.pop(src.lowPath);
                    if (null != dst)
                    {
                        // compare src and dst name
                        if (src.name != dst.name)
                        {
                            // un consist file name found
                            result.diffName++;
                            DiffName?.Invoke(src.path, dst.path);
                        }
                        // compare src and dst code
                        if (src.code != dst.code
                            || !isSameContent(src, dst))
                        {
                            // un consist file content found
                            result.diffData++;
                            DiffData?.Invoke(src.path, dst.path);
                        }
                    }
                    else
                    {
                        // lack file found
                        result.lackFile++;
                        LackFile?.Invoke(src.path);
                    }

                    CheckUpdate?.Invoke();
                }
                foreach (var dst in dstLowMap.Values)
                {
                    // more file found
                    result.moreFile++;
                    MoreFile?.Invoke(dst.path);
                }
            }

            {
                var dstLowMap = dstRep.lowAllDirMap();
                dstDirs = new List<string>(dstLowMap.Values);
                foreach (var src in srcRep.enumAllDirs())
                {
                    var dst = dstLowMap.pop(src.low());
                    if (null != dst)
                    {
                        if (src.pathName() != dst.pathName())
                        {
                            result.diffName++;
                            DiffName?.Invoke(src, dst);
                        }
                    }
                    else
                    {
                        result.lackDir++;
                        LackDir?.Invoke(src);
                    }
                };
                foreach (var dst in dstLowMap.Values)
                {
                    result.moreDir++;
                    MoreDir?.Invoke(dst);
                }
            }

            EndCheck?.Invoke();
        }

        bool isSameContent(HashItem src, HashItem dst)
            => args.comp.isSameData(srcRep, src, 
                dstRep, dst, 
                BeginCompare, CompareUpdate, EndCompare, 
                checkCancel);
    }
}
