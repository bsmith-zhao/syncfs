using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.prop
{
    public class DescRefer : Attribute
    {
        Type refer;

        public DescRefer(Type refer)
            => this.refer = refer;

        public Type referTo() => refer;
    }
}
