using SpatialAnalysis.Entity;
using SpatialAnalysis.Service.AddRecordExtend;
using System;
using System.IO;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 测试类，用于外部去访问非公开的类
    /// </summary>
    public class ForTest
    {
        public static void Entrance()
        {
            AddRecord add = new AddRecord();
            DirectoryInfo dir = new DirectoryInfo(@"");
            //RecordBean bean = add.SeeDirectory(dir, 1);
        }
    }
}
