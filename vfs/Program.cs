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
                var rsa = new RSACryptoServiceProvider(2048);

                var pubKey = rsa.ExportParameters(false);
                Console.WriteLine($"{VfsArgs.PubKey}{new { pubKey.Exponent, pubKey.Modulus }.json()}");

                var cipher = Console.ReadLine().b64();
                var repArgs = rsa.Decrypt(cipher, false).utf8();

                if (args.Length == 0)
                    throw new Error<VfsService>("EmptyArgs");

                var vfs = args[0].b64().utf8().obj<VfsArgs>();

                var rep = vfs.newRep(repArgs);

                Environment.ExitCode = new VfsService
                {
                    rep = rep,
                    path = vfs.path,
                    name = vfs.name,
                }.Run();
            }
            catch (Error err)
            {
                Console.Error.WriteLine(err.Json);
                return;
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.toError<VfsService>("Error").Json);
                return;
            }
        }
    }
}
