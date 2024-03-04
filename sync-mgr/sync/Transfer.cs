using sync.hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync.sync
{
    public class Transfer<T>
    {
        public List<T> adds = new List<T>();
        public List<T> dels = new List<T>();
        public List<T[]> moves = new List<T[]>();
    }
}
