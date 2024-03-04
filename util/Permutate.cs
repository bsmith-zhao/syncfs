using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public class Permutate
    {
        public static void full(int count, Action<int[]> func)
        {
            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = i;
            }
            full(nums, func);
        }

        public static void full(int[] nums, Action<int[]> func)
        {
            full(nums, 0, nums.Length, func);
        }

        private static void full(int[] nums, int m, int n, Action<int[]> func)
        {
            int i, t;
            if (m < n - 1)
            {
                full(nums, m + 1, n, func);
                for (i = m + 1; i < n; i++)
                {
                    t = nums[m];
                    nums[m] = nums[i];
                    nums[i] = t;
                    //
                    full(nums, m + 1, n, func);
                    //
                    t = nums[m];
                    nums[m] = nums[i];
                    nums[i] = t;
                }
            }
            else
            {
                func?.Invoke(nums);
            }
        }
    }
}
