using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
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
            MftReader.openMft();
        }
    }
}
