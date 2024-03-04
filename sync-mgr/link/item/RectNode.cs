using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using util.ext;
using System.Collections.Generic;

namespace link
{
    public class RectNode : RectItem, INode, IEditLabel, IPickable
    {
        public Point[] Anchors => anchor.Locs;
        public List<Link> Links { get; set; } = new List<Link>();
        public string Text { get => Label.Text; set => Label.Text = value; }
        public Font Font { get => Label.Font; set => Label.Font = value; }
        public Image Image { get; set; }
        public override Point Pos
        {
            set => value.update(ref pos, adjustItems);
        }

        public Label Label { get; }

        internal override Rectangle Region
            => pickFrame.Region.union(Label.Region);

        RectFrame pickFrame;
        RectAnchor anchor = new RectAnchor();

        void adjustItems()
        {
            Label.Pos = labelPos;

            anchor.adjustPos(ref pos, ref size);
            Links.each(lk => lk.adjustPos(this));
        }

        const int LabelMargin = 10;
        Point labelPos => pos.move(-LabelMargin, size.Height);

        public RectNode(Size size)
        {
            this.size = size;
            Label = new Label
            {
                Pos = labelPos,
                Size = new Size(size.Width + LabelMargin*2, 0),
            };
            pickFrame = new RectFrame(this);
        }

        internal override void draw(Graphics g, DrawArgs e)
        {
            if (null != Image)
                g.DrawImage(Image, pos.X, pos.Y, 
                        size.Width, size.Height);

            Label.draw(g, e);
            pickFrame.draw(g, e);
        }

        bool IPickable.Picked
        {
            get => pickFrame.Visiable;
            set => pickFrame.Visiable = value;
        }

        public override bool HitTest(Point pt)
        {
            if (Label.HitTest(pt))
                return true;
            return base.HitTest(pt);
        }
    }
}
