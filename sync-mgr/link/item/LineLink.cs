using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using util.ext;

namespace link
{
	public class LineLink: Link, IPickable, ILine, IHitable
	{
        public bool Picked
        {
            get => frame.Visiable;
            set => frame.Visiable = value;
        }
        public Line Line { get; set; } = new Line();
        public CustomLineCap[] Caps { set => Line.Caps = value; }

        internal override Rectangle Region
            => Begin.rect(End).zoom(frame.Width);

        Point ILine.Begin => Begin;
        Point ILine.End => End;

        LineFrame frame;

        public LineLink(INode begin, INode end) 
            : base(begin, end)
        {
            frame = new LineFrame(this)
            {
                Width = Line.Width + 6,
            };
        }

        internal override void draw(Graphics g, DrawArgs e)
		{
            Line.Begin = Begin;
            Line.End = End;
            Line.draw(g, e);
            frame.draw(g, e);
        }

        public bool HitTest(Point pt)
            => pt.hitLine(Begin, End, Line.Width);
    }
}


