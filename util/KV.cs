using util.ext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace util
{
    public static class KV
    {
        public static Dictionary<string, string> kvLoad(this string path)
        {
            using (var fin = File.OpenText(path))
            {
                return kvLoad(fin);
            }
        }

        public static Dictionary<string, string> kvLoad(this StreamReader fin)
        {
            var map = new Dictionary<string, string>();
            string row, key, value;
            int idx;
            while ((row = fin.ReadLine()) != null)
            {
                idx = row.IndexOf("=");
                if (idx >= 0)
                {
                    value = row.Substring(idx + 1).Trim();
                    if (value.Length > 0)
                    {
                        value = value.Replace("\\n", "\r\n")
                            .Replace("\\t", "\t");
                        key = row.Substring(0, idx).Trim();
                        map[key] = value;
                    }
                }
            }
            return map;
        }
    }
}
