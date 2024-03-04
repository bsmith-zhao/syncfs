using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace link
{
    public interface INode : IItem
    {
        Point[] Anchors { get; }
        List<Link> Links { get; }
    }
}
