using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using util.ext;

namespace util
{
    // use in filed define:
    // [TypeConverter(typeof(ArrayField))]
    public class ArrayField : ArrayConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type dstType)
        {
            if (dstType == typeof(string))
                return value == null ? "" : format(value);

            return base.ConvertTo(context, culture, value, dstType);
        }

        public string format(object value)
        {
            if (value is Array arr)
                return string.Join(",", arr.conv<object, string>(e => e?.ToString()));

            return value.ToString();
        }
    }
}
