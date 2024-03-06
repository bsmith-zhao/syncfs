using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using util.ext;

namespace util
{
    public static class Lang
    {
        public static string dir = $"{true.appDir()}/lang";
        public static string current = null;
        public static Dictionary<string, string> values = new Dictionary<string, string>();
        public static bool trace = false;

        static string CurrentPath => $"{dir}/current";

        static IEnumerable<string> defaultCodes()
        {
            if (CurrentPath.fileExist())
                yield return dir.tryget(() 
                    => CurrentPath.readText().Trim());
            var cu = CultureInfo.CurrentCulture;
            yield return cu.NativeName;
            yield return cu.EnglishName;
            yield return cu.Name;
        }

        static string langPath(string code)
            => $"{dir}/{code}.lang";

        public static void init()
        {
            dir.trydo(() => 
            {
                var code = defaultCodes().first(c => langPath(c).fileExist());
                if (code != null)
                    loadLang(code);
            });
        }

        public static string trans(this object obj, string item, params object[] args)
        {
            return trans(getKey(obj.GetType().Name, ref item), args);
        }

        public static string trans<T>(this string item, params object[] args)
            => typeof(T).trans(item, args);

        public static string trans(this Type type, string item, params object[] args)
        {
            return trans(getKey(type.Name, ref item), args);
        }

        static string getKey(string cls, ref string name)
        {
            if (name.Length > 0 && char.IsLower(name[0]))
                name = $"{char.ToUpper(name[0])}{name.Substring(1)}";
            return $"{cls}.{name}";
        }

        public static string trans(this string key, params object[] args)
        {
            if (values.TryGetValue(key, out string value))
            {
                if (args.Length > 0)
                    value = key.tryget(() => format(value, args));
                if (value != null)
                    return value;
            }
            value = args.Length > 0 
                ? $"{key}({string.Join(",", args)})" : key;
            if (trace)
                value.msg();
            return value;
        }

        static string format(string value, object[] args)
            => string.Format(value, args);

        public static void trans(this Control ui)
        {
            ui.layoutOnce(() => 
            {
                var cls = ui.GetType().Name;
                foreach (var fld in ui.GetType().GetFields())
                {
                    var obj = fld.GetValue(ui);
                    if (obj is UserControl uc)
                        trans(uc);
                    else if (obj is ToolStripItem tb)
                    {
                        tb.Text = transUIFld(cls, fld.Name, out var key);
                        tb.ToolTipText = values.TryGetValue($"{key}Tip", 
                                        out var tip) ? tip : tb.Text;
                    }
                    else if (obj is Control ct)
                        ct.Text = transUIFld(cls, fld.Name);
                    else if (obj is ColumnHeader ch)
                        ch.Text = transUIFld(cls, fld.Name);
                }
                if (ui is Form form)
                    form.Text = trans($"{cls}.Title");
            });
        }

        public static string transUIFld(this string cls, string name)
            => transUIFld(cls, name, out var key);

        public static string transUIFld(this string cls, 
            string name, out string key)
        {
            key = getKey(cls, ref name);
            if (values.TryGetValue(name, out var value))
                return value;
            if (values.TryGetValue(key, out value))
                return value;
            if (trace)
                key.msg();
            return defaultUIText(name);
        }

        static string defaultUIText(string name)
        {
            int idx = name.Length - 1;
            while (idx >= 0 && !char.IsUpper(name[idx]))
            {
                idx--;
            }
            return idx < 0 ? name : name.Substring(0, idx);
        }

        public static void initLang(this ToolStripDropDownItem menu,
            Action update)
        {
            menu.trydo(() =>
            {
                menu.Tag = update;
                menu.DropDownOpening += menuOpen;

                foreach (var p in Directory.EnumerateFiles(dir, "*.lang"))
                {
                    var code = Path.GetFileNameWithoutExtension(p).Trim();
                    menu.addLangItem(code, code);
                }
            });
        }

        static void addLangItem(this ToolStripDropDownItem menu,
            string code, string text)
        {
            var item = new ToolStripMenuItem()
            {
                Text = text,
                Tag = code,
            };
            item.Click += menuClick;
            menu.DropDownItems.Add(item);
        }

        static void menuOpen(object s, EventArgs e)
        {
            (s as ToolStripDropDownItem).DropDownItems
                .conv<ToolStripMenuItem>().each(it =>
                it.Checked = (it.Tag as string) == current);
        }

        static void menuClick(object s, EventArgs e)
        {
            e.trydo(() =>
            {
                var menu = s as ToolStripMenuItem;
                var code = menu.Tag as string;
                if (code == Lang.current)
                    return;

                loadLang(code);
                var update = menu.OwnerItem.Tag as Action;
                update();
                File.WriteAllText(CurrentPath, code);
            });
        }

        static void loadLang(string code)
        {
            try
            {
                Lang.values = langPath(code).kvLoad();
                Lang.current = code;
            }
            catch (Exception err)
            {
                string msg = $"Fail to load language: {err.Message}";
                throw new Exception(msg);
            }
        }

        //static void addLocaleItem(this ToolStripMenuItem menu)
        //{
        //    var item = new ToolStripMenuItem()
        //    {
        //        Text = GenLocales,
        //    };
        //    item.Click += localeClick;
        //    menu.DropDownItems.Add(item);
        //}

        //const string GenLocales = "Locales";

        //static void localeClick(object s, EventArgs e)
        //{
        //    e.trydo(() => 
        //    {
        //        var path = $"{dir}/locales.txt";
        //        using (var fout = File.CreateText(path))
        //        {
        //            fout.WriteLine($"Code,\tNativeName,\tEnglishName");
        //            foreach(var c in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        //            {
        //                fout.WriteLine($"{c.Name},\t{c.NativeName},\t{c.EnglishName}");
        //            }
        //        }
        //        $"{GenLocales}: {path}".msg();
        //    });
        //}
    }
}
