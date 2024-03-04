using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util
{
    public static class PathEx
    {
        public static string pathOwner(this string path, int maxLevel, out int level)
        {
            var ns = path.pathSplit();
            ns = ns.head(maxLevel.min(ns.Length - 1));
            level = ns.Length;
            return ns.join("/");
        }

        public static string[] pathSplit(this string path)
            => path?.Split('/', '\\');

        public static string[] pathUnify(this string[] paths)
            => paths?.conv(p => p.pathUnify())
            .pick(p => p?.Length > 0).ToArray()
            .retain(arr => arr.Length > 0);

        public static string pathUnify(this string path)
            => path?.Split('/', '\\')
                .conv(nd => nd.Trim())
                .exclude(nd => nd.Length == 0).join("/")
                .keep(p => !p.empty());

        public static string pathDir(this string path)
        {
            if (null == path)
                return null;
            var pos = path.LastIndexOf("/");
            if (-1 == pos)
                return null;
            return path.Substring(0, pos);
        }

        public static string pathName(this string path)
        {
            if (null == path)
                return null;
            int pos = path.LastIndexOf('/');
            if (pos == -1)
                return path;
            return path.Substring(pos + 1, path.Length - pos - 1);
        }

        public static string pathTrunk(this string path)
        {
            if (null == path)
                return null;
            var pos = path.Length - 1;
            pos = path.LastIndexOf('.', pos, pos - path.LastIndexOf('/'));
            if (pos == -1)
                return path;
            return path.Substring(0, pos);
        }

        public static string pathExt(this string path)
        {
            if (null == path)
                return null;
            var pos = path.Length - 1;
            pos = path.LastIndexOf('.', pos, pos - path.LastIndexOf('/'));
            if (pos == -1)
                return null;
            return path.Substring(pos, path.Length - pos);
        }

        public static string pathMerge(this string dir, string name, 
            params string[] others)
            => others.Length == 0 
            ? $"{dir}/{name}" 
            : $"{dir}/{name}/{string.Join("/", others)}";
    }
}
