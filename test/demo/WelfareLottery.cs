using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;

namespace test.demo
{
    public class WelfareLottery : ITest
    {
        public void test()
        {
            25.count().each(i =>
            {
                $"{i+1} {unionLotto().json()}".msg();
            });

            25.count().each(i =>
            {
                $"{i + 1} {superLotto().json()}".msg();
            });
        }

        int[] unionLotto()
        {
            var reds = 33.count().ToList();
            var blues = 16.count().ToList();
            var rnd = new byte[7].aesRnd();
            var bet = new int[rnd.Length];
            rnd.each((i, r) =>
            {
                if (i < 6)
                {
                    var idx = r % reds.Count;
                    bet[i] = reds[idx] + 1;

                    reds.RemoveAt(idx);
                }
                else
                {
                    var idx = r % blues.Count;
                    bet[i] = blues[idx] + 1;
                }
            });
            return bet;
        }

        int[] superLotto()
        {
            var reds = 35.count().ToList();
            var blues = 12.count().ToList();
            var rnd = new byte[7].aesRnd();
            var bet = new int[rnd.Length];
            rnd.each((i, r) =>
            {
                if (i < 5)
                {
                    var idx = r % reds.Count;
                    bet[i] = reds[idx] + 1;

                    reds.RemoveAt(idx);
                }
                else
                {
                    var idx = r % blues.Count;
                    bet[i] = blues[idx] + 1;
                }
            });
            return bet;
        }
    }
}
