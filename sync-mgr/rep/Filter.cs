using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;

namespace util.rep
{
    public class Filter
    {
        public string[] incs;
        public string[] excs;
        public string[] names;

        public override string ToString()
            => this.desc();

        public Filter(string[] incs, string[] excs, string[] names)
        {
            this.incs = incs.pathUnify().low();
            this.excs = excs.pathUnify().low();
            this.names = names.pathUnify().low();

            init();
        }

        void init()
        {
            incs.each(inc => excs.each((i, exc) =>
            {
                if (pathBelong(inc, exc))
                    excs[i] = null;
            }));

            excs = excs.exclude(p => p.empty())
                .ToArray().remain(a => a.Length > 0);
        }

        public bool allowDir(string path)
            => allowPath(path, true);

        public bool allowFile(string path)
            => allowPath(path, false);

        public bool allowPath(string path, bool isDir)
        {
            path = path.low();
            if (!include(path, isDir))
                return false;
            return !exclude(path);
        }

        bool include(string path, bool isDir)
            => !(incs?.Length > 0
                && !incs.exist(inc => pathBelong(path, inc)
                            || (isDir && pathBelong(inc, path))));

        bool exclude(string path)
        {
            if (excs.exist(exc => pathBelong(path, exc)))
                return true;
            if (names?.Length > 0)
            {
                var nodes = path.Split('/');
                return names.exist(name 
                    => nodes.exist(node => node == name));
            }
            return false;
        }

        bool pathBelong(string path, string dir)
            => path != null && dir != null
            && path.StartsWith(dir)
            && (path.Length == dir.Length 
                || path[dir.Length] == '/');
    }
}
