using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;
using util.rep;

namespace demo.crypt
{
    public class CtrGcmSpeedDemo : Test
    {
        public override void test()
        {
            encTest(2);
        }

        public void encTest(int blockSize)
        {
            //blockSize = blockSize * 1024;
            //$"\r\nblock size: {blockSize}".log();

            //var src = @"C:\test\rnd.data";
            //var dst = @"C:\test\dst.ctr";
            //var tagPath = @"C:\test\gcm.tag";
            //var buff = new byte[2*1024*1024];
            //var encKey = 32.aesKey();
            //var macKey = 32.aesKey();

            //var mac = new AesGcm { key = encKey };
            //var cipher = new byte[4*1024];

            //using (var fin = File.OpenRead(src))
            //using (var fout = File.OpenWrite(dst))
            //using (var cs = new AesCtrStream(fout, encKey, dst))
            //using (var mout = File.OpenWrite(tagPath))
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
            //        cs.Write(buff, 0, len);

            //        int pos = 0;
            //        while (pos < len)
            //        {
            //            var iv = 16.aesRnd();
            //            var tag = mac.encrypt(iv, buff, pos, blockSize, cipher, 0);
            //            mout.write(tag);

            //            pos += blockSize;
            //        }

            //        ctr.addSize(len);
            //    }
            //    ctr.trigger();
            //}

            //randomFile();
        }


        void randomFile()
        {
            var dst = @"C:\test\rnd.ctr";
            var buff = new byte[2 * 1024 * 1024];
            int count = 1024;
            var rnd = new Random();
            var ctr = new Counter { TotalSize = buff.Length * count };
            ctr.TimeIsUp += (t) =>
            {
                t.FullInfo.status();
            };
            ctr.trigger();
            using (var fout = File.Create(dst))
            {
                while (count-- > 0)
                {
                    rnd.NextBytes(buff);
                    fout.write(buff);
                    ctr.addSize(buff.Length);
                }
            }
            ctr.trigger();
        }
    }
}
