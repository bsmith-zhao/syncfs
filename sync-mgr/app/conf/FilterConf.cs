using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace sync.app
{
    [TypeConverter(typeof(ExpandProp))]
    public class FilterConf
    {
        string[] incPaths;
        [Category("2.View")]
        [TypeConverter(typeof(ArrayProp))]
        public string[] AllowPaths
        {
            get => incPaths;
            set => incPaths = value.pathUnify();
        }

        string[] excsPaths;
        [Category("2.View")]
        [TypeConverter(typeof(ArrayProp))]
        public string[] ExcludePaths
        {
            get => excsPaths;
            set => excsPaths = value.pathUnify();
        }

        string[] excNames;
        [Category("2.View")]
        [TypeConverter(typeof(ArrayProp))]
        public string[] ExcludeNames
        {
            get => excNames;
            set => excNames = value.pathUnify();
        }

        public Filter newFilter(params string[] names)
            => new Filter
            {
                incPaths = incPaths.pathUnify(),
                excPaths = excsPaths.pathUnify(),
                excNames = excNames.append(names).pathUnify(),
            }.init();
    }
}
