using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public abstract class Test
    {
        public abstract void test();

        public R call<T1, R>(Func<T1, R> func, T1 a1)
        {
            func.showCall(a1);
            return func(a1);
        }

        public void call<T1>(Action<T1> func, T1 a1)
        {
            func.showCall(a1);
            func(a1);
        }

        public void call<T1, T2>(Action<T1, T2> func, T1 a1, T2 a2)
        {
            func.showCall(a1, a2);
            func(a1, a2);
        }

        public void assert(bool @true)
            => @true.assert();
        public void assertError(Action func)
            => func.assertError();
        public void assertError<T1>(Action<T1> func, T1 a1)
            => func.assertError(a1);
        public void assertError<T1, T2>(Action<T1, T2> func, T1 a1, T2 a2)
            => func.assertError(a1, a2);
    }
}
