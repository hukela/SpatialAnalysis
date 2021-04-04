using System;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public class DateTimeUtil
    {
        private static DateTime _dtStart = new DateTime(1970, 1, 1, 8, 0, 0);

        /// <summary> 
        /// 根据时间戳获取时间
        /// </summary>  
        public static long GetTimeStamp(DateTime dateTime)
        {
            return Convert.ToInt64(dateTime.Subtract(_dtStart).TotalMilliseconds);
        }
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        public static long GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }
        /// <summary> 
        /// 根据时间戳获取时间
        /// </summary>  
        public static DateTime TimeStampToDateTime(long timeStamp)
        {
            if (timeStamp > 0)
            {
                return _dtStart.AddMilliseconds(timeStamp);
            }
            return DateTime.MinValue;
        }
    }
}
