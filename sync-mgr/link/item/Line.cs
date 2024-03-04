using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using util;

namespace link
{
	public class Line: Item
    {
        public Point Begin;
        public Point End;
        public CustomLineCap BeginCap;
        public CustomLineCap EndCap;
        public Color Color = Color.Wheat;
        public int Width = 3;
        public float[] Dashs;
        public DashStyle Style = DashStyle.Solid;

        public CustomLineCap[] Caps
        {
            get => new CustomLineCap[] { BeginCap, EndCap };
            set
            {
                BeginCap = value[0];
                EndCap = value[1];
            }
        }

        internal override void draw(Graphics g, DrawArgs e)
		{
            using (Pen p = new Pen(Color, Width))
            {
                if (BeginCap == null)
                    p.StartCap = LineCap.Round;
                else
                    p.CustomStartCap = BeginCap;

                if (EndCap == null)
                    p.EndCap = LineCap.Round;
                else
                    p.CustomEndCap = EndCap;

                if (null != Dashs)
                    p.DashPattern = Dashs;
                else if (Style != DashStyle.Solid)
                    p.DashStyle = Style;
                g.DrawLine(p, Begin, End);
            }
            base.draw(g, e);
		}
    }
}