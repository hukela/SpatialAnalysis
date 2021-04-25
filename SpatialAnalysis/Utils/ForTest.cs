using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Service.AddRecordExtend;
using SpatialAnalysis.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Security.AccessControl;
using System.Threading;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 测试类，用于外部去访问非公开的类
    /// </summary>
    public class ForTest
    {
        public static void Entrance()
        {
            DirectoryInfo info = new DirectoryInfo(@"C:\Users\All Users\");
            Console.WriteLine(info.FullName);
            foreach (DirectoryInfo dir in info.GetDirectories())
            {
                Console.WriteLine(dir.FullName);
            }
        }
    }
}
