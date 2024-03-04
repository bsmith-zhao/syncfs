using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public class EventAgent<T> where T : EventArgs
    {
        Action<object, T> dispatch;
        public EventAgent(Action<object, T> disp)
        {
            this.dispatch = disp;
        }

        bool pause = false;
        public void suspend(Action func)
        {
            try
            {
                pause = true;
                func();
            }
            finally
            {
                pause = false;
            }
        }

        public void trigger(object s, T e)
        {
            if (pause)
                return;
            dispatch(s, e);
        }
    }
}
