using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util;
using util.ext;

namespace link
{
    public class MoveAction : BaseAction
    {
        public Point begin;
        public Point last;

        bool moved;
        public MoveAction start(Point begin)
        {
            this.begin = begin;
            this.last = begin;
            moved = false;

            return this;
        }

        public override bool OnMouseMove(MouseEventArgs e)
        {
            var pt = ui.toGramPos(e.Location);
            var delta = pt.minus(last);
            last = pt;

            if (delta.Width != 0 || delta.Height != 0)
            {
                ui.Picks.each(item =>
                {
                    if (item is IMovable mv)
                        mv.Pos = (mv.Pos + delta).min(10, 10);
                });
                ui.redraw();

                //new
                //{
                //    name = "Move.OnMouseMove",
                //    pos = e.Location,
                //    begin,
                //    last,
                //}.msgj();

                moved = true;
            }
            return true;
        }

        public override bool OnMouseUp(MouseEventArgs e)
        {
            stop();
            return moved;
        }

        // block key events on moving.
        public override bool OnKeyDown(KeyEventArgs e) => true;
        public override bool OnKeyUp(KeyEventArgs e) => true;

        public override void stop()
        {
            reset();
            if (last != begin)
            {
                ui.updateScroll();
                ui.OnItemsMoved(begin, last);
            }
        }
    }
}
