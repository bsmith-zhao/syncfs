using System.IO;
using util;
using util.crypt;
using util.ext;

namespace demo.crypt
{
    public class ChaCha20Poly1305Speed : Test
    {
        public override void test()
        {
            encTest(4);
        }

        public void encTest(int blockSize)
        {
            //blockSize = blockSize * 1024;
            //$"\r\nblock size: {blockSize}".log();

            //var src = @"C:\test\rnd.data";
            //var dst = @"C:\test\cha.out";
            //var buff = new byte[2 * 1024 * 1024];
            //var encKey = 32.aesKey();
            //var iv = 12.aesRnd();
            //var tag = new byte[16];
            //var plain = blockSize.aesRnd();
            //var cipher = new byte[plain.Length];

            //var cha = new ChaCha20Poly1305(encKey);

            //using (var fin = File.OpenRead(src))
            //using (var fout = File.Create(dst))
            //{
            //    var ctr = new Counter { TotalSize = fin.Length };
            //    ctr.TimeIsUp += (t) =>
            //    {
            //        t.FullInfo.stat();
            //    };
            //    ctr.trigger();
            //    int len;
            //    while (fin.read(buff, out len))
            //    {
            //        int pos = 0;
            //        while (pos < len)
            //        {
            //            cha.Encrypt(iv, plain, cipher, tag);

            //            pos += blockSize;
            //        }

            //        ctr.addSize(len);
            //    }
            //    ctr.trigger();
            //}
        }
    }
}
