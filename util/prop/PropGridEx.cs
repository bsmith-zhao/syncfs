using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util.ext;
using util.prop.edit;

namespace util.prop
{
    public static class PropGridEx
    {
        public static void enhanceDesc(this PropertyGrid grid, 
            TextBoxBase desc)
        {
            grid.SelectedGridItemChanged += (s, e) =>
            {
                e.trydo(() => 
                {
                    var item = e.NewSelection;
                    var owner = item.owner();
                    var name = item.field();
                    if (name == null || owner == null || owner is Array)
                        desc.Text = "";
                    else
                    {
                        desc.Text = (item.attr<DescRefer>()?.referTo() 
                                    ?? owner.GetType().propOwner(name))
                                    .trans(item.Label);
                    }
                });
            };
            grid.SelectedObjectsChanged += (s, e) => 
            {
                desc.Text = "";
            };
        }

        static Type propOwner(this Type cls, string name)
        {
            while (cls.GetProperty(name,
                BindingFlags.Public
                | BindingFlags.DeclaredOnly
                | BindingFlags.Instance) == null
                && cls.BaseType != null)
            {
                cls = cls.BaseType;
            }
            return cls;
        }

        public static void enhanceEdit(this PropertyGrid ui, 
            Action<object, PropertyValueChangedEventArgs> notify = null)
        {
            ui.PropertyValueChanged += (s, e) =>
            {
                e.trydo(() => 
                {
                    var item = e.ChangedItem;

                    if (!item.attr<AdjustValue>(out var adj,
                                out var prop, out var owner))
                        return;

                    var src = item.Value;
                    var dst = adj.adjust(src);
                    if (src == dst)
                        return;

                    prop.SetValue(owner, dst);
                    ui.Refresh();

                    notify?.Invoke(s, e);
                });
            };

            ui.MouseWheel += (s, e) =>
            {
                e.trydo(() => 
                {
                    var item = ui.SelectedGridItem;
                    if (item.field() == null
                        || !item.attr<WheelEdit>(out var wheel, 
                                                out var prop, 
                                                out var owner)
                        || !wheel.next(item.Value, 
                                        out var dst, e.Delta > 0))
                        return;

                    var old = item.Value;
                    prop.SetValue(owner, dst);
                    ui.Refresh();

                    notify?.Invoke(ui, new PropertyValueChangedEventArgs(item, old));
                });
            };
        }

        public static bool hasAttr<T>(this GridItem item)
            where T : Attribute
            => item.attr<T>(out object owner, out var prop) != null;

        public static bool attr<T>(this GridItem item, out T attr, 
            out PropertyInfo prop, out object owner)
            where T : Attribute
            => (attr = attr<T>(item, out owner, out prop)) != null;

        public static T attr<T>(this GridItem item, out object owner, 
            out PropertyInfo prop)
            where T : Attribute
            => (prop = (owner = item.owner())
            .GetType().GetProperty(item.field()))
            ?.GetCustomAttribute<T>();

        public static T attr<T>(this GridItem item)
            where T : Attribute
            => attr<T>(item, out object owner, out var prop);

        public static object owner(this GridItem item)
        {
            object obj = null;
            while (item != null && (obj = item.Parent.Value) == null)
                item = item.Parent;
            return obj;
        }

        public static string field(this GridItem item)
            => item?.PropertyDescriptor?.Name;
    }
}
