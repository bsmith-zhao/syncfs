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
            => Process.GetProcesses()
                .conv(p => 
                {
                    try
                    {
                        var vfs = App.Option.VfsExe.pathName().pathTrunk();
                        if (!p.ProcessName.Contains(vfs))
                            return null;
                        var c = p.cmd();
                        var idx = c.IndexOf("\"", 1);
                        var exe = c.Substring(1, idx - 1);
                        if (exe.pathName() != App.Option.VfsExe)
                            return null;
                        return new MountInfo
                        {
                            args = c.jump(idx + 1).Trim().b64().utf8()
                                .obj<VfsArgs>(),
                            proc = p,
                        };
                    }
                    catch(Exception err)
                    {
                        new { f="error", err.Message}.msgj();
                    }
                    return null;
                }).exclude(e => e == null);
    }
}
