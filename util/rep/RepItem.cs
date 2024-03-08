using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.rep
{
    public abstract class RepItem
    {
        public string path;
        public long size;
        public long createTime;
        public long modifyTime;

        protected string _name;
        public string name
        {
            get => _name ?? (_name = path.pathName());
            set => _name = value;
        }

        protected string _dir;
        public string dir
        {
            get => _dir ?? (_dir = path.pathDir());
            set => _dir = value;
        }
    }
}
