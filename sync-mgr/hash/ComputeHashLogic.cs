using System;
using System.Collections.Generic;
using System.IO;
using util;
using util.ext;
using sync.sync;
using sync.work;
using util.rep;

namespace sync.hash
{
    public class ComputeHashLogic : Logic
    {
        public ComputeHashLogic(Param args) 
            => this.args = args;

        public Param args;
        public class Param
        {
            public IDir src;
            public string dst;
            public Hash hash;
        }

        public event Action InitCompute;
        public event Action<int> BeginCompute;
        public event Action<HashItem> SkipComputeFile;
        public event Action<HashItem> BeginComputeFile;
        public event Action<long> ComputeFileUpdate;
        public event Action<HashItem> EndComputeFile;
        public event Action EndCompute;

        public Hash hash => args.hash;
        public List<HashItem> files;
        public int updateCount = 0;
        public int reduceCount = 0;

        public override void start()
        {
            InitCompute?.Invoke();

            var lowMap = hash.loadUnits(args.dst).toMap(u=>u.lowPath);

            files = args.src.enumAllFiles<HashItem>().toList();

            BeginCompute?.Invoke(files.Count);

            var now = DateTime.Now;
            long hashTime = now.longMs();
            long expireTime = (now - args.hash.expire).longMs();
            using (var alg = hash.newAlg())
            {
                foreach (var unit in files)
                {
                    if (lowMap.pop(unit.lowPath, out var last)
                        && unit.name == last.name
                        && unit.size == last.size
                        && unit.createTime == last.createTime
                        && unit.modifyTime == last.modifyTime
                        && last.hashTime > expireTime)
                    {
                        unit.code = last.code;
                        unit.hashTime = last.hashTime;
                        SkipComputeFile?.Invoke(unit);
                    }
                    else
                    {
                        BeginComputeFile?.Invoke(unit);
                        using (var fin = args.src.readFile(unit.path))
                        {
                            unit.code = hash.compute(fin, alg, delta =>
                            {
                                checkCancel();

                                ComputeFileUpdate?.Invoke(delta);
                            }).b64().TrimEnd('=');
                            unit.hashTime = hashTime;
                        }
                        updateCount++;
                        EndComputeFile?.Invoke(unit);
                    }
                }
            }
            reduceCount = lowMap.Count;

            if (!args.dst.fileExist()
                || updateCount > 0
                || reduceCount > 0)
                hash.saveUnits(files, args.dst);

            EndCompute?.Invoke();
        }
    }

    public static class HashEx
    {
        public static List<HashItem> loadUnits(this Hash hash, string path)
        {
            var units = new List<HashItem>();
            if (!path.fileExist())
                return units;
            using (var fin = File.OpenText(path))
            {
                // hash spec
                var spec = fin.ReadLine();
                if (hash.getSpec() != spec)
                    return units;
                // header
                var header = fin.ReadLine();
                // unit list
                string row;
                int ci;
                while (fin.readRow(out row))
                {
                    var ps = row.Split('|');
                    ci = 0;
                    var unit = new HashItem
                    {
                        code = ps[ci++].TrimEnd('='),
                        hashTime = long.Parse(ps[ci++]),
                        createTime = long.Parse(ps[ci++]),
                        modifyTime = long.Parse(ps[ci++]),
                        size = long.Parse(ps[ci++]),
                        path = ps[ci++],
                    };
                    units.Add(unit);
                }
            }
            return units;
        }

        public static void saveUnits(this Hash hash, List<HashItem> units, string path)
        {
            using (var fout = File.CreateText(path))
            {
                fout.WriteLine(hash.getSpec());
                fout.WriteLine("code, hash-time, create-time, write-time, size, path");
                foreach (var u in units)
                {
                    fout.WriteLine($"{u.code}|{u.hashTime}|{u.createTime}|{u.modifyTime}|{u.size}|{u.path}");
                }
            }
        }
    }
}
