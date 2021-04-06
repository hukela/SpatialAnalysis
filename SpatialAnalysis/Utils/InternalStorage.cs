using System.Collections.Generic;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 内部存储
    /// </summary>
    class InternalStorage
    {
        //用于临时存储数据
        private static Dictionary<Domain, Dictionary<string, object>> dict = new Dictionary<Domain, Dictionary<string, object>>();
        //用于区分的域名
        public enum Domain
        {
        }
        public static void Build(Domain name)
        {
        }
        public static void Add(Domain name, string key, object value)
        {
        }
        public static object Get(Domain name, string key)
        {
            return null;
        }
        public static void Clean()
        {
        }
    }
}
