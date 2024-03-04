using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util.ext;

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

                    if (anyTrue(limitValue(item), unifyPath(item)))
                        ui.Refresh();

                    notify?.Invoke(s, e);
                });
            };

            ui.MouseWheel += (s, e) =>
            {
                e.trydo((Action)(() => 
                {
                    var item = ui.SelectedGridItem;
                    if (item == null || item.field() == null
                        || !item.attr<EditByWheel>(out var wheel, out var prop, out var owner)
                        || !wheel.modify(item.Value, e.Delta > 0, out var dst))
                        return;

                    var old = item.Value;
                    if (item.hasAttr<ByteSize>())
                        prop.SetValue(owner, $"{dst}".byteSize().byteSize());
                    else
                        prop.SetValue(owner, dst);
                    limitValue(item);

                    if ($"{old}" == $"{item.Value}")
                        return;

                    ui.Refresh();

                    notify?.Invoke(ui, new PropertyValueChangedEventArgs(item, old));
                }));
            };
        }

        static bool limitValue(GridItem item)
            => item.attr<RangeLimit>(out var rng, out var prop, out var owner)
                && rng.limit(item.Value, 
                    dst => prop.SetValue(owner, dst));

        static bool unifyPath(GridItem item)
        {
            if (!item.attr<UnifyPath>(out var rng, out var prop, out var owner))
                return false;
            var path = prop.GetValue(owner) as string;
            var newPath = path.pathUnify();
            if (path == newPath)
                return false;
            prop.SetValue(owner, newPath);
            return true;
        }

        static bool anyTrue(params bool[] vs)
            => vs.exist(v => v);

        public static bool hasAttr<T>(this GridItem item)
            where T : Attribute
            => item.attr<T>(out object owner, out var prop) != null;

        public static bool attr<T>(this GridItem item, out T meta, 
            out PropertyInfo prop, out object owner)
            where T : Attribute
            => (meta = attr<T>(item, out owner, out prop)) != null;

        public static T attr<T>(this GridItem item, out object owner, 
            out PropertyInfo prop)
            where T : Attribute
            => (prop = (owner = item.owner()).GetType().GetProperty(item.field()))
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
