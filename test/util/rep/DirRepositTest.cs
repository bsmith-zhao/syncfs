using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.rep;

namespace test
{
    public class DirRepositTest : Test
    {
        public override void test()
        {
            var dir = @"C:\test\dir.test".pathUnify();
            if(Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            var rep = new NormalDirReposit(dir);
            new RepositTest
            {
                rep = rep
            }.test();
        }
    }
}
