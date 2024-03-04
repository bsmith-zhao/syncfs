using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace link
{
    public class LineFrame : Item
    {
        public Color FocusColor = Color.Gold;
        public Color UnfocusColor = Color.Olive;
        public bool Visiable = false;
        public int Width = 6;

        ILine owner;
        public LineFrame(ILine owner)
        {
            this.owner = owner;
        }

        internal override void draw(Graphics g, DrawArgs e)
        {
            if (!Visiable)
                return;

            using (Pen p = new Pen(Color.FromArgb(80, 
                                e.focus ? FocusColor : UnfocusColor), 
                                Width))
            { 
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                g.DrawLine(p, owner.Begin, owner.End);
            }
        }
    }
}
