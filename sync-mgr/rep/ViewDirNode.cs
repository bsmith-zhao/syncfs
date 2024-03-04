using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.rep
{
    public class ViewDirNode : DirItem
    {
        public View view;
        public DirItem src;
        
        Filter flt => view.flt;

        public override IEnumerable<DirItem> enumDirs()
        {
            foreach (var dn in src.enumDirs())
            {
                var path = view.toViewPath(dn.path);
                if (flt.allowDir(path))
                    yield return new ViewDirNode
                    {
                        view = view,
                        src = dn,
                        path = path,
                    };
            }
        }

        public override IEnumerable<T> enumFiles<T>()
        {
            foreach (var fn in src.enumFiles<T>())
            {
                if (flt.allowFile(fn.path = view.toViewPath(fn.path)))
                {
                    yield return fn;
                }
            }
        }
    }
}
