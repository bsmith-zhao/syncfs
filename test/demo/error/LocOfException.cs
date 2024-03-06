using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;

namespace test.demo.error
{
    public class LocOfException : ITest
    {
        public void test()
        {
            testError();
        }

        public void testError()
        {
            try
            {
                throwError(1);
            }
            catch (Exception err)
            {
                traceError(err);
            }
        }

        void traceError(Exception err)
        {
            var st = new StackTrace(false);
            var caller = new StackFrame(1).GetMethod().Name;
            new {err.Message, caller, err.Source, trigger = err.TargetSite.shortName() }.debugj();

            new { f = this.callTrace(0), t = 0 }.debugj();
            new { f = this.callTrace(-1), t = 1 }.debugj();
            new { f = this.callTrace(-2), t = 2 }.debugj();

            new { thisFunc = this.thisFunc()}.debugj();
            new { lastFunc = this.lastFunc()}.debugj();
        }

        //string fullName(MemberInfo m)
        //    => m == null ? null
        //    : $"{m.DeclaringType?.FullName}.{m.Name}";

        public long throwError(int a)
        {
            //throw new Exception("abc");
            throw new Error<LocOfException>("MyError", 1, 2);
            //return 0;
        }
    }
}
