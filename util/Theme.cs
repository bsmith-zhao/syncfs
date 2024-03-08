using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util
{
    public static class Theme
    {
        public static Color FormBack = Color.FromArgb(80, 80, 80);
        public static Color TextFore = Color.FromArgb(230, 230, 220);
        public static Color ControlBack = Color.FromArgb(50, 50, 50);
        public static Color ButtonBack = Color.FromArgb(110, 110, 110);
        public static Color ButtonBorder = Color.FromArgb(80, 80, 80);
        public static BorderStyle PanelBorder = BorderStyle.None;

        //public static ListBox theme(this ListBox lb)
        //{
        //    dark(lb);
        //    return lb;
        //}

        public static T theme<T>(this T form) where T : Form
        {
            setDarkTitle(form);
            form.BackColor = FormBack;
            form.ForeColor = TextFore;

            darkSubs(form);

            return form;
        }

        public static void darkSubs(Control ui)
        {
            foreach (var obj in ui.Controls)
            {
                if (obj is Button btn)
                    dark(btn);
                else if (obj is TextBoxBase tx)
                    dark(tx);
                else if (obj is ListBox lb)
                    theme(lb);
                else if (obj is TreeView tr)
                    dark(tr);
                else if (obj is ListView lv)
                    dark(lv);
                else if (obj is ToolStrip tb)
                    dark(tb);
                else if (obj is PropertyGrid pg)
                    dark(pg);
                else if (obj is Panel panel)
                    darkSubs(panel);
                else if (obj is UserControl uc)
                    darkSubs(uc);
            }
        }

        public static void theme(this UserControl ui)
        {
            ui.BackColor = FormBack;
            ui.ForeColor = TextFore;

            darkSubs(ui);
        }

        public static void dark(PropertyGrid ui)
        {
            ui.ViewBackColor = ControlBack;
            ui.ViewBorderColor = ControlBack;
            ui.ViewForeColor = TextFore;
            ui.LineColor = FormBack;
            ui.CanShowVisualStyleGlyphs = false;
            ui.CategorySplitterColor = ControlBack;
        }

        public static void dark(ToolStrip ui)
        {
            ui.BackColor = FormBack;
            ui.ForeColor = TextFore;
            foreach (var obj in ui.Items)
            {
                if (obj is ToolStripTextBox tx)
                {
                    tx.BackColor = ControlBack;
                    tx.ForeColor = TextFore;
                    tx.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        public static void dark(TreeView ui)
        {
            ui.BackColor = ControlBack;
            ui.ForeColor = TextFore;
            ui.BorderStyle = PanelBorder;
        }

        public static void dark(ListView ui)
        {
            ui.BackColor = ControlBack;
            ui.ForeColor = TextFore;
            ui.BorderStyle = PanelBorder;

            //ui.OwnerDraw = true;
            //ui.DrawItem += Ui_DrawItem;
            //ui.DrawColumnHeader += ListView_DrawColumnHeader;
            //ui.ColumnWidthChanged += ListView_ColumnWidthChanged;
            ////ui.LostFocus += (s,e) => ui.autoSpan();
            //ui.GotFocus += (s, e) => ui.autoSpan();
            //ui.autoSpan();
        }

        //private static void ListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        //{
        //    var ui = sender as ListView;
        //    if (e.ColumnIndex == ui.Columns.Count - 1)
        //        return;
        //    var col = ui.Columns[ui.Columns.Count - 1];
        //    col.Width = -2;
        //}

        //private static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        //{
        //    //Fills one solid background for each cell.
        //    using (SolidBrush backBrush = new SolidBrush(FormBack))
        //    {
        //        e.Graphics.FillRectangle(backBrush, e.Bounds);
        //    }
        //    //Draw the borders for the header around each cell.
        //    using (Pen backBrush = new Pen(ControlBack))
        //    {
        //        e.Graphics.DrawRectangle(backBrush, e.Bounds);
        //    }
        //    using (SolidBrush foreBrush = new SolidBrush(TextFore))
        //    {
        //        //Since e.Header.TextAlign returns 'HorizontalAlignment' with values of (Right, Center, Left).  
        //        //DrawString uses 'StringAlignment' with values of (Near, Center, Far). 
        //        //We must translate these and setup a vertical alignment that doesn't exist in DrawListViewColumnHeaderEventArgs.
        //        StringFormat stringFormat = GetStringFormat(e.Header.TextAlign);

        //        //Do some padding, since these draws right up next to the border for Left/Near.  Will need to change this if you use Right/Far
        //        Rectangle rect = e.Bounds; rect.X += 2;
        //        // e.Graphics.DrawString(e.Header.Text, e.Font, foreBrush, rect, stringFormat);
        //        e.Graphics.DrawString(e.Header.Text, e.Font, foreBrush, rect, stringFormat);
        //    }
        //}

        //private static StringFormat GetStringFormat(HorizontalAlignment ha)
        //{
        //    StringAlignment align;

        //    switch (ha)
        //    {
        //        case HorizontalAlignment.Right:
        //            align = StringAlignment.Far;
        //            break;
        //        case HorizontalAlignment.Center:
        //            align = StringAlignment.Center;
        //            break;
        //        default:
        //            align = StringAlignment.Near;
        //            break;
        //    }

        //    return new StringFormat()
        //    {
        //        Alignment = align,
        //        LineAlignment = StringAlignment.Center
        //    };
        //}

        //private static void Ui_DrawItem(object sender, DrawListViewItemEventArgs e)
        //{
        //    e.DrawDefault = true;
        //}

        public static void dark(Button ui)
        {
            ui.BackColor = ButtonBack;
            ui.FlatStyle = FlatStyle.Flat;
            ui.FlatAppearance.BorderColor = ButtonBorder;
        }

        public static ListBox theme(this ListBox ui)
        {
            ui.BackColor = ControlBack;
            ui.ForeColor = TextFore;
            ui.BorderStyle = PanelBorder;
            return ui;
        }

        public static void dark(TextBoxBase ui)
        {
            if (ui.ReadOnly)
                ui.BackColor = FormBack;
            else
                ui.BackColor = ControlBack;
            ui.ForeColor = TextFore;

            if (ui.Multiline)
            {
                ui.BorderStyle = PanelBorder;
            }
            else if (ui.BorderStyle != BorderStyle.None)
            {
                //ui.MinimumSize = new Size(0, ui.Size.Height);
                ui.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public static void setDarkTitle(Form form)
        {
            IntPtr hwnd = form.Handle;
            //DwmSetWindowAttribute(hwnd, 19, new[] { 1 }, 4);
            //DwmSetWindowAttribute(hwnd, 20, new[] { 1 }, 4);
            if (DwmSetWindowAttribute(hwnd, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(hwnd, 20, new[] { 1 }, 4);
        }
    }
}
