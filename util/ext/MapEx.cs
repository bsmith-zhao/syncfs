using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.ext
{
    public static class MapEx
    {
        public static bool conv<K, V, R>(this Dictionary<K, V> map, K key, out R value, R def)
        {
            try
            {
                var type = typeof(R);
                if (type.IsEnum)
                    value = (R)Enum.Parse(type, map[key].ToString(), true);
                else
                    value = (R)Convert.ChangeType(map[key], type);
                return true;
            }
            catch { }
            value = def;
            return false;
        }

        public static Dictionary<K, V> pick<K, V>(this Dictionary<K, V> map, K key, out V value, Func<V> create)
        {
            map = map ?? new Dictionary<K, V>();

            if (map.TryGetValue(key, out value))
                return map;

            map[key] = (value = create());
            return map;
        }

        public static V get<K, V>(this K key, ref Dictionary<K, V> map, Func<V> create)
        {
            map = map ?? new Dictionary<K, V>();

            if (map.TryGetValue(key, out var value))
                return value;

            return map[key] = (value = create());
        }

        public static bool get<K, V>(this Dictionary<K, V> map, K key, out V value)
        {
            if (null != map)
                return map.TryGetValue(key, out value);

            value = default(V);
            return false;
        }

        public static V get<K, V>(this Dictionary<K, V> map, K key)
        {
            map.get(key, out V value);
            return value;
        }

        public static Dictionary<K, V> set<K, V>(this Dictionary<K, V> map, K key, V value)
        {
            map = map ?? new Dictionary<K, V>();
            map[key] = value;
            return map;
        }

        public static bool pop<K, V>(this Dictionary<K, V> map, K key, out V value)
        {
            if (map.TryGetValue(key, out value))
            {
                map.Remove(key);
                return true;
            }
            return false;
        }

        public static V pop<K, V>(this Dictionary<K, V> map, K key)
        {
            if (map.TryGetValue(key, out V value))
            {
                map.Remove(key);
            }
            return value;
        }
    }
}
