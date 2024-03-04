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
        public string[] incPaths;
        public string[] excPaths;
        public string[] excNames;

        public override string ToString()
            => this.desc();

        public Filter init()
        {
            incPaths.each(inc =>
            {
                excPaths.each((i, exc) =>
                {
                    if (contain(inc, exc))
                        excPaths[i] = null;
                });
            });
            return this;
        }

        public bool allowDir(string path)
            => allowNode(path, true);

        public bool allowFile(string path)
            => allowNode(path, false);

        public bool allowNode(string path, bool isDir)
        {
            path = path.low();
            if (!allow(path, isDir))
                return false;
            return !exclude(path);
        }

        bool allow(string path, bool isDir)
            => !(incPaths?.Length > 0
                && !incPaths.exist(inc => contain(path, inc)
                            || (isDir && contain(inc, path))));

        bool exclude(string path)
            => excPaths.exist(exc => null != exc && contain(path, exc))
            || excNames.exist(key => path.Contains(key));

        bool contain(string path, string pre)
            => path.StartsWith(pre)
            && (path.Length == pre.Length 
                || path[pre.Length] == '/');
    }
}
