using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace util.prop.adjust
{
    public class UnifyEncode : AdjustValue
    {
        public const string Default = UTF8;

        public const string UTF8 = "UTF-8";
        public const string GBK = "GBK";

        string def;

        public UnifyEncode(string @default = null)
            => this.def = @default ?? Default;

        public override object adjust(object value)
        {
            var enc = $"{value}".Trim();
            return true.tryget(() => Encoding.GetEncoding(enc)) 
                != null ? enc : def;
        }
    }
}
