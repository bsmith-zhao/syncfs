using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class ProcessEx
    {
        public static async void runCmdAwait(this string cmd, string args,
            Action<Process> before,
            Action<Process> after,
            Action<string> stdout,
            Action<string> stderr)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = args;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                if (stdout != null)
                {
                    p.StartInfo.RedirectStandardOutput = true;
                    p.OutputDataReceived += (s, e) =>
                    {
                        if (null != stdout && null != e.Data)
                            stdout(e.Data);
                    };
                }
                if (null != stderr)
                {
                    p.StartInfo.RedirectStandardError = true;
                    p.ErrorDataReceived += (s, e) =>
                    {
                        if (null != e.Data)
                            stderr(e.Data);
                    };
                }

                p.Start();

                if (null != stdout)
                    p.BeginOutputReadLine();
                if (null != stderr)
                    p.BeginErrorReadLine();

                before?.Invoke(p);

                await Task.Run(() => p.WaitForExit());
                after?.Invoke(p);
            }
        }

        public static async void awaitEnd(this Process proc,
            Action<Process> end = null)
        {
            await Task.Run(() => proc.WaitForExit());
            end?.Invoke(proc);
        }

        public static int runCmd(this string cmd, string args, 
            Action<Process> before, 
            Action<string> stdout, 
            Action<string> stderr, 
            Action<Process> after)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = args;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;

                if (null != stdout)
                {
                    p.StartInfo.RedirectStandardOutput = true;
                    p.OutputDataReceived += (s, e) =>
                    {
                        if (null != e.Data)
                            stdout(e.Data);
                    };
                }

                if (null != stderr)
                {
                    p.StartInfo.RedirectStandardError = true;
                    p.ErrorDataReceived += (s, e) =>
                    {
                        if (null != e.Data)
                            stderr(e.Data);
                    };
                }

                p.Start();

                before?.Invoke(p);

                if (null != stdout)
                    p.BeginOutputReadLine();

                if (null != stderr)
                    p.BeginErrorReadLine();

                p.WaitForExit();

                after?.Invoke(p);

                return p.ExitCode;
            }
        }
    }
}
