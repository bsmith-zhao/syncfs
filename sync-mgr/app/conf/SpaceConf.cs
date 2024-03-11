using sync.app.conf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace sync.app
{
    public class SpaceConf
    {
        public RepEntry[] reps;
        public ViewEntry[] views;
        public SyncEntry[] syncs;
    }

    public class SpaceEntry
    {
        public string dir;
        public string name;

        public string entryPath(string id)
            => $"{dir}/{id}/args.conf";

        public void createEntry(string id, object entry)
        {
            var path = entryPath(id);
            path.pathDir().dirCreate();
            entry.jsonIndent().bakSaveTo(path);
        }

        string confPath
            => $"{dir}/space.conf";

        public void createConf()
        {
            if (confPath.fileExist())
                return;
            saveConf(new SpaceConf());
        }

        public void saveConf(SpaceConf conf)
            => conf.jsonIndent().bakSaveTo(confPath);

        public SpaceConf loadConf()
            => confPath.readJson<SpaceConf>();
    }

    public class SpaceContext
    {
        public SpaceContext(SpaceEntry sp)
            => space = sp;

        SpaceEntry space;

        public Reposit openRep(string id)
        {
            if (id == null)
                return null;
            if (reps.get(id, out var re))
            {
                return (space.entryPath(re.id).readText()
                    .obj(RepConf.cls(re.type)) as RepConf)
                    .openRep();
            }
            return null;
        }

        public View openView(string id)
        {
            if (id == null)
                return null;
            if (reps.get(id, out var re))
            {
                return (space.entryPath(re.id).readText()
                    .obj(RepConf.cls(re.type)) as RepConf)
                    .openView();
            }
            else if (views.get(id, out var ve))
            {
                return space.entryPath(ve.id).readText()
                    .obj<ViewConf>().openView(openView(ve.src));
            }
            return null;
        }

        SpaceConf cf;
        SpaceConf conf => cf ?? (cf = space.loadConf());

        Dictionary<string, RepEntry> repMap;
        Dictionary<string, RepEntry> reps => repMap
            ?? (repMap = conf.reps.toMap(r => r.id));

        Dictionary<string, ViewEntry> viewMap;
        Dictionary<string, ViewEntry> views => viewMap
            ?? (viewMap = conf.views.toMap(r => r.id));
    }

    public class RepEntry
    {
        public string id;
        public RepType type;
        public string name;
        public Point pos;
    }

    public class ViewEntry
    {
        public string id;
        public string src;
        public string name;
        public Point pos;
    }

    public enum SyncType
    {
        MasterSync,
        RoundSync,
    }

    public class SyncEntry
    {
        public string id;
        public string src;
        public string dst;
        public SyncType type;
        public string name;
        public bool check;
    }
}
