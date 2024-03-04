using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace link
{
    public class RectAnchor
    {
        public Point[] Locs { get; set; } = new Point[4];

        static PointF[] jfs = new PointF[]
        {
            new PointF(0.5f, 0),
            new PointF(0.5f, 1),
            new PointF(0, 0.5f),
            new PointF(1, 0.5f),
        };

        public void adjustPos(ref Point pos, ref Size size)
        {
            for (int n = 0; n < Locs.Length; n++)
                Locs[n] = new Point(pos.X + (int)(size.Width * jfs[n].X),
                                pos.Y + (int)(size.Height * jfs[n].Y));
        }
    }
}
