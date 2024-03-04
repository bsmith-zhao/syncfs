using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util.ext;

namespace link
{
    public class LinkViewGraph
    {
        public List<Item> Items = new List<Item>();
        public List<Item> Picks = new List<Item>();
        public Item LastPick => Picks.last();

        internal LinkView view;

        public Item addItem(Item item)
        {
            if (Items.Contains(item))
                return item;

            if (item is IEditLabel lb && view != null)
                lb.Label.Font = view.Font;

            Items.Add(item);

            redraw();

            return item;
        }

        public void delItem(Item item, Action<Item> notify = null)
        {
            if (!Items.Remove(item))
                return;

            Picks.Remove(item);

            if (item is Link lnk)
                lnk.detach();

            if (item is INode nd)
            {
                new List<Link>(nd.Links).each(lk =>
                {
                    lk.detach();
                    Items.Remove(lk);
                    Picks.Remove(lk);

                    notify?.Invoke(lk);
                });
            }

            notify?.Invoke(item);

            redraw();
        }

        public void pickItem(Item item)
        {
            int cnt = clearPicks(false);
            if (item is IPickable pk)
            {
                Picks.Add(item);
                pk.Picked = true;
            }

            if (cnt > 0 || Picks.Count > 0)
                redraw();
        }

        public int clearPicks(bool draw = true)
        {
            int cnt = Picks.Count;
            Picks.each(item => (item as IPickable).Picked = false);
            Picks.Clear();
            if (cnt > 0 && draw)
                redraw();
            return cnt;
        }

        bool draw = true;
        bool update = false;
        public void drawOnce(Action func)
        {
            try
            {
                draw = false;
                update = false;
                func();
            }
            finally
            {
                draw = true;
                if (update)
                    redraw();
            }
        }

        public void redraw()
        {
            update = true;
            if (!draw)
                return;
            view?.redraw(this);
        }
    }
}
