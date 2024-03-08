using util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace sync.work
{
    public class Worker
    {
        public int logCount = 10;
        public string logFile = "work.log";

        public void run(WorkItem item, Work job)
        {
            var workDir = item.dir;
            job.dir = workDir;

            job.CheckCancel = item.CheckCancel;
            job.UpdateStatus = item.UpdateStatus;
            job.MsgOutput = item.MsgOutput;
            job.LogOutput = item.LogOutput;

            job.PanelOutput = item.PanelOutput;

            job.MsgOutput += log;
            job.LogOutput += log;

            var logPath = workDir.pathMerge(logFile);
            if (logPath.fileExist())
            {
                var bakPath = logPath.pathBackup(logCount - 1, File.Exists, File.Delete, "-");
                File.Move(logPath, bakPath);
            }
            using (fout = File.AppendText(logPath))
            {
                try
                {
                    log($"[{DateTime.Now}][{workDir}]<begin>\r\n{job}");
                    try
                    {
                        job.begin();
                    }
                    finally
                    {
                        try { job.end(); } catch { }
                    }
                    log($"\r\n[{DateTime.Now}][{workDir}]<end>");
                }
                catch (Exception err)
                {
                    log($"\r\n[{DateTime.Now}]<Error>{err.Message}\r\n{err.StackTrace}");
                    throw err;
                }
            }
        }

        private StreamWriter fout;
        private void log(string msg)
        {
            fout.WriteLine(msg);
        }
    }
}
