using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public class LDMaker
    {
        public static int distance(string src, string dst)
        {
            try
            {
                char[] str1 = src.ToCharArray();
                char[] str2 = dst.ToCharArray();
                int m = str1.Length;
                int n = str2.Length;
                int[,] d = new int[m + 1, n + 1];
                for (int i = 0; i <= m; i++)
                    d[i, 0] = i;
                for (int i = 0; i <= n; i++)
                    d[0, i] = i;
                for (int i = 1; i <= m; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        d[i, j] = d[i - 1, j - 1] + (str1[i - 1] == str2[j - 1] ? 0 : 1);
                        //修改一个字符
                        d[i, j] = Math.Min(d[i, j], d[i - 1, j] + 1);
                        // 插入一个字符串
                        d[i, j] = Math.Min(d[i, j], d[i, j - 1] + 1);
                        //删除一个字符
                    }
                }
                return d[m, n];
            }
            catch
            {
                return -1;
            }
        }

        public static double similar(string src, string dst)
        {
            var max = Math.Max(src.Length, dst.Length);
            if (max == 0)
            {
                return 1;
            }
            var ld = distance(src, dst);
            if (ld >= 0 && ld <= max)
            {
                var rt = (max - ld) * 1.0 / max;
                return rt;
            }
            return 0;
        }
    }
}
