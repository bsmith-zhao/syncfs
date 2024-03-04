using sync.hash;
using sync.work;
using System;
using System.Collections.Generic;
using util;
using util.ext;
using util.rep;

namespace sync.sync
{
    public class RoundDiffLogic : Logic
    {
        public RoundDiffLogic(Param args)
            => this.args = args;

        public Param args;
        public class Param
        {
            public IDir src;
            public IDir dst;

            public List<HashItem> fileIncrs;
            public List<HashItem> fileLacks;
            public List<HashItem[]> fileMoves;

            public List<string> dirIncrs;
            public List<string> dirLacks;
            public List<string[]> dirMoves;

            public List<HashItem> lastFiles;
            public List<string> lastDirs;
        }

        public InterTrans<HashItem> fileTrans;
        public InterTrans<string> dirTrans;

        public List<HashItem[]> fileMoveConfuses => fileTrans.confuses;
        public List<HashItem[]> fileNameConflicts = new List<HashItem[]>();
        public List<string[]> dirMoveConfuses => dirTrans.confuses;

        public int unsolvedCount 
            => fileTrans.confuses.Count
            + dirTrans.confuses.Count
            + fileNameConflicts.Count;

        public override void start()
        {
            parseFiles();
            parseDirs();
        }

        Dictionary<string, HashItem> lastFiles;
        void parseFiles()
        {
            lastFiles = args.lastFiles.toCodeMap();
            var trans = (fileTrans = transParse(args.fileIncrs, 
                args.fileLacks, args.fileMoves, fileExistInLast));

            var srcExists = new List<HashItem>(trans.dst.adds);
            trans.dst.moves.each(m => srcExists.Add(m[1]));
            trans.confuses.each(m => srcExists.Add(m[0]));

            var dstExists = new Dictionary<string, HashItem>();
            trans.src.adds.each(u => dstExists[u.lowPath] = u);
            trans.src.moves.each(m => dstExists[m[1].lowPath] = m[1]);
            foreach (var src in srcExists)
            {
                if (dstExists.get(src.lowPath, out var dst))
                {
                    fileNameConflicts.Add(new HashItem[] { src, dst });
                }
            }
        }

        public bool fileExistInLast(HashItem unit)
            => lastFiles.get(unit.code, out var past)
                && (unit.samePath(past) 
                    || past.copys.exist(p => p.samePath(unit)));

        Dictionary<string, string> lastDirs;
        void parseDirs()
        {
            lastDirs = args.lastDirs.toMap(d => d.low());

            dirTrans = transParse(args.dirIncrs, 
                args.dirLacks, args.dirMoves, dirExistInLast);
        }

        public bool dirExistInLast(string dir)
            => lastDirs.get(dir.low(), out var past)
                && dir.pathName() == past.pathName();

        public class InterTrans<T>
        {
            public Transfer<T> src = new Transfer<T>();
            public Transfer<T> dst = new Transfer<T>();
            public List<T[]> confuses = new List<T[]>();
        }

        InterTrans<T> transParse<T>(List<T> incrs, List<T> lacks, List<T[]> moves, Func<T, bool> exist)
        {
            var trans = new InterTrans<T>();
            foreach (var incr in incrs)
            {
                if (exist(incr))
                    trans.src.dels.Add(incr);
                else
                    trans.dst.adds.Add(incr);
            }
            foreach (var lack in lacks)
            {
                if (exist(lack))
                    trans.dst.dels.Add(lack);
                else
                    trans.src.adds.Add(lack);
            }
            foreach (var move in moves)
            {
                var src = move[0];
                var dst = move[1];
                var srcMove = !exist(src);
                var dstMove = !exist(dst);
                if (!srcMove && dstMove)
                    trans.src.moves.Add(new T[] { src, dst });
                else if (srcMove && !dstMove)
                    trans.dst.moves.Add(new T[] { dst, src });
                else
                    trans.confuses.Add(move);
            }
            return trans;
        }
    }
}
