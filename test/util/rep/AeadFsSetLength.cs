using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep.aead;

namespace test.util.rep
{
    public class AeadFsSetLength : ITest
    {
        Stream fs;
        public void test()
        {
            var dir = @"C:\test\t.xcha";
            var conf = AeadFsConf.loadByDir(dir);
            if (!conf.decrypt("".utf8()))
                throw new Exception("decrypt fail!!");

            conf.jsonIndent().msg();

            var rep = new AeadFsReposit(dir, conf);

            var path = "abc";
            if (rep.exist(path))
                rep.deleteFile(path);

            using (var fs = rep.createFile(path))
            {
                this.fs = fs;

                new { fs.Length, fs.Position }.msgj();

                fs.SetLength(20);
                new { fs.Length, fs.Position }.msgj();

                fs.SetLength(128);
                new { fs.Length, fs.Position }.msgj();

                fs.SetLength(conf.BlockSize*5);
                new { fs.Length, fs.Position }.msgj();

                readAll();

                new { fs.Length, fs.Position }.msgj();

                fs.SetLength(123);
                new { fs.Length, fs.Position }.msgj();

                fs.SetLength(0);
                new { fs.Length, fs.Position }.msgj();

                //fs.SetLength(123);

                fs.SetLength(conf.BlockSize * 5);
                new { fs.Length, fs.Position }.msgj();

                var begin = new DateTime();
                new { begin }.msgj();
                fs.SetLength(1000L*1024*1024);
                new { cost = (new DateTime() - begin) }.msgj();
                new { fs.Length, fs.Position }.msgj();
            }
        }

        void readAll()
        {
            fs.Position = 0;
            byte[] buff = new byte[4287];
            int len;
            int total = 0;
            while ((len = fs.readFull(buff)) > 0)
            {
                total += len;
                new { total, len, head=buff.head(16).hex() }.msgj();
            }
        }
    }
}
