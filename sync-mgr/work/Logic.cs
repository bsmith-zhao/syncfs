using System;

namespace sync.work
{
    public abstract class Logic
    {
        public abstract void start();

        public Action CancelCheck;

        public void checkCancel()
            => CancelCheck?.Invoke();
    }
}
