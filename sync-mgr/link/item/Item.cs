using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml;
using System.Collections.Generic;

namespace link
{
    public interface IItem
    {
        object Tag { get; set; }
    }

    public abstract class Item : IItem
    {
        public object Tag { get; set; }
        internal virtual Rectangle Region { get; }
        internal virtual void draw(Graphics g, DrawArgs e) { }
    }
}
