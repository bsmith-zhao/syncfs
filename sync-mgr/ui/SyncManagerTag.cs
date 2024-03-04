using link;
using sync.app;
using sync.app.conf;
using sync.ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using util;
using util.ext;
using util.rep;

namespace sync.ui
{
    public interface IViewTag
    {
        //IDir newView();
    }

    public interface IConfTag
    {
        ArgsConf conf { get; }
    }

    public interface IResTag
    {
        string id { get; }
    }

    public class SpaceTag
    {
        public SpaceEntry space;
        public LinkViewGraph graph;

        public Dictionary<string, RepEntry> getRepMap()
            => getReps().toMap(r => r.id);

        public RepEntry[] getReps()
            => graph.Items.pick(e => e.Tag is RepTag)
            .conv(e =>
            {
                var rc = e as RectNode;
                var re = (rc.Tag as RepTag).entry;
                re.name = rc.Text;
                re.pos = rc.Pos;
                return re;
            }).ToArray();

        public ViewEntry[] getViews()
        {
            var vs = graph.Items.pick(e => e.Tag is ViewTag).conv(e =>
            {
                var rc = e as RectNode;
                var ve = (rc.Tag as ViewTag).entry;
                ve.src = null;
                ve.name = rc.Text;
                ve.pos = rc.Pos;
                return ve;
            }).ToArray();
            graph.Items.pick(e => e.Tag is ViewLinkTag)
            .each(it =>
            {
                var lk = it as Link;
                lk.Target.viewTag().entry.src = lk.Source.idTag().id;
            });
            return vs;
        }
    }

    public abstract class ConfTag : IConfTag
    {
        public ArgsConf conf { get; set; }
    }

    public class RepTag : ConfTag, IResTag, IViewTag
    {
        public RepEntry entry;
        public Process vfs;

        public bool canModifyPwd
            => args.canModifyPwd();

        public RepConf args => conf.Args as RepConf;

        public string id => entry.id;

        public void OnActive() => args.OnActive();

        public bool OnChange(object owner, string fld)
            => args.OnChange(owner, fld);
    }

    public class ViewTag : ConfTag, IResTag
    {
        public ViewEntry entry;

        public string id => entry.id;

        public ViewConf args => conf.Args as ViewConf;
    }

    public class ViewLinkTag { }

    public class SyncTag : ConfTag, IResTag
    {
        public SyncEntry entry;
        public LineLink link;
        public TreeNode node;

        public SyncEntry getWork()
        {
            if (null != link)
            {
                entry.src = link.Source.idTag().id;
                entry.dst = link.Target.idTag().id;
            }
            return entry;
        }

        public SyncConf args
        {
            get => conf.Args as SyncConf;
            set => conf.Args = value;
        }

        public HashConf hash
        {
            get => args.Hash;
            set => args.Hash = value;
        }

        public string id => entry.id;

        public bool OnChange(object owner, string fld)
        {
            if (owner == hash
                || (owner == conf.Args && fld == nameof(SyncConf.Hash)))
                this.trydo(() => 
                {
                    string js = null;
                    foreach (var lk in link.Source.allLinks(lk => lk.Tag is SyncTag))
                    {
                        if (lk == link)
                            continue;
                        var other = lk.Tag as SyncTag;
                        js = js ?? hash.json();
                        other.hash = js.obj<HashConf>();
                        other.conf.save();
                    }
                });
            return false;
        }
    }
}
