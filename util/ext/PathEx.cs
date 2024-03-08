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
        public static string pathRoot(this string path)
            => path.pathSplit().first(s => s.Length > 0);

        public static string pathBackup(this string path,
            int total, Func<string, bool> exist,
            Action<string> delete, string sep = null)
        {
            total = total.atLeast(2);
            var trk = path.pathTrunk();
            var ext = path.pathExt();
            string newPath = null;
            int idx = 0;
            while (idx++ < total)
            {
                newPath = makePath(trk, sep, idx, ext);
                if (!exist(newPath))
                    break;
            }
            if (idx > total)
            {
                idx = 1;
                newPath = makePath(trk, sep, idx, ext);
                delete(newPath);
            }
            var delPath = makePath(trk, sep, idx == total ? 1 : (idx + 1), ext);
            if (exist(delPath))
                delete(delPath);

            return newPath;
        }

        static string makePath(string trk, string sep, int idx, string ext)
            => $"{trk}{sep}{idx}{ext}";

        public static void pathPush(this string path, int cnt,
            Func<string, bool> exist, Action<string> delete,
            Action<string, string> move, string sep = null)
        {
            var trk = path.pathTrunk();
            var ext = path.pathExt();
            var lastPath = $"{trk}{sep}{cnt}{ext}";
            if (exist(lastPath))
                delete(lastPath);
            while (cnt > 1)
            {
                var bakPath = $"{trk}{sep}{cnt - 1}{ext}";
                if (exist(bakPath))
                    move(bakPath, $"{trk}{sep}{cnt}{ext}");
                cnt--;
            }
            if (exist(path))
                move(path, $"{trk}{sep}{1}{ext}");
        }

        public static string pathSettle(this string path,
            Func<string, bool> exist, string sep = null)
        {
            if (!exist(path))
                return path;
            return path.pathAppend(exist, sep);
        }

        public static string pathAppend(this string path,
            Func<string, bool> exist, string sep = null)
        {
            var trk = path.pathTrunk();
            var ext = path.pathExt();
            int idx = 1;
            while (idx < 1000)
            {
                var newPath = makePath(trk, sep, idx++, ext);
                if (!exist(newPath))
                    return newPath;
            }
            throw new Error(typeof(Path), "AppendOverflow", idx);
        }

        public static string pathOwner(this string path, int maxLevel, out int level)
        {
            var ns = path.pathSplit();
            ns = ns.head(maxLevel.atMost(ns.Length - 1));
            level = ns.Length;
            return ns.join("/");
        }

        public static string[] pathSplit(this string path)
            => path?.Split('/', '\\');

        public static string[] pathUnify(this string[] paths)
            => paths?.conv(p => p.pathUnify())
            .pick(p => p?.Length > 0).ToArray()
            .remain(arr => arr.Length > 0);

        public static string pathUnify(this string path)
            => path?.Split('/', '\\')
                .conv(nd => nd.Trim())
                .exclude(nd => nd.Length == 0).join("/")
                .discard(p => p.empty());

        public static string pathUnifyName(this string path)
            => path?.Trim().Replace("\\", "-")
            .Replace("/", "-").discard(s => s.empty());

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
