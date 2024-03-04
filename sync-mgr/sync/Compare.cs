using sync.hash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace sync.sync
{
    public enum CompareType
    {
        Hash, Data,
    }

    public class Compare
    {
        public CompareType @default = CompareType.Hash;
        public string rules => rs.join(",");
        public string cache;
        public TimeSpan expire;

        public override string ToString()
            => this.desc();

        List<Rule> rs = new List<Rule>();
        public Compare(CompareType @default, string[] byHash, string[] byData)
        {
            this.@default = @default;

            byHash.each(p =>
                rs.Add(new Rule
                { path = p, type = CompareType.Hash }));
            byData.each(p =>
                rs.Add(new Rule
                { path = p, type = CompareType.Data }));

            rs.Sort();
            rs.Reverse();
        }

        public bool isByData(HashItem src, HashItem dst)
            => isByData(src) || isByData(dst);

        public bool isByData(HashItem unit)
            => isByData(unit.lowPath);

        bool isByData(string path)
            => @default == CompareType.Data
            || (rs.first(p => path.StartsWith(p.path), out var pt)
                && pt.type == CompareType.Data);

        int buffSize = 1.mb();
        byte[] srcBuff;
        byte[] dstBuff;
        public bool isSameData(IDir srcRep, HashItem src,
            IDir dstRep, HashItem dst, 
            Action<HashItem, HashItem> begin, Action<long> update, Action end, 
            Action cancel)
        {
            if (src.size != dst.size)
                return false;
            if (src.size == 0)
                return true;
            if (!isByData(src, dst))
                return true;

            string key = src.path;
            if (src.path != dst.path)
                key = $"{src.path}:{dst.path}";
            cm = cm ?? loadCache();
            Record ck;
            if ((ck = cm.get(key)) != null
                && ck.compTime > (DateTime.Now - expire).longMs()
                && src.createTime == ck.srcCreate
                && src.modifyTime == ck.srcWrite
                && dst.createTime == ck.dstCreate
                && dst.modifyTime == ck.dstWrite
                && src.size == ck.size)
            {
                return ck.keep = true;
            }

            begin?.Invoke(src, dst);
            bool same = false;
            using (var srcIn = srcRep.readFile(src.path))
            using (var dstIn = dstRep.readFile(dst.path))
            {
                same = srcIn.isSame(dstIn, delta => 
                {
                    cancel?.Invoke();
                    update?.Invoke(delta);
                }, ref srcBuff, ref dstBuff, buffSize);
            }
            end?.Invoke();

            if (same && cm != null)
            {
                cm[key] = new Record
                {
                    key = key,
                    srcCreate = src.createTime,
                    srcWrite = src.modifyTime,
                    dstCreate = dst.createTime,
                    dstWrite = dst.modifyTime,
                    size = src.size,
                    compTime = now,
                    keep = true,
                };
            }

            return same;
        }

        Map<string, Record> cm;
        long now = DateTime.Now.longMs();
        Map<string, Record> loadCache()
        {
            var cks = new Map<string, Record>();
            if (!cache.fileExist())
                return cks;
            using (var fin = File.OpenText(cache))
            {
                var header = fin.ReadLine();
                string row;
                int ci;
                while (fin.readRow(out row))
                {
                    var ps = row.Split('|');
                    ci = 0;
                    var ck = new Record
                    {
                        compTime = long.Parse(ps[ci++]),
                        srcCreate = long.Parse(ps[ci++]),
                        srcWrite = long.Parse(ps[ci++]),
                        dstCreate = long.Parse(ps[ci++]),
                        dstWrite = long.Parse(ps[ci++]),
                        size = long.Parse(ps[ci++]),
                        key = ps[ci++],
                    };
                    cks[ck.key] = ck;
                }
            }
            return cks;
        }

        public void saveCache()
        {
            if (cm == null || cache == null)
                return;

            using (var fout = File.CreateText(cache))
            {
                fout.WriteLine("compare-time, src-create, src-write, dst-create, dst-write, size, key");
                foreach (var u in cm.Values)
                {
                    if (u.keep)
                        fout.WriteLine($"{u.compTime}|{u.srcCreate}|{u.srcWrite}|{u.dstCreate}|{u.dstWrite}|{u.size}|{u.key}");
                }
            }
        }

        public class Rule : IComparable<Rule>
        {
            public string path;
            public CompareType type;

            public int CompareTo(Rule other)
                => path.CompareTo(other.path);

            string code => type == CompareType.Hash ? "h" : "c";
            public override string ToString()
                => $"{path}:{code}";
        }

        public class Record
        {
            public long compTime;
            public long srcCreate;
            public long srcWrite;
            public long dstCreate;
            public long dstWrite;
            public long size;
            public string key;
            public bool keep = false;
        }
    }
}
