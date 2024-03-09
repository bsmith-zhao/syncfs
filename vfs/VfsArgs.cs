using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;
using vfs.rep;

namespace vfs
{
    public class VfsArgs
    {
        public const string PubKey = "<PubKey>";

        public string path = "V:";
        public string name = "VFS";
        public RepType type;
        public string src;
        public string bak = null;

        public Reposit newRep(string args)
            => (args.obj(argsClass()) as RepArgs).newRep();

        Type argsClass()
        {
            switch (type)
            {
                case RepType.NormalDir:
                    return typeof(NormalDirArgs);
                case RepType.AeadFS:
                    return typeof(AeadFsArgs);
            }
            return null;
        }

        public void mount(string vfsExe, RepArgs repArgs,
            Action<Process> before = null, 
            Action<Process> after = null,
            Action<string> stdout = null,
            Action<string> stderr = null)
        {
            Process proc = null;
            vfsExe.runCmdAwait(this.json().utf8().b64(), before: p=> 
            {
                proc = p;
                before?.Invoke(p);
            }, after: after,
            stdout: str => 
            {
                if (str.StartsWith(PubKey))
                {
                    var pubKey = str.jump(PubKey.Length).obj<RSAParameters>();
                    var rsa = new RSACryptoServiceProvider();
                    rsa.ImportParameters(pubKey);
                    var cipher = rsa.Encrypt(repArgs.json().utf8(), false);
                    proc.StandardInput.WriteLine(cipher.b64());
                }
                else
                    stdout?.Invoke(str);
            }, stderr: err => 
            {
                if (stderr != null)
                {
                    err = true.trygetQuiet(err.obj<ErrorJson>).str() ?? err;
                    stderr?.Invoke(err);
                }
            });
        }
    }
}
