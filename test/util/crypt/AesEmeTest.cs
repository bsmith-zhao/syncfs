using System.IO;
using util.ext;

namespace util.crypt
{
    public class AesEmeTest : Test
    {
        public override void test()
        {
            //test16();
            //test2048();
        }

        //void test16()
        //{
        //    $"{nameof(test16)}".@case();

        //    var key = new byte[32].set<byte>(0x00);
        //    var iv = new byte[16].set<byte>(0x00);
        //    var plain = new byte[16].set<byte>(0x00);
        //    var cipher = key.emeEnc(plain, iv);
        //    var verify = "f1b9ce8ca15a4ba9fb476905434b9fd3".hex();
        //    assert(cipher.same(verify));
        //}

        //void test2048()
        //{
        //    $"{nameof(test2048)}".@case();

        //    var key = new byte[32].set<byte>(0x00);
        //    var iv = new byte[16].set<byte>(0x00);
        //    var plain = new byte[2048].set<byte>(0x00);
        //    var cipher = key.emeEnc(plain, iv);
        //    var verify = File.ReadAllText(@"C:\prj\SyncFavor\doc\aes-eme-2048.txt").Replace("\r","").Replace("\n", "").hex();
        //    assert(cipher.same(verify));
        //}
    }
}
