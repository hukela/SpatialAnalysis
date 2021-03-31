using SpatialAnalysis.Entity;
using SpatialAnalysis.Service.AddRecordPatter;
using System;
using System.Diagnostics;
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
            Process[] arr = Process.GetProcessesByName("mysqld");
            foreach (Process p in arr)
                Console.WriteLine(p.Id);
        }
    }
}
