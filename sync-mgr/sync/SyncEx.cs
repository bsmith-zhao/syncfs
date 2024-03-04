using sync.hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;
using util.rep;

namespace sync.sync
{
    public static class SyncEx
    {
        public static Dictionary<string, HashItem> toCodeMap(this List<HashItem> list)
            => list.each(f => f.copys = null)
                .toMap<string, HashItem>((f, map) =>
                {
                    if (map.get(f.code, out var past))
                        past.copys = past.copys.add(f);
                    else
                        map.Add(f.code, f);
                });

        public static Dictionary<string, string> lowAllDirMap(this IDir dir)
            => dir.enumAllDirs().toMap(d => d.low());
    }
}
