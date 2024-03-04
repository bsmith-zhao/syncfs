using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace link
{
    public interface ILine
    {
        Point Begin { get; }
        Point End { get; }
    }
}
