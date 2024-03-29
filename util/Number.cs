﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public static class Number
    {
        public const long KB = 1024;
        public const long MB = KB * KB;
        public const long GB = MB * KB;
        public const long TB = GB * KB;

        public static string[] ByteUnits = { "", "K", "M", "G", "T" };

        public static string byteSize(this int size)
            => ((long)size).byteSize();

        public static string byteSize(this long size, int point = -1)
        {
            if (size <= KB)
                return $"{size}{ByteUnits[0]}";

            int ui = 0;
            double num = size * 1.0;
            while (num > 1000 && ui < ByteUnits.Length - 1)
            {
                num = num / KB;
                ui++;
            }
            if (point < 0)
                return num.ToString("0.##") + ByteUnits[ui];
            else
                return num.ToString($"F{point}") + ByteUnits[ui];
        }

        public static bool isByteSize(this string str, out long unit)
        {
            unit = 1;
            switch (char.ToUpper(str[str.Length - 1]))
            {
                case 'K': unit = KB; break;
                case 'M': unit = MB; break;
                case 'G': unit = GB; break;
                case 'T': unit = TB; break;
            }
            return unit > 1;
        }

        public static long byteSize(this string str, long @default = 0)
        {
            str = str?.Trim();
            if (str?.Length > 0)
            {
                if (isByteSize(str, out var unit))
                    str = str.Substring(0, str.Length - 1).Trim();

                if (double.TryParse(str, out var size))
                    return (long)(size * unit);
            }
            return @default;
        }

        public static string p100(this int value)
        {
            return (value >= 0) ? $"{value}%" : "NA%";
        }

        public static int r100(this long value, long total)
        {
            return total <= 0 ? 0 : (int)(value * 100 / total);
        }

        public static T atMost<T>(this T value, T max) 
            where T : IComparable
            => value.CompareTo(max) > 0 ? max : value;

        public static T atLeast<T>(this T value, T min)
            where T : IComparable
            => value.CompareTo(min) < 0 ? min : value;

        public static T limit<T>(this T value, T min, T max)
            where T : IComparable
            => value.atLeast(min).atMost(max);

        public static byte[] bytes(this long value)
        {
            return BitConverter.GetBytes(value);
        }

        public static unsafe byte[] copyTo(this long value, 
            byte[] dst, int dstOff = 0)
        {
            if (dstOff < 0)
                dstOff = dst.Length + dstOff;
            fixed (byte* p = dst)
            {
                *((long*)(p + dstOff)) = value;
            }
            return dst;
        }

        public static long i64(this byte[] data, int offset = 0)
        {
            return BitConverter.ToInt64(data, offset);
        }

        public static double f64(this string str, double @default = 0)
            => double.TryParse(str, out var v) ? v : @default;

        public static byte[] bytes(this int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] bytes(this short value)
        {
            return BitConverter.GetBytes(value);
        }

        public static int i32(this byte[] data, int offset = 0)
        {
            return BitConverter.ToInt32(data, offset);
        }

        public static int i32(this string str, int @default = 0)
            => int.TryParse(str, out var v) ? v : @default;

        public static int i16(this byte[] data, int offset = 0)
        {
            return BitConverter.ToInt16(data, offset);
        }

        public static int i16(this string text, int n = 10)
        {
            return Convert.ToInt16(text, n);
        }

        public static bool inside<T>(this T src, T min, T max)
            where T : IComparable
            => src.CompareTo(min) >= 0 
            && src.CompareTo(max) <= 0;

        public static string hex(this int value, int len)
            => value.ToString($"X{len}");

        public static int abs(this int value) => Math.Abs(value);

        public static int kb(this int value)
            => value * 1024;

        public static int mb(this int value)
            => value * 1024 * 1024;

        public static double num(this string str)
        {
            if (double.TryParse(str, out var value))
                return value;
            else if (str.isByteSize(out var unit))
                return str.byteSize();
            return 0;
        }

        public static bool markBy(this int flag, int mark)
            => (flag & mark) == mark;

        public static bool markBy(this uint flag, uint mark)
            => (flag & mark) == mark;

        public static int slice(this int total,
            int limit,
            Func<int, int> adjust, Action<int> proc)
        {
            int remain = total;
            int actual;
            while (remain > 0
                && (actual = adjust(remain.atMost(limit))) > 0)
            {
                proc.Invoke(actual);
                remain -= actual;
            }
            return total - remain;
        }

        public static void slice(this int total,
            int limit, Action<int> proc)
            => slice((long)total, limit, proc);

        public static void slice(this long total,
            int limit, Action<int> proc)
        {
            int unit;
            while (total > 0)
            {
                unit = (int)total.atMost(limit);
                proc(unit);
                total -= unit;
            }
        }
    }
}
