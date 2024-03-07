using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class ToolStripEx
    {
        public static void alignWidth(this ToolStripItem btn,
            params ToolStripItem[] others)
        {
            int max = btn.Width;
            others.each(b => b.Width.atLeast(ref max));
            btn.Width = max;
            others.each(b => b.Width = max);
        }

        public static void adjustBtns(this ToolStrip tb,
            int minWidth = 80, Action addon = null)
        {
            tb.layoutOnce(() => 
            {
                tb.eachItem(b =>
                {
                    b.AutoSize = false;
                    if (b.Width < minWidth)
                        b.Width = minWidth;
                });
                addon?.Invoke();
            });
        }

        public static void eachItem(this ToolStrip tb, Action<ToolStripItem> func)
            => tb.Items.each(func);

        public static void fixBorderBug(this ToolStrip tb)
        {
            tb.GripStyle = ToolStripGripStyle.Hidden;
            tb.Paint += ToolStrip_Paint_ClipBorder;
        }

        static void ToolStrip_Paint_ClipBorder(object sender, PaintEventArgs e)
        {
            var tb = sender as ToolStrip;
            var rec = new Rectangle(2, 0, tb.Width-4, tb.Height - 2);
            e.Graphics.SetClip(rec);
        }
    }
}
