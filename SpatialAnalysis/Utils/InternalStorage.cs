using System.Collections.Generic;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 内部存储
    /// </summary>
    internal class InternalStorage
    {
        //用于临时存储数据
        private static Dictionary<Domain, Dictionary<string, object>> dict = new Dictionary<Domain, Dictionary<string, object>>();
        //用于区分的域名
        public enum Domain
        {
            tag
        }
        public static void Build(Domain name)
        {
            dict.Add(name, new Dictionary<string, object>());
        }
        public static void Set(Domain name, string key, object value)
        {
            if (!dict.ContainsKey(name))
                Build(name);
            dict[name][key] = value;
        }
        public static object Get(Domain name, string key)
        {
            if (!dict.ContainsKey(name))
                return null;
            else if (dict[name].ContainsKey(key))
                return dict[name][key];
            else return null;
        }
        public static void Clean(Domain name)
        {
            dict[name] = null;
        }
    }
}
