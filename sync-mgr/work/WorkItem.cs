using sync.work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;

namespace sync.work
{
    public class UserCancel : Exception
    {
    }

    public abstract class WorkItem
    {
        public abstract string group { get; }
        public abstract string dir { get; }
        public abstract string name { get; }
        public abstract string icon { get; }
        public abstract bool check { get; }
        public abstract bool canParse { get; }

        public
            Action CheckCancel;
        public
            Action<string, string> UpdateStatus;
        public
            Action<string> MsgOutput;
        public
            Action<string> LogOutput;
        public
            Control PanelOutput;

        public abstract void parse();
        public abstract void work();
    }
}
