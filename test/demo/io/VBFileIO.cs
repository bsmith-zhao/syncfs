using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.demo.io
{
    class VBFileIO : ITest
    {
        public void test()
        {
            FileSystem.DeleteFile("e:/test/abc.txt", 
                UIOption.OnlyErrorDialogs, 
                RecycleOption.SendToRecycleBin);
        }
    }
}
