using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace link
{
    public interface ILink : IItem
    {
        INode Source { get; }
        INode Target { get; }
    }
}
