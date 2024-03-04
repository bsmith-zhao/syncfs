using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace util.ext
{
    public static class ListViewEx
    {
        public static int grpIdx(this ListViewItem item)
            => item.Group.Items.conv<ListViewItem>()
            .index(it => it == item);

        public static void drawOnce(this ListView ui, Action func)
        {
            try
            {
                ui.BeginUpdate();
                func();
            }
            finally
            {
                ui.EndUpdate();
            }
        }

        public static void autoSpan(this ListView ui)
        {
            if (ui.Columns.Count > 0)
                ui.Columns[ui.Columns.Count - 1].Width = -2;
        }

        public static void autoSpan(this ColumnHeader header)
        {
            header.Width = -2;
        }

        public static bool isEmpty(this ListView list)
        {
            return list.Items.Count == 0;
        }

        public static ListViewItem selItem(this ListView list)
            => list.SelectedItems.Count == 0 ? null
            : list.SelectedItems[0];

        public static ListViewItem icon(this ListViewItem it, string key)
        {
            it.ImageKey = key;
            return it;
        }

        public static bool selItem(this ListView list, out ListViewItem item)
        {
            return (item = list.selItem()) != null;
        }

        public static ListViewGroup group(this ListView list, string name)
        {
            var grp = list.Groups[name];
            if (grp == null)
                grp = list.Groups.Add(name, name);
            return grp;
        }

        public static ListViewItem label(this ListViewItem item, int idx, object value)
        {
            if (idx >= 0 && idx < item.SubItems.Count)
            {
                var sub = item.SubItems[idx];
                if (sub != null)
                    sub.Text = $"{value}";
            }
            return item;
        }

        public static string[] labels(this ListViewItem item)
            => item.SubItems.conv<ListViewItem.ListViewSubItem>()
                .conv(f => f.Text).ToArray();

        public static string label(this ListViewItem item, int idx)
        {
            if (idx >= 0 && idx < item.SubItems.Count)
                return item.SubItems[idx].Text;
            return null;
        }

        public static ListViewItem label(this ListViewItem item, string key, object value)
        {
            var sub = item.SubItems[key];
            if (sub != null)
                sub.Text = $"{value}";
            return item;
        }

        public static bool subItem(this ListViewItem item, string key, out ListViewItem.ListViewSubItem sub)
            => (sub = item.SubItems[key]) != null;

        public static void add(this ListViewGroup grp, ListViewItem item)
        {
            item.Group = grp;
            grp.ListView.Items.Add(item);
        }

        public static void delete(this ListView.ListViewItemCollection items)
            => items.newList<ListViewItem>().each(it => it.Remove());
    }
}
