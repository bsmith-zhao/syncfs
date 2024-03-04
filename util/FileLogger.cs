using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util.ext;

namespace util
{
    public class FileLogger : IDisposable
    {
        StreamWriter writer;

        public FileLogger(string path = null, 
            int maxSize = 5 * 1024 * 1024)
        {
            try
            {
                path = path ?? $"{this.appTrunk()}.log";

                var fi = new FileInfo(path);
                if (fi.Exists && fi.Length >= maxSize)
                {
                    var bakPath = $"{path}1";
                    if (File.Exists(bakPath))
                        File.Delete(bakPath);
                    fi.MoveTo(bakPath);
                }
                writer = File.AppendText(path);
                Log.output = this.output;
            }
            catch (Exception err)
            {
                MessageBox.Show($"Fail to open log [{path}]: {err.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Dispose()
        {
            this.free(ref writer);
        }

        public void output(params string[] msgs)
        {
            try
            {
                foreach (var msg in msgs)
                    writer.WriteLine(msg);
                writer.Flush();
            }
            catch (Exception err)
            {
                $"[{nameof(FileLogger)}]{err.Message}".msg();
            }
        }
    }
}
