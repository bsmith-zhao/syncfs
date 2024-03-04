using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    //public class TreeViewEnhance
    //{
    //    public TreeView tree;

    //    public bool PreventDoubleClickToggle = true;
    //    public event TreeNodeMouseClickEventHandler NodeMouseDoubleClick;

    //    bool stopToggle = false;
    //    public TreeViewEnhance init()
    //    {
    //        if (PreventDoubleClickToggle)
    //        {
    //            tree.BeforeCollapse += (s, e) =>
    //            {
    //                if (stopToggle)
    //                    e.Cancel = true;
    //            };
    //            tree.BeforeExpand += (s, e) =>
    //            {
    //                if (stopToggle)
    //                    e.Cancel = true;
    //            };
    //            tree.MouseDown += (s, e) =>
    //            {
    //                if (e.Clicks == 2)
    //                    stopToggle = true;
    //            };
    //            tree.NodeMouseDoubleClick += (s, e) =>
    //            {
    //                stopToggle = false;

    //                NodeMouseDoubleClick?.Invoke(s, e);
    //            };
    //        }
    //        return this;
    //    }
    //}
}
