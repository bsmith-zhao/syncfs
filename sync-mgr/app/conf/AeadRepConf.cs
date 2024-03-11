using Newtonsoft.Json;
using sync.work;
using System.ComponentModel;
using System.Drawing.Design;
using util;
using util.crypt;
using util.ext;
using util.option;
using util.prop;
using util.rep;
using util.rep.aead;
using vfs.rep;

namespace sync.app.conf
{
    public class AeadRepConf : RepConf
    {
        string dir;
        [Editor(typeof(DirPicker), typeof(UITypeEditor))]
        [Category("1.Reposit"), UnifyPath]
        public virtual string Path { get => dir; set => dir = value; }

        public byte[] data;
        static byte[] salt = $"419@066".utf8();
        [Editor(typeof(PwdView), typeof(UITypeEditor))]
        [PasswordPropertyText(true), JsonIgnore]
        [Category("3.AeadFS")]
        public string Password
        {
            get => data.winTryDec(salt).utf8();
            set => data = value.utf8().winEnc(salt);
        }
        byte[] pwd => Password?.utf8();

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

        string repUid => AeadFsReposit.unifyId(dir);

        public override Reposit openRep()
        {
            if (!exist())
                throw new Error<AeadFsReposit>("NotExist", confPath);
            var conf = decryptConf();
            return new AeadFsReposit(dir, conf);
        }

        AeadFsConf decryptConf()
        {
            var conf = AeadFsConf.load(confPath);
            if (!conf.decrypt(pwd))
                repUid.queryPwd<UserCancel>(conf.decrypt);
            return conf;
        }

        public override RepArgs newRepArgs()
        {
            var conf = decryptConf();
            return new AeadFsArgs
            {
                path = confPath,
                mkey = conf.getMKey()
            };
        }

        public override bool exist()
            => confPath.fileExist();

        public override string createRep()
        {
            if ((dir = true.pickDir()) == null)
                return null;

            if (!confPath.fileExist())
            {
                var opt = option.jclone();
                if (!opt.dlgSetup() || !repUid.setPwd(out var pwd))
                    return null;

                saveConf(opt.createConf(), pwd, opt.PwdDerives);
            }

            return dir.pathName();
        }

        public override bool canModifyPwd()
            => conf != null;

        AeadFsOption option => App.Option.AeadFS;
        public override bool modifyPwd()
        {
            if (!canModifyPwd())
                return false;

            if (!repUid.modifyPwd(conf.decrypt, out var newPwd))
                return false;

            saveConf(conf, newPwd, App.Option.AeadFS.PwdDerives);

            return true;
        }

        void saveConf(AeadFsConf conf, byte[] pwd, PwdDeriveType[] pds)
        {
            conf.save(confPath, pwd, App.Option.newPwdDerives(pds));

            Password = pwd.utf8();
        }

        string confPath => AeadFsConf.confPath(dir);
        AeadFsConf conf;
        public override void OnActive()
        {
            true.trydo(()=> 
            {
                conf = conf ?? AeadFsConf.load(confPath);
            });
        }

        public override bool OnChange(object owner, string fld)
        {
            if (fld == nameof(Path))
            {
                true.trydo(() => conf = AeadFsConf.load(confPath));
                return true;
            }
            return false;
        }

        public override string getSource()
            => Path;
    }
}
