using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace vfs.mgr
{
    public static class VfsManagerEx
    {
        public static string cmd(this Process proc)
        {
            using (var cond = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + proc.Id))
            using (var objs = cond.Get())
            {
                return objs.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
            }
        }
    }
}
