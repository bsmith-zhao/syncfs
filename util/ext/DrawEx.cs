using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class DrawEx
    {
        public static void near(this Point[] srcs, Point[] dsts,
            out Point ps, out Point pe)
        {
            double min = Double.MaxValue, dis;
            int si = 0, ei = 0;
            srcs.each((s, sp) => dsts.each((e, ep) =>
            {
                if ((dis = sp.distance(ep)) < min)
                {
                    si = s;
                    ei = e;
                    min = dis;
                }
            }));
            ps = srcs[si];
            pe = dsts[ei];
        }

        public static double distance(this Point a, Point b)
            => Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);

        public static Rectangle union(this Rectangle r1, Rectangle r2)
            => Rectangle.Union(r1, r2);

        public static Rectangle rect(this Point p1, Point p2)
            => new Rectangle(Math.Min(p1.X, p2.X),
                                Math.Min(p1.Y, p2.Y),
                                Math.Abs(p1.X - p2.X),
                                Math.Abs(p1.Y - p2.Y));

        public static Rectangle zoom(this Rectangle rc, int s)
        {
            rc.Inflate(s, s);
            return rc;
        }

        public static bool hitRect(this Point pt, Point pos, Size size)
        {
            return pt.X >= pos.X && pt.X <= pos.X + size.Width
                 && pt.Y >= pos.Y && pt.Y <= pos.Y + size.Height;
        }

        public static bool hitLine(this Point pt, Point begin, Point end, int width)
        {
            using (GraphicsPath gp = new GraphicsPath())
            using (Pen pen = new Pen(Color.Gray, width + 16))
            {
                gp.AddLine(begin, end);
                return gp.IsOutlineVisible(pt, pen);
            }
        }

        public static Point min(this Point p, int x, int y)
        {
            if (p.X < x)
                p.X = x;
            if (p.Y < y)
                p.Y = y;
            return p;
        }

        public static Size minus(this Point sp, Point ep)
        {
            return new Size(sp.X - ep.X, sp.Y - ep.Y);
        }

        public static Point move(this Point p, int w, int h)
        {
            return new Point(p.X + w, p.Y + h);
        }
    }
}
