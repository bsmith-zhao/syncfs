using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.crypt;
using util.crypt.sodium;
using util.ext;
using util.prop;
using util.prop.edit;

namespace util.option
{
    [TypeConverter(typeof(ExpandClass))]
    public class Argon2idOption
    {
        [NumberWheel(2, 1024, 1, 32)]
        public int CPU { get; set; } = 32;

        [ByteSizeWheel("32M", "512M", "16M", "64M")]
        public string Memory { get; set; } = "64M";

        public override string ToString()
            => $"CPU:{CPU}, Memory:{Memory}";

        public PwdDerive create()
            => new Argon2id
            {
                salt = Argon2id.SaltSize.aesRnd(),
                cpu = CPU,
                memory = (int)Memory.byteSize(),
            };

        public void evalTime()
        {
            create().genKey("123abc".utf8(), 32);
        }
    }
}
