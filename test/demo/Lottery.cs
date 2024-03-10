using System;
using System.Collections.Generic;
using System.IO;
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
            // 0306
            //superCheck(check("d:/note/5+2.0306.txt",
            //    new int[] { 1, 2, 8, 18, 27 },
            //    new int[] { 4, 8 }));

            // 0309
            superCheck(check("d:/note/5+2.0309.txt",
                new int[] { 1, 18, 21, 26, 33 },
                new int[] { 2, 12 }));

            //unionCheck(check("e:/note/6+1.txt",
            //    new int[] { 8, 15, 21, 22, 25, 33 },
            //    new int[] { 13 }));

            //50.count().each(i =>
            //{
            //    $"[{format(i + 1)}]  {format(unionLotto())}".msg();
            //});

            //50.count().each(i =>
            //{
            //    $"[{format(i+1)}]  {format(superLotto())}".msg();
            //});
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

        IEnumerable<object[]> check(string path, int[] reds, int[] blues)
        {
            var rs = reds.ToHashSet();
            var bs = blues.ToHashSet();

            //var path = "e:/note/5+2.txt";
            var rows = File.ReadAllText(path).Split('\r', '\n');
            var cks = rows.exclude(r => r.Length < 5).conv(r => r.tail(26)).conv(r => 
            {
                var ck = new int[2];
                r.Split(' ').exclude(e => e.empty())
                .conv(s => s.i32()).ToArray()
                .each((i, n) => 
                {
                    if (i < reds.Length)
                    {
                        if (reds.Contains(n))
                            ck[0]++;
                    }
                    else
                    {
                        if (blues.Contains(n))
                            ck[1]++;
                    }
                });
                return new object[] { ck, r };
            });
            return cks;
        }

        void superCheck(IEnumerable<object[]> cks)
        {
            foreach (var ck in cks)
            {
                var c = ck[0] as int[];
                var r = ck[1] as string;
                if (c[0] == 5 && c[1] == 2)
                    new { c, s = 1, r }.json().msg();
                else if (c[0] == 5 && c[1] == 1)
                    new { c, s = 2, r }.json().msg();
                else if (c[0] == 5 && c[1] == 0)
                    new { c, s = 3, r }.json().msg();
                else if (c[0] == 4 && c[1] == 2)
                    new { c, s = 4, r }.json().msg();
                else if (c[0] == 4 && c[1] == 1)
                    new { c, s = 5, r }.json().msg();
                else if (c[0] == 3 && c[1] == 2)
                    new { c, s = 6, r }.json().msg();
                else if (c[0] == 4 && c[1] == 0)
                    new { c, s = 7, r }.json().msg();
                else if (c[0] == 3 && c[1] == 1)
                    new { c, s = 8, r }.json().msg();
                else if (c[0] == 2 && c[1] == 2)
                    new { c, s = 8, r }.json().msg();
                else if (c[0] == 3 && c[1] == 0)
                    new { c, s = 9, r }.json().msg();
                else if (c[0] == 1 && c[1] == 2)
                    new { c, s = 9, r }.json().msg();
                else if (c[0] == 2 && c[1] == 1)
                    new { c, s = 9, r }.json().msg();
                else if (c[0] == 0 && c[1] == 2)
                    new { c, s = 9, r }.json().msg();
                //else
                //    new { c, s = 0, r }.json().msg();
            }
        }

        void unionCheck(IEnumerable<object[]> cks)
        {
            foreach (var ck in cks)
            {
                var c = ck[0] as int[];
                var n = ck[1] as string;

                //new { c, s = 1, n }.json().msg();
                var r = c[0];
                var b = c[1];
                if (r == 6 && b == 1)
                    new { c, s = 1, n }.json().msg();
                else if (r == 6 && b == 0)
                    new { c, s = 2, n }.json().msg();
                else if (r == 5 && b == 1)
                    new { c, s = 3, n }.json().msg();
                else if (r == 5 && b == 0)
                    new { c, s = 4, n }.json().msg();
                else if (r == 4 && b == 1)
                    new { c, s = 4, n }.json().msg();
                else if (r == 4 && b == 0)
                    new { c, s = 5, n }.json().msg();
                else if (r == 3 && b == 1)
                    new { c, s = 5, n }.json().msg();
                else if (r == 2 && b == 1)
                    new { c, s = 6, n }.json().msg();
                else if (r == 1 && b == 1)
                    new { c, s = 6, n }.json().msg();
                else if (r == 0 && b == 1)
                    new { c, s = 6, n }.json().msg();
            }
        }
    }
}
