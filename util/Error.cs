using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public class Error : Exception
    {
        public Type type;
        public string item;
        public object[] args;

        public Error(object obj, string act, params object[] args)
        {
            this.type = obj.GetType();
            this.item = act;
            this.args = args;
        }

        public Error(Type type, string act, params object[] args)
        {
            this.type = type;
            this.item = act;
            this.args = args;
        }

        public override string Message 
            => type.trans(item, args);

        public string Json
            => new ErrorJson
            {
                code = $"{type.Name}.{item}",
                args = args,
            }.json();
    }

    public class Error<T> : Error
    {
        public Error(string act, params object[] args)
            : base(typeof(T), act, args)
        {
        }
    }

    public class ErrorJson
    {
        public string code;
        public object[] args;

        public override string ToString()
            => code.trans(args);
    }
}
