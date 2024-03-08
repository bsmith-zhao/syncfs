using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.prop.conv
{
    [TypeConverter(typeof(ExpandClass))]
    public class DropListClass
    {
        public DropListClass()
        {
            Names = new List<string> { "jack", "pam", "phil", "suzan" };
        }

        [Browsable(false)]
        public List<string> Names { get; }

        [TypeConverter(typeof(DropList))]
        public string SelectedName { get; set; }


    }

    public class DropList : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            // you need to get the list of values from somewhere
            // in this sample, I get it from the MyClass itself
            var myClass = context.Instance as DropListClass;
            if (myClass != null)
                return new StandardValuesCollection(myClass.Names);

            return base.GetStandardValues(context);
        }
    }
}
