using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.prop;
using util.rep;

namespace sync.app.conf
{
    public class ViewConf : FilterConf
    {
        [Category("2.View"), UnifyPath]
        public string RootDir { get; set; }

        [Category("2.View")]
        public bool ForceUnlock { get; set; }

        public View openView(View src)
            => new View
            {
                src = src,
                rep = src.rep,
                root = RootDir,
                flt = new Filter(AllowPaths, ExcludePaths, ExcludeNames),
            };
    }
}
