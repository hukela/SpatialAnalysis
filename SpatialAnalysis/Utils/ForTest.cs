using SpatialAnalysis.Entity;
using SpatialAnalysis.Service.AddRecordPatter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

namespace SpatialAnalysis.Utils
{
    /// <summary>
    /// 测试类，用于外部去访问非公开的类
    /// </summary>
    public class ForTest
    {
        public static void Entrance()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["1"] = "a";
            Console.WriteLine(dict["1"]);
            dict["1"] = "b";
            Console.WriteLine(dict["1"]);
        }
    }
}
