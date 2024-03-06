using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;
using util.option;
using util.prop;
using util.rep.aead;
using vfs.rep;

namespace vfs.mgr.conf
{
    [TypeConverter(typeof(ExpandProp))]
    public class AeadRepConf : RepConf, IDynamicConf
    {
        string dir;
        [ReadOnly(true)]
        public string Path { get => dir; set => dir = value; }

        public override string getSource()
            => Path;

        string confPath => AeadFsConf.confPath(dir);
        string repUid => AeadFsReposit.unifyId(dir);

        public override RepArgs newRepArgs()
        {
            var conf = AeadFsConf.loadByDir(dir);
            repUid.queryPwd<CancelPwd>(conf.decrypt);

            return new AeadFsArgs
            {
                path = confPath,
                mkey = conf.getMKey(),
            };
        }

        public override bool canModifyPwd()
            => true;

        public override void modifyPwd()
        {
            var conf = AeadFsConf.loadByDir(dir);

            if (!repUid.modifyPwd(conf.decrypt, out var newPwd))
                return;

            conf.saveByDir(dir, newPwd, newPwdDerives());
        }

        public bool createRep(out string dir)
        {
            if (!true.pickDir(out dir))
                return false;

            Path = dir;

            if (confPath.fileExist())
                return true;

            var opt = App.Option.AeadFS.jclone();
            if (!opt.dlgSetup() || !repUid.setPwd(out var pwd))
                return false;

            opt.createConf().saveByDir(dir, pwd, newPwdDerives());

            return true;
        }

        PwdDeriveEntry[] newPwdDerives()
            => App.Option.newPwdDerives(App.Option.AeadFS.PwdDerives);

        public void OnCreate() { }

        AeadFsConf conf;
        public void OnActive()
        {
            this.trydo(() =>
            {
                conf = conf ?? AeadFsConf.loadByDir(dir);
            });
        }

        public bool OnChange(object owner, string fld, object old)
        {
            return false;
        }

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public string Encode => conf?.Encode;

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public int? MasterKeySize => conf?.MasterKeySize;

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public int? FileIdSize => conf?.FileIdSize;

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public string BlockSize
            => conf != null
            ? $"{((long)conf.BlockSize).byteSize()}({conf.BlockSize.ToString("#,##0")})"
            : null;

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public string PwdDerives => conf?.PwdDerives.conv(kg => kg.type).join(", ");

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public KeyDeriveType? KeyDerive => conf?.KeyDerive;

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public AeadCryptType? DataCrypt => conf?.DataCrypt;

        [DescRefer(typeof(AeadFsOption))]
        [Category("3.AeadFS"), JsonIgnore]
        public DirCryptType? DirCrypt => conf?.DirCrypt;
    }
}
