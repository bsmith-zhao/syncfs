using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace vfs.mgr
{
    public class MountInfo
    {
        public VfsArgs args;
        public Process proc;

        public string path => args.path;
        public string name => args.name;
        public RepType type => args.type;
        public string src => args.src;

        public string info => $"{path} <{name}>";

        public static IEnumerable<MountInfo> enumMounts()
        {
            var vfsExe = App.Option.VfsExe.low();
            var vfsName = vfsExe.pathName().pathTrunk();
            return Process.GetProcesses()
                .conv(p =>
                {
                    try
                    {
                        if (!p.ProcessName.low().Contains(vfsName))
                            return null;
                        var c = p.cmd();
                        var idx = c.IndexOf("\"", 1);
                        var exe = c.Substring(1, idx - 1);
                        if (exe.pathName().low() != vfsExe)
                            return null;
                        return new MountInfo
                        {
                            args = c.jump(idx + 1).Trim().b64().utf8()
                                .obj<VfsArgs>(),
                            proc = p,
                        };
                    }
                    catch (Exception err)
                    {
                        err.log(nameof(enumMounts));
                    }
                    return null;
                }).exclude(p => p == null);
        }
    }
}
