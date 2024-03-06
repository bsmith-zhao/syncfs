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
    public class Lottery : ITest
    {
        public void test()
        {
            50.count().each(i =>
            {
                $"[{format(i + 1)}]  {format(unionLotto())}".msg();
            });

            50.count().each(i =>
            {
                $"[{format(i+1)}]  {format(superLotto())}".msg();
            });
        }

        string format(int b)
            => b < 10 ? $" {b}" : $"{b}";

        string format(int[] bet)
            => bet.conv(format).join("  ");

        int[] unionLotto() => doubleColor(33, 16, 6);
        int[] superLotto() => doubleColor(35, 12, 5);

        int[] doubleColor(int redTotal, int blueTotal, int redCount, int betCount = 7)
        {
            var reds = redTotal.count().ToList();
            var blues = blueTotal.count().ToList();
            var rnd = new byte[betCount].aesRnd();
            var bet = new int[rnd.Length];
            rnd.each((i, r) =>
            {
                if (i < redCount)
                {
                    var idx = r % reds.Count;
                    bet[i] = reds[idx] + 1;

                    reds.RemoveAt(idx);
                }
                else
                {
                    var idx = r % blues.Count;
                    bet[i] = blues[idx] + 1;

                    blues.RemoveAt(idx);
                }
            });
            return bet;
        }
    }
}
