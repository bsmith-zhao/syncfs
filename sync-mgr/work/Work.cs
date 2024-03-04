using util;
using System;
using System.IO;
using util.ext;
using System.Windows.Forms;

namespace sync.work
{
    public abstract class Work
    {
        public string dir;

        public
            Action CheckCancel;
        public
            Action<string> MsgOutput;
        public
            Action<string, string> UpdateStatus;
        public
            Action<string> LogOutput;
        public 
            Control PanelOutput;

        public abstract void begin();
        public virtual void end() { }

        public T run<T>(T ag) where T : Agent
        {
            ag.CancelCheck = CheckCancel;
            ag.MsgOutput = MsgOutput;
            ag.StatusUpdate = UpdateStatus;
            ag.LogOutput = LogOutput;
            ag.work();
            return ag;
        }

        protected void msg(string info)
        {
            MsgOutput?.Invoke(info);
        }

        //protected void msg(params string[] infos)
        //{
        //    MsgOutput?.Invoke(string.Join("\r\n", infos));
        //}
    }
}
