using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep.aead;

namespace test.util.rep
{
    public class AeadFsLongPath : ITest
    {
        public void test()
        {
            var dir = @"C:\test\t.xcha";
            var conf = AeadFsConf.loadByDir(dir);
            if (!conf.decrypt("".utf8()))
                throw new Exception("decrypt fail!!");

            conf.jsonIndent().msg();

            var rep = new AeadFsReposit(dir, conf);

            this.trydo(()=>rep.deleteDir("1"));

            this.trydo(() =>
            {
                var name = makeName(30);

                rep.createDir($"1/{name}");

                $"<success>".msg();
            });

            this.trydo(() =>
            {
                rep.createFile(makePath(24));

                $"<success>".msg();
            });

            this.trydo(() =>
            {
                rep.createFile(makePath(20));

                $"<success>".msg();
            });

            this.trydo(() =>
            {
                rep.createFile(makePath(17));

                $"<success>".msg();
            });

            this.trydo(() =>
            {
                rep.createFile(makePath(16, 10));

                $"<success>".msg();
            });
        }

        string makeName(int times)
        {
            var ten = "0123456789";

            var name = "";
            times.count(n => name = $"{name}{ten}");

            new { name, len = name.Length }.msgj();

            return name;
        }

        string makePath(int nameTimes, int pathTimes = 3)
        {
            var name = makeName(nameTimes);

            var path = "1/";
            pathTimes.count(n => path = $"{path}/{name}");

            new { path, pathLen = path.Length }.msgj();

            return path;
        }
    }
}
