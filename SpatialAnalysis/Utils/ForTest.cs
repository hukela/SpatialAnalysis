﻿using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Service.AddRecordExtend;
using SpatialAnalysis.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            long a = -10;
            Console.WriteLine(Convert.ToUInt32(a));
        }
    }
}
