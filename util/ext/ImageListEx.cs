using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class ImageListEx
    {
        public static ImageList newImages(this Control ui, int size)
            => new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(size, size),
            };

        public static ImageList add(this ImageList list, string key, Image img)
        {
            list.Images.Add(key, img);
            return list;
        }
    }
}
