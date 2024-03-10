using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public class FileLogger : IDisposable
    {
        StreamWriter fout;

        public FileLogger(string path = null,
            Action<Exception> error = null,
            int maxSize = 5 * 1024 * 1024)
        {
            try
            {
                path = path ?? $"{true.appTrunk()}.log";

                var fi = new FileInfo(path);
                if (fi.Exists && fi.Length >= maxSize)
                {
                    var bakPath = $"{path}1";
                    if (File.Exists(bakPath))
                        File.Delete(bakPath);
                    fi.MoveTo(bakPath);
                }
                fout = File.AppendText(path);
                Log.output = this.output;
            }
            catch (Exception err)
            {
                error?.Invoke(err);
            }
        }

        public void Dispose()
        {
            true.free(ref fout);
        }

        void output(string msg)
        {
            try
            {
                fout.WriteLine(msg);
                fout.Flush();
            }
            catch (Exception err)
            {
                $"[{nameof(FileLogger)}]{err.Message}".msg();
            }
        }
    }
}
