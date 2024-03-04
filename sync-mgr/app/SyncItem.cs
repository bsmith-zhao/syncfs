using sync.sync;
using sync.work;
using util;
using util.ext;
using util.rep;

namespace sync.app
{
    public class SyncItem : WorkItem
    {
        public SpaceEntry space;
        public SyncEntry sync;
        public SpaceContext context;

        public override bool canParse => true;

        public override string group 
            => $"{space.name}<{space.dir}>";

        public override string name
            => sync.name;

        public override bool check 
            => sync.check;

        string path => space.entryPath(sync.id);

        public override string dir
            => path.pathDir();

        public override string icon 
            => $"{sync.type}";

        SyncConf cf;
        SyncConf conf => cf ?? 
            (cf = path.readText().obj<SyncConf>());

        SpaceContext ctx => context
            ?? (context = new SpaceContext(space));

        MasterSync.Param newArgs(IDir src, IDir dst)
            => new MasterSync.Param
            {
                src = src,
                dst = dst,

                srcOut = space.entryPath(sync.src).pathDir(),
                dstOut = space.entryPath(sync.dst).pathDir(),

                hash = conf.Hash.newHash(),
                comp = conf.Compare.newCompare(),
            };

        Work newSync(IDir src, IDir dst, bool parse)
        {
            switch (sync.type)
            {
                case SyncType.MasterSync:
                    return new MasterSync
                    {
                        parse = parse,
                        args = newArgs(src, dst)
                    };
                case SyncType.RoundSync:
                    return new RoundSync
                    {
                        parse = parse,
                        args = newArgs(src, dst)
                    };
            }
            return null;
        }

        void run(bool parse)
        {
            using (var src = ctx.openView(sync.src))
            using (var dst = ctx.openView(sync.dst))
            {
                new Worker
                {
                    logCount = App.Option.LogCount,
                }.run(this, newSync(src, dst, parse));
            }
        }

        public override void parse()
        {
            run(parse: true);
        }

        public override void work()
        {
            run(parse: false);
        }
    }
}
