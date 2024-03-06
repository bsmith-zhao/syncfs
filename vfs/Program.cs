using System;
using System.Collections;
using System.Security.Cryptography;
using util;
using util.crypt.sodium;
using util.ext;

namespace vfs
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Msg.output = Console.WriteLine;

                if (args.Length == 0)
                    throw new Error<VfsService>("EmptyArgs");

                var vfs = args[0].b64().utf8().obj<VfsArgs>();
                if (vfs.path.fsExist())
                    throw new Error<VfsService>("PathExist", vfs.path);

                var logName = vfs.path.Replace(":", "#")
                                        .Replace("\\", "%")
                                        .Replace("/", "%");
                var logPath = $"{true.appDir()}/log/vfs@{logName}.log";
                logPath.pathDir().dirCreate();
                new FileLogger(logPath, error: err => err.msg());

                var rsa = new RSACryptoServiceProvider(2048);

                var pubKey = rsa.ExportParameters(false);
                Console.WriteLine($"{VfsArgs.PubKey}{new { pubKey.Exponent, pubKey.Modulus }.json()}");

                var cipher = Console.ReadLine().b64();
                var repArgs = rsa.Decrypt(cipher, false).utf8();

                var rep = vfs.newRep(repArgs);

                new VfsService
                {
                    rep = rep,
                    mount = vfs.path,
                    label = vfs.name,
                }.Run();
            }
            catch (Error err)
            {
                Console.Error.WriteLine(err.Json);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.toError<VfsService>("Error").Json);
            }
        }
    }
}
