using System;
using System.ComponentModel;
using util.prop;

namespace util
{
    // use in class define: 
    // [TypeConverter(typeof(ExpandClass))]
    public class ExpandClass : ExpandableObjectConverter
    {
        public override bool CanConvertTo(
            ITypeDescriptorContext context, 
            Type dstType)
        {
            if (dstType == typeof(string))
                return false;

            return base.CanConvertTo(context, dstType);
        }

        public override bool CanConvertFrom(
            ITypeDescriptorContext context, 
            Type srcType)
        {
            if (srcType == typeof(string))
                return false;

            return base.CanConvertFrom(context, srcType);
        }
    }
}
