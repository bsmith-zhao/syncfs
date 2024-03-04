using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using util.ext;

namespace link
{
    public partial class LinkView
    {
        LinkViewGraph graph = null;
        [Browsable(false)]
        public LinkViewGraph Graph
        {
            get => graph;
            set => value.update(ref graph, @new =>
            {
                action?.stop();
                action = null;
            },old => 
            {
                if (null != old)
                    old.view = null;
                if (null != graph)
                    graph.view = this;
                updateScroll();
                redraw();
            });
        }

        [Browsable(false)]
        public List<Item> Items => graph?.Items;
        [Browsable(false)]
        public List<Item> Picks => graph?.Picks;

        public event Action PicksChanged;
        public event Action<INode, INode> ItemsLinked;
        public event Action LinkStoped;
        public event Action<Point, Point> ItemsMoved;
        public event Action<Item> LabelModified;

        internal void OnItemsLinked(INode begin, INode end)
            => ItemsLinked?.Invoke(begin, end);

        internal void OnItemsMoved(Point begin, Point end)
            => ItemsMoved?.Invoke(begin, end);

        internal void OnLinkStoped()
            => LinkStoped?.Invoke();

        internal void OnLabelModified(Item it)
            => LabelModified?.Invoke(it);

        public void beginEdit(Item item)
        {
            if (!(item is IEditLabel lb) || !Items.has(item))
                return;
            if (action == editAct && editAct.isEdit(lb))
                return;

            action?.stop();
            action = editAct.start(lb);
        }
    }
}
