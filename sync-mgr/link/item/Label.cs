using System.Drawing;
using System.Drawing.Drawing2D;
using util.ext;

namespace link
{
    public class Label : RectItem
    {
        public string Text
        {
            get => text;
            set => value.update(ref text, adjustHeight);
        }

        public Font Font
        {
            get => font;
            set => value.update(ref font, adjustHeight);
        }

        public Brush Brush { get; set; } = Brushes.LightGray;

        string text;
        Font font = new Font(FontFamily.GenericSansSerif, 10);
        StringFormat fmt = new StringFormat
        {
            Alignment = StringAlignment.Center,
        };

        internal override void draw(Graphics g, DrawArgs e)
        {
            var r = new Rectangle(pos, size);
            g.DrawString(text, font, Brush, (RectangleF)r, fmt);
        }

        void adjustHeight() 
            => size.Height = measureHeight(text);

        public int measureHeight(string str)
        {
            using (Bitmap bmp = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SizeF sizeF = g.MeasureString(str, font, size.Width, fmt);
                return Size.Round(sizeF).Height;
            }
        }
    }
}




