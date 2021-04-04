using System.Collections.Generic;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 内部存储
    /// </summary>
    class InternalStorage
    {
        //用于临时存储数据
        private static Dictionary<string, object> dict = new Dictionary<string, object>();
        //用于区分的域名
        public enum Domain
        {
            FileCount
        }
        public static void Add(Domain name, string key, object value)
        {
            dict[name + "-" + key] = value;
        }
        public static object Get(Domain name, string key)
        {
            return dict[name + "-" + key];
        }
        public static void Clean()
        {
            //释放掉原有的内存
            dict = new Dictionary<string, object>();
        }
    }
}
