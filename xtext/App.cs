using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;
using util.option;

namespace xtext
{
    public class App
    {
        public static string ConfPath => $"{"".appTrunk()}.conf";

        public static AppOption Option = new AppOption();
    }

    public class AppOption
    {
        public ExtendTextOption TextFile { get; set; } 
            = new ExtendTextOption();

        public PwdDeriveOption PwdDerive { get; set; }
            = new PwdDeriveOption();
    }
}
