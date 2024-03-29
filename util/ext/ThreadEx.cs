﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace util.ext
{
    public static class ThreadEx
    {
        public static int thdId(this bool src)
            => Thread.CurrentThread.ManagedThreadId;

        public static void sleep(this bool src, int ms)
            => Thread.Sleep(ms);

        // Environment.ProcessorCount - cpu logic cores
        public static void runParallel(this int total, int minThds, int maxThds, int minPerThd, Action<int, int> func)
        {
            if (total < minThds * minPerThd)
            {
                func(0, total);
                return;
            }

            var thdCount = maxThds.atMost(total / minPerThd);
            var countPerThd = total / thdCount;
            if (total % thdCount != 0)
                countPerThd++;

            var tasks = new List<Task>();
            int pos = 0;
            while (pos < total)
            {
                var cnt = countPerThd.atMost(total - pos);
                var off = pos;
                tasks.Add(Task.Run(() => func(off, cnt)));
                pos += cnt;
            }
            Task.WaitAll(tasks.ToArray());
        }

        public static void runByThd(this bool api,
                        ThreadStart func)
            => new Thread(func).Start();

        public static void runByThd(this bool api, 
                        ref Thread thd,
                        ThreadStart func)
        {
            if (thd != null)
                return;
            thd = new Thread(func);
            thd.Start();
        }

        public static bool isActive(this BackgroundWorker thd)
            => thd?.IsBusy == true;

        public static void run(this BackgroundWorker oldThd, ref BackgroundWorker newThd, Action func, Action<Exception> end = null)
        {
            oldThd.run(ref newThd, null, func, end);
        }

        public static void run(this BackgroundWorker oldThd, ref BackgroundWorker newThd, Func<bool> begin, Action func, Action<Exception> end = null)
        {
            newThd = oldThd;

            if (newThd.isActive()
                || begin?.Invoke() == false)
                return;

            newThd = new BackgroundWorker();
            newThd.WorkerSupportsCancellation = true;
            newThd.DoWork += (s, e) => func();
            if (null != end)
                newThd.RunWorkerCompleted += (s, e) => end?.Invoke(e.Error);

            newThd.RunWorkerAsync();
        }
    }
}
