using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using util;
using util.ext;

namespace link
{
    public partial class LinkView : UserControl
    {
        public LinkView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // This change control to not flick
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            //this.SetStyle(ControlStyles.Selectable, true);

            moveAct = new MoveAction
            {
                ui = this,
            };
            editAct = new EditLabelAction
            {
                ui = this,
            };

            (editTimer = new Timer
            {
                Interval = SystemInformation.DoubleClickTime + 50
            }).Tick += EditTimer_Tick;
        }

        Timer editTimer;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DiagramView
            // 
            this.AutoScroll = true;
            this.Name = "DiagramView";
            this.ResumeLayout(false);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (Picks?.Count > 0)
                redraw();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (Picks?.Count > 0)
                redraw();
            base.OnLostFocus(e);
        }

        DrawArgs dr = new DrawArgs();
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Default;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Point sc = this.AutoScrollPosition;
            g.TranslateTransform(sc.X, sc.Y);

            dr.focus = this.Focused;

            Items.each(item => item.draw(g, dr));
        }

        internal BaseAction action;
        EditLabelAction editAct;
        MoveAction moveAct;

        public Item lastPick;
        int clicks;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();

            if (action?.OnMouseDown(e) == true)
                return;

            clicks = e.Clicks;
            if (e.Button == MouseButtons.Left
                || e.Button == MouseButtons.Right)
            {
                var pt = toGramPos(e.Location);
                if (pickItem(pt, out var pick))
                {
                    if (!Picks.Contains(pick))
                    {
                        if (!(Control.ModifierKeys == Keys.Control
                            || e.Button == MouseButtons.Right))
                        {
                            graph?.clearPicks();
                        }
                        Picks.Add(pick);
                        (pick as IPickable).Picked = true;
                        PicksChanged?.Invoke();
                    }

                    if (Picks.Count > 0)
                        action = moveAct.start(pt);
                }
                else
                {
                    if ((graph?.clearPicks() ?? 0) > 0)
                        PicksChanged?.Invoke();
                }

                redraw();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (action?.OnMouseMove(e) == true)
                return;

            base.OnMouseMove(e);
        }

        private void EditTimer_Tick(object sender, EventArgs e)
        {
            editTimer.Stop();
            if (action == null && lastPick is IEditLabel lb)
                action = editAct.start(lb);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            var pick = Picks.last();
            if (action?.OnMouseUp(e) != true)
            {
                if (clicks == 1 && lastPick == pick
                    && pick is IEditLabel)
                {
                    editTimer.Start();
                    return;
                }
                else if (clicks == 2)
                    editTimer.Stop();
                    base.OnMouseUp(e);
            }
            lastPick = pick;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (action?.OnKeyDown(e) == true)
                return;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (action?.OnKeyUp(e) == true)
                return;

            base.OnKeyUp(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (action?.OnMouseDoubleClick(e) == true)
                return;

            base.OnMouseDoubleClick(e);
        }

        /// Graphic surface coordinates to graphic object coordinates.
        public Point toGramPos(Point p)
        {
            p.X = p.X - this.AutoScrollPosition.X;
            p.Y = p.Y - this.AutoScrollPosition.Y;
            return p;
        }

        public Point toViewPos(Point p)
        {
            p.X = p.X + this.AutoScrollPosition.X;
            p.Y = p.Y + this.AutoScrollPosition.Y;
            return p;
        }

        internal bool pickItem(Point pt, out Item item)
            => (item = Items.last(e => e is IPickable
                                && e is IHitable ht
                                && ht.HitTest(pt))) != null;

        internal INode getDock(Point pt)
        {
            return Items.last(item => item is INode
                            && item is IHitable ht
                            && ht.HitTest(pt)) as INode;
        }

        internal void updateScroll()
        {
            var rc = new Rectangle();
            Items.each(it => rc = rc.union(it.Region));
            this.AutoScrollMinSize = rc.Size;
        }

        internal void redraw(LinkViewGraph gram)
        {
            if (this.graph == gram)
                redraw();
        }

        internal void redraw()
        {
            this.Invalidate();
        }
    }
}
