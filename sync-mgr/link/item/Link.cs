using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace link
{
    public class Link : Item, ILink
    {
        public INode Source { get; set; }
        public INode Target { get; set; }

        public Point Begin;
        public Point End;

        public Link(INode src, INode dst)
        {
            Source = src;
            Target = dst;
            src.Links.Add(this);
            dst.Links.Add(this);
            Source.Anchors.near(Target.Anchors, out Begin, out End);
        }

        public INode another(INode one)
            => one == Source ? Target : Source;

        public void detach()
        {
            Source?.Links.Remove(this);
            Source = null;
            Target?.Links.Remove(this);
            Target = null;
        }

        public void adjustPos(INode nd)
        {
            double min = double.MaxValue, dis;
            foreach (var b in Source.Anchors)
                foreach (var e in Target.Anchors)
                {
                    dis = b.distance(e);
                    if (dis < min)
                    {
                        Begin = b;
                        End = e;
                        min = dis;
                    }
                }
        }
    }
}
