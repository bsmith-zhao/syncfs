using sync.app;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;
using util.prop;

namespace sync.ui
{
    public class ArgsConf
    {
        public string path;
        public Type type;
        public Action OnActive;
        public Func<object, string, bool> OnChange;

        object args;
        public object Args
        {
            get => args ?? (args = path.readText().obj(type));
            set => value.update(ref args, save);
        }

        public void save()
            => args.jsonIndent().bakSaveTo(path);
    }
}
