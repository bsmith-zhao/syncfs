using link;
using sync.app;
using System.Windows.Forms;
using util;
using util.ext;
using util.rep;

namespace sync.ui
{
    public static class SyncManagerEx
    {
        public static bool isRep(this IItem it)
            => it?.Tag is RepTag;

        public static SyncTag syncTag(this TreeNode tn)
            => tn?.Tag as SyncTag;

        public static bool isSpace(this TreeNode tn)
            => tn?.Tag is SpaceTag;

        public static SpaceTag spaceTag(this TreeNode tn)
            => tn?.Tag as SpaceTag;

        public static bool isView(this IItem it)
            => it?.Tag is ViewTag;

        public static RepTag repTag(this IItem it)
            => it?.Tag as RepTag;

        public static ViewTag viewTag(this IItem it)
            => it?.Tag as ViewTag;

        public static IResTag idTag(this IItem it)
            => it?.Tag as IResTag;

        public static IConfTag confTag(this IItem it)
            => it?.Tag as IConfTag;

        public static SyncTag syncTag(this IItem it)
            => it?.Tag as SyncTag;

        public static IItem rootView(this IItem it)
        {
            var rn = it as INode;
            INode pn;
            while (baseView(rn, out pn) && pn != it)
                rn = pn;
            return rn;
        }

        static bool baseView(INode vn, out INode pn)
            => (pn = vn.Links.first(lk 
                => lk.Tag is ViewLinkTag 
                    && lk.Target == vn)?.Source) != null;

        public static bool canBeView(this IItem it)
            => it?.Tag is RepTag || it?.Tag is ViewTag;

        public static bool isRefed(this IItem it)
            => it is INode nd && nd.Links.exist(lk 
                => lk.Target == it && lk.Tag is ViewLinkTag);
    }
}
