using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;

namespace test.util
{
    public class NumberTest : ITest
    {
        public void test()
        {
            {
                int a = 2, b = 3;
                new { a, b, atLeast = a.atLeast(b) }.debug();
            }
            {
                int a = 3, b = 2;
                new { a, b, atLeast = a.atLeast(b) }.debug();
            }
            {
                int a = 2, b = 2;
                new { a, b, atLeast = a.atLeast(b) }.debug();
            }
            {
                int a = 2, b = 2;
                new { a, b, atMost = a.atMost(b) }.debug();
            }
            {
                int a = 2, b = 3;
                new { a, b, atMost = a.atMost(b) }.debug();
            }
            {
                int a = 3, b = 2;
                new { a, b, atMost = a.atMost(b) }.debug();
            }
        }
    }
}
