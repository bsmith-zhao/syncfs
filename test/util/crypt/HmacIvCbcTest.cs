using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;

namespace util.crypt
{
    public class HmacIvCbcTest : Test
    {
        HmacIvCbc macCbc;

        public override void test()
        {
            byte[] encKey = 32.aesKey();
            byte[] macKey = 32.aesKey();
            macCbc = new HmacIvCbc(encKey, macKey);

            var cs = new List<string>
            {
                @"c:\users\bsmith\documents\自定义 office 模板",
                @"c:\u1sers\bsmith\documents\自定义 office 模板",
                @"c:\users\bsmith\documents\自定义 work 模板",
                "c:/u11",
                "c:/u117890123456",
                "c:/u1178901234567890",
                "c:/u1178901234567890123456789012"
            };

            cs.ForEach(c => encAndDec(c));
            
        }

        public void encAndDec(string s)
        {
            $"{s}".msg();

            var d = s.utf8();
            d.showHex(nameof(d));

            var c = macCbc.encrypt(d);
            c.showB64(nameof(c));

            var b = macCbc.decrypt(c, false);
            b.showHex(nameof(b));

            assert(b.utf8() == s);

            $"".msg();
        }
    }
}
