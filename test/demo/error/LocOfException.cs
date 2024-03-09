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
    //public class Abc
    //{
    //    public int a { get; set; } = 1;
    //}

    //public class LocOfException : ITest
    //{
    //    public void test()
    //    {
    //        new
    //        { a = 1, b = 2, c = 3, d = new { a = 1, b = 2 }, e = new Abc(),
    //            f = new int[] { 1, 2, 3 },
    //            g = "  12  3  ",
    //        }.msg();
    //        //testError();
    //    }

    //    public void testError()
    //    {
    //        try
    //        {
    //            throwError(1);
    //        }
    //        catch (Exception err)
    //        {
    //            traceError(err);
    //        }
    //    }

    //    void traceError(Exception err)
    //    {
    //        var st = new StackTrace(false);
    //        var caller = new StackFrame(1).GetMethod().Name;
    //        new {err.Message, caller, err.Source, trigger = err.TargetSite.shortName() }.debugj();

    //        new { f = true.callTrace(0), t = 0 }.debugj();
    //        new { f = true.callTrace(-1), t = 1 }.debugj();
    //        new { f = true.callTrace(-2), t = 2 }.debugj();

    //        new { thisFunc = true.thisFunc()}.debugj();
    //        new { lastFunc = true.lastFunc()}.debugj();
    //    }

    //    //string fullName(MemberInfo m)
    //    //    => m == null ? null
    //    //    : $"{m.DeclaringType?.FullName}.{m.Name}";

    //    public long throwError(int a)
    //    {
    //        //throw new Exception("abc");
    //        throw new Error<LocOfException>("MyError", 1, 2);
    //        //return 0;
    //    }
    //}
}
