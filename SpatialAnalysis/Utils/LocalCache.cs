using System;
using System.Runtime.Caching;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 本地缓存工具
    /// </summary>
    internal class LocalCache<R, P>
    {
        //数据加载器
        public delegate R CacheLoader(P param);
        private readonly CacheLoader loader;
        //保留时间
        public readonly int retentionTime;
        //缓存区
        private readonly MemoryCache memoryCache;
        /// <summary>
        /// 新建缓存
        /// </summary>
        /// <param name="minute">缓存时间(分钟)</param>
        /// <param name="loader">获取数据的方法</param>
        public LocalCache(int minute, CacheLoader loader)
        {
            retentionTime = minute;
            this.loader = loader;
            memoryCache = MemoryCache.Default;
        }
        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        public R Get(P param)
        {
            string hash = param.GetHashCode().ToString();
            if (memoryCache.Contains(hash))
                return (R)memoryCache.Get(hash);
            else
            {
                R result = loader(param);
                DateTimeOffset dateTime = DateTimeOffset.Now.AddMinutes(retentionTime);
                memoryCache.Set(hash, result, dateTime);
                Console.WriteLine("save");
                return result;
            }
        }
    }
}
