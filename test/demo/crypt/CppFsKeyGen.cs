using util;

namespace test.demo
{
    public class AesGCMDemo : Test
    {
        

        public override void test()
        {
            //testHkdf();
            //test1();
        }

        //public void showBytes(string name, byte[] data)
        //{
        //    $"{name} size: {data.Length}, {name}: {data.hex()}".log();
        //}

        //public  void test1()
        //{
        //    var salt = "gzNtJZhwzirVlEZxtVqekNuCQJgtbuyq5ij34POhX5Y=".b64();

        //    var pwd = "1".utf8();
        //    showBytes("pwd", pwd);

        //    var key = new Rfc7914DeriveBytes(pwd, salt, 8, 1, 65536).GetBytes(32);
        //    showBytes("key", key);

        //    var dfKey = key.hkdf(null, "AES-GCM file content encryption".utf8());
        //    showBytes("dfKey", dfKey);

        //    var ek = "aYuCp1mOk16sru4nOZzDu3r93fKfo9DCYNeSziQ9sK+9q4qMEfuCJVQp9irn7i29pHU6FJqw6ZEwJNts2lSxNw==".b64();
        //    showBytes("ek", ek);
 
        //    var iv = ek.first(16);
        //    showBytes("iv", iv);
        //    var cipher = ek.sub(16, 48);
        //    showBytes("cipher", cipher);
        //    var tag = ek.sub(48, 16);
        //    showBytes("tag", tag);

        //    var decData = dfKey.gcmDec(cipher, iv);
        //    showBytes("decData", decData);
        //}

        //public void testHkdf()
        //{
        //    var master0 = new byte[32].set<byte>(0x00, 0, 32);
        //    var master1 = new byte[32].set<byte>(0x01, 0, 32);

        //    var out1 = "9ba3cddd48c6339c6e56ebe85f0281d6e9051be4104176e65cb0f8a6f77ae6b4";
        //    var out2 = "e8a2499f48700b954f31de732efd04abce822f5c948e7fbc0896607be0d36d12";
        //    var out3 = "9137f2e67a842484137f3c458f357f204c30d7458f94f432fa989be96854a649";
        //    var out4 = "0bfa5da7d9724d4753269940d36898e2c0f3717c0fee86ada58b5fd6c08cc26c";

        //    var out3Key = master1.hkdf(null, "AES-GCM file content encryption".utf8());
        //    showBytes("out3Key", out3Key);
        //    assert(out3Key.hex().ToLower() == out3);
        //}
    }

    //public static class BCUtils
    //{
    //    public static byte[] hkdf(this byte[] key, byte[] salt, byte[] info)
    //    {
    //        var df = new HkdfBytesGenerator(new Sha256Digest());
    //        df.Init(new HkdfParameters(key, salt, info));
    //        byte[] newKey = new byte[32];
    //        df.GenerateBytes(newKey, 0, newKey.Length);

    //        return newKey;
    //    }

    //    public static byte[] gcmEnc(this byte[] key, byte[] plain, byte[] iv)
    //    {
    //        var dec = new GcmBlockCipher(new AesEngine());
    //        var args = new AeadParameters(new KeyParameter(key), 128, iv);
    //        dec.Init(true, args);

    //        var cipher = new byte[plain.Length+16];
    //        var len = dec.ProcessBytes(plain, 0, plain.Length, cipher, 0);
    //        dec.DoFinal(cipher, len);

    //        return cipher;
    //    }

    //    public static byte[] gcmDec(this byte[] key, byte[] src, byte[] iv)
    //    {
    //        var dec = new GcmBlockCipher(new AesEngine());
    //        var ad = new byte[8].set<byte>(0x00, 0, 8);
    //        var args = new AeadParameters(new KeyParameter(key), 128, iv, ad);
    //        dec.Init(false, args);

    //        var dst = new byte[dec.GetOutputSize(src.Length)];
    //        $"out size: {dst.Length}".log();
    //        var len = dec.ProcessBytes(src, 0, src.Length, dst, 0);
    //        $"out len: {len}".log();
    //        dec.DoFinal(dst, len);

    //        return dst;
    //    }
    //}
}
