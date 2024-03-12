namespace demo.crypt
{
    //class HashFileDemo : Test
    //{
    //    public override void test()
    //    {
    //        {
    //            var path = @"C:\test\src.list";
    //            fullHash(path);
    //            sampleHash(path);
    //        }
    //        {
    //            var path = @"C:\test\dst.mac";
    //            fullHash(path);
    //            sampleHash(path);
    //        }
    //    }

    //    public void fullHash(string path)
    //    {
    //        path.msg();
    //        var hash = new Hash { mode = HashMode.Full };

    //        hash.getSpec().msg();
    //        using (var alg = hash.newAlg())
    //        {
    //            $"full hash multi times".@case();
    //            var code1 = compute(hash, alg, path);
    //            code1.showHex("code1");
    //            var code2 = compute(hash, alg, path);
    //            code2.showHex("code2");
    //            var code3 = compute(hash, alg, path);
    //            code3.showHex("code3");

    //            assert(code1.isSame(code2) && code2.isSame(code3));

    //            $"full hash vs full file stream hash".@case();
    //            var code4 = computeFile(hash, path);
    //            code4.showHex("code4");
    //            assert(code4.isSame(code1));
    //        }

    //        using (var alg = hash.newAlg())
    //        using (var fin = File.OpenRead(path))
    //        {
    //            var code2 = alg.ComputeHash(fin);
    //            code2.showHex("code2");
    //        }
    //    }

    //    public void sampleHash(string path)
    //    {
    //        var hash = new Hash { mode = HashMode.Sample };
    //        hash.getSpec().msg();
    //        using (var alg = hash.newAlg())
    //        {
    //            $"sample hash multi times".@case();
    //            var code1 = compute(hash, alg, path);
    //            code1.showHex("code1");
    //            var code2 = compute(hash, alg, path);
    //            code2.showHex("code2");
    //            var code3 = compute(hash, alg, path);
    //            code3.showHex("code3");

    //            assert(code1.isSame(code2) && code2.isSame(code3));
    //        }
    //    }

    //    byte[] compute(Hash hash, HashAlgorithm alg, string path)
    //    {
    //        using (var fin = File.OpenRead(path))
    //        {
    //            Counter ctr = new Counter { TotalSize = fin.Length };
    //            ctr.TimeIsUp += (t) => t.FullInfo.status();
    //            ctr.trigger();
    //            var code = hash.compute(fin, alg, ctr.addSize);
    //            ctr.trigger();

    //            return code;
    //        }
    //    }

    //    byte[] computeFile(Hash hash, string path)
    //    {
    //        using (var alg = hash.newAlg())
    //        using (var fin = File.OpenRead(path))
    //        {
    //            return alg.ComputeHash(fin);
    //        }
    //    }
    //}
}
