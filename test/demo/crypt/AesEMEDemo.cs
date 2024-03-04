using util;

namespace test.demo
{
    public class AesEMEDemo : Test
    {
        public override void test()
        {
            //prefixTest();

            //cfsMasterKey();


            
        }

        //public void cfsMasterKey()
        //{
        //    var mk = "VICD4flfvXGVS+mMnaqCOQoSxYYYo5E1/omfx0CX8hn6k36her/BEtCbBmGppZPTfvKfGbwM1QHp9NxlJtyfFg==".b64();
        //    $"{mk.Length}".log();

        //    var salt = "ltRc5cFhIBgakF4uo1++p/9ZCa5CIZceBggzCI4aSos=".b64();
        //    $"{salt.Length}".log();
        //}

        //public void prefixTest()
        //{
        //    var f1 = @"C:\prj\\vdisk\aesRnd\SyncFavor".utf8().round(16);
        //    var f2 = @"C:\prj\SyncFavor\new byte[32]".utf8().round(16);

        //    //$"{}".log();
        //    $"f1: {f1.hex()}".log();
        //    $"f2: {f2.hex()}".log();

        //    var key = new byte[32].aesRnd();
        //    var iv = new byte[16].aesRnd();

        //    var c1 = key.emeEnc(f1, iv);
        //    var c2 = key.emeEnc(f2, iv);

        //    $"c1: {c1.hex()} / {c1.b64()}".log();
        //    $"c2: {c2.hex()} / {c2.b64()}".log();
        //}

        //public void fireTest()
        //{
        //    var key = new byte[32].aesRnd();
        //    var plain = new byte[32].aesRnd();
        //    var iv = new byte[16].aesRnd();

        //    $"key: {key.hex()}".log();
        //    $"plain: {plain.hex()}".log();
        //    $"iv: {iv.hex()}".log();

        //    var cipher = key.emeEnc(plain, iv);

        //    $"cipher: {cipher.hex()}".log();

        //    var decPlain = key.emeDec(cipher, iv);

        //    $"decPlain: {plain.hex()}".log();
        //}
    }
}
