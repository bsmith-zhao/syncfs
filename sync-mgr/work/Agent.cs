using util;
using System;
using util.ext;
using System.Collections.Generic;
using System.Collections;

namespace sync.work
{
    public abstract class Agent
    {
        public Action CancelCheck;
        public Action<string> MsgOutput;
        public Action<string> LogOutput;
        public Action<string, string> StatusUpdate;

        public abstract void work();

        protected void log(string str)
        {
            LogOutput?.Invoke(str);
        }

        protected void msg(string str)
        {
            MsgOutput?.Invoke(str);
        }

        protected void updateStatus(string action, string status)
        {
            StatusUpdate?.Invoke(action, status);
        }

        string lgcName => $"{this.trans("name")}";

        protected void beginLogic()
        {
            var name = lgcName;
            log($"\r\n[{DateTime.Now}][{name}]<begin>");
            msg($"\r\n[{name}]");
        }

        protected void endLogic()
        {
            log($"\r\n[{DateTime.Now}][{lgcName}]<end>");
        }

        protected void run(Logic lgc, object args, Action before = null, Action after = null)
        {
            lgc.CancelCheck = CancelCheck;
            beginLogic();
            log($"{args.desc(v => (v is IList ls) ? ls.Count : v)}");
            before?.Invoke();
            lgc.start();
            after?.Invoke();
            endLogic();
        }

        protected void showResult(params object[] res)
        {
            for (int i = 0; i < res.Length; i += 2)
                msg($"{this.trans(res[i].ToString())}\t{res[i + 1]}");
        }
    }
}
