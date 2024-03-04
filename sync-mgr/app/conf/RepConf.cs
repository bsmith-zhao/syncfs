using sync.app.conf;
using sync.sync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using util;
using util.ext;
using util.prop;
using util.rep;
using vfs.rep;

namespace sync.app
{
    [TypeConverter(typeof(ExpandProp))]
    public abstract class RepConf : ViewConf
    {
        public static RepConf create(RepType type)
        {
            var conf = cls(type).@new() as RepConf;
            conf.Type = $"{type}";
            return conf;
        }

        public static Type cls(RepType type)
        {
            switch (type)
            {
                case RepType.NormalDir:
                    return typeof(DirRepConf);
                case RepType.AeadFS:
                    return typeof(AeadRepConf);
            }
            return null;
        }

        public abstract string getSource();

        [ReadOnly(true)]
        [Category("1.Reposit"), PasteIgnore]
        public virtual string Type { get; set; }

        [UnifyPath]
        [Category("1.Reposit")]
        public string Match { get; set; }

        [Category("1.Reposit")]
        public MountConf Mount { get; set; } = new MountConf();

        [Category("1.Reposit")]
        public BackupConf Backup { get; set; } = new BackupConf();

        public abstract Reposit openRep();
        public abstract RepArgs newRepArgs();

        public View openView()
        {
            var rep = openSyncRep();
            return new View
            {
                rep = rep,
                src = rep,
                root = RootDir,
                flt = newFilter(App.Option.Lock,
                            Match,
                            Backup.Directory),
            }.open(ForceUnlock);
        }

        public SyncReposit openSyncRep()
            => new SyncReposit
            {
                src = openRep(),
                match = Match,
                @lock = App.Option.Lock,
                backup = Backup.Enable ? Backup.create() : null,
            }.open();

        public virtual void OnActive() { }
        public virtual bool OnChange(object owner, string fld)
            => false;

        public abstract bool exist();
        public abstract string createRep();

        public abstract bool canModifyPwd();
        public virtual bool modifyPwd() => false;
    }
}
