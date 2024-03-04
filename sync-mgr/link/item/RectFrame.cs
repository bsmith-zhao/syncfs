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
    public class RectFrame : Item
    {
        public bool Visiable = false;
        public Color FocusColor = Color.Gold;
        public Color UnfocusColor = Color.Olive;
        public const int Width = 3;
        internal override Rectangle Region
            => new Rectangle(pos, size).zoom(Width+1);

        public RectItem owner;
        public RectFrame(RectItem owner) => this.owner = owner;

        Point pos => owner.Pos;
        Size size => owner.Size;

        internal override void draw(Graphics g, DrawArgs e)
        {
            if (!Visiable)
                return;

            Rectangle r = new Rectangle(
                pos.X - Width, pos.Y - Width,
                size.Width + (Width * 2), size.Height + (Width * 2));

            // HatchBrush brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.LightGray, Color.Transparent);
            using (Brush br = new HatchBrush(HatchStyle.SmallCheckerBoard,
                                    e.focus ? FocusColor : UnfocusColor,
                                    Color.Transparent))
            using (Pen pen = new Pen(br, Width))
            {
                g.DrawRectangle(pen, r);
            }
        }
    }
}
