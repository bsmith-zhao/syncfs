using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace link
{
    public static class LinkViewEx
    {
        public static bool isLinked(this IItem a, IItem b)
            => a is INode na
            && na.Links.exist(lk => lk.linkedTo(b));

        public static bool linkedTo(this ILink lk, IItem it)
            => lk.Source == it || lk.Target == it;

        public static HashSet<Link> allLinks(this INode nd, 
                                            Func<ILink, bool> cond)
            => allLinks(nd, new HashSet<Link>(), cond);

        public static HashSet<Link> allLinks(this INode nd, 
                                HashSet<Link> lks,
                                Func<ILink, bool> cond = null)
        {
            foreach (var lk in nd.Links)
            {
                if (cond?.Invoke(lk) == false
                    || lks.Contains(lk))
                    continue;
                lks.Add(lk);
                allLinks(lk.another(nd), lks, cond);
            }
            return lks;
        }

        public static IEnumerable<Link> links(this INode nd, 
                        Func<ILink, bool> cond)
        {
            foreach(var lk in nd.Links)
                if (cond?.Invoke(lk) != false)
                    yield return lk;
        }
    }
}
