using System;
using System.Windows.Forms;
using util.ext;

namespace test
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new TestForm().dark());

            //Application.Run(new TestManager().dark()
            //    .addTest(typeof(AesEmeTest))
            //    .addTest(typeof(HmacIvCbcTest))
            //    .addTest(typeof(CtrMacSpeedDemo))
            //    .addTest(typeof(CtrGcmSpeedDemo))
            //    .addTest(typeof(ChaCha20Poly1305Speed))
            //    .addTest(typeof(HKDFTest))
            //    .addTest(typeof(AesCtrStreamTest))
            //    .addTest(typeof(AesCtrNameEncode))
            //    .addTest(typeof(HmacThreadsSpeedDemo))
            //    .addTest(typeof(LambdaVariableScopeDemo))
            //    .addTest(typeof(ParallelScheduleDemo))
            //    .addTest(typeof(MonoScriptDemo))
            //    .addTest(typeof(TextSplitSpeedTest))
            //    .addTest(typeof(HashFileDemo))
            //    );
        }
    }
}
