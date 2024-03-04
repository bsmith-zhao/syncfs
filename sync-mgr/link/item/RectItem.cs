using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace link
{
    public class RectItem : Item, IHitable, IMovable
    {
        protected Point pos = new Point(100, 100);
        protected Size size = new Size(96, 96);

        public virtual Point Pos { get => pos; set => pos = value; }
        public int X => pos.X;
        public int Y => pos.Y;
        public virtual Size Size { get => size; set => size = value; }
        public virtual int Width { get => size.Width; set => size.Width = value; }
        internal override Rectangle Region
            => new Rectangle(pos, size);

        public virtual bool HitTest(Point pt)
            => pt.hitRect(pos, size);
    }
}
