using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using util.ext;

namespace util
{
    public static class JSON
    {
        public static JsonSerializerSettings Conf = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> { new StringEnumConverter() },
            DateFormatString = "yyyy'/'MM'/'dd' 'HH':'mm':'ss'.'fff",
        };

        public static T obj<T>(this string json)
            => JsonConvert.DeserializeObject<T>(json, Conf);

        public static object obj(this string json, Type type)
            => JsonConvert.DeserializeObject(json, type, Conf);

        public static string jsonIndent(this object obj)
            => JsonConvert.SerializeObject(obj, Formatting.Indented, Conf);

        public static string json(this object obj, bool indent)
            => JsonConvert.SerializeObject(obj,
                indent ? Formatting.Indented : Formatting.None,
                Conf);

        public static string json(this object obj)
            => JsonConvert.SerializeObject(obj, Formatting.None, Conf);

        public static object jcopy(this object src, Type cls = null)
            => src.json().obj(cls ?? src.GetType());

        public static T jclone<T>(this T src)
            => src.json().obj<T>();

        public static T jpaste<T>(this T obj, string json)
        {
            JsonConvert.PopulateObject(json, obj, Conf);
            return obj;
        }

        public static T jpaste<T>(this T dst, object src)
        {
            JsonConvert.PopulateObject(src.json(), dst, Conf);
            return dst;
        }
    }

    //public static class JObjectEx
    //{
    //    public static T get<T>(this JObject js, string name)
    //    {
    //        return $"{js[name]}".conv<T>();
    //    }

    //    //public static string jget(this object obj, string path)
    //    //{
    //    //    if (obj is JObject json)
    //    //    {
    //    //        JToken tk = json.SelectToken(path);
    //    //        if (null != tk)
    //    //        {
    //    //            var value = tk.Value<object>();
    //    //            return value?.ToString();
    //    //        }
    //    //    }
    //    //    return null;
    //    //}
    //}
}
