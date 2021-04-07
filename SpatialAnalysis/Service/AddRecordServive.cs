﻿using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service.AddRecordExtend;
using System.Threading;

namespace SpatialAnalysis.Service
{
    class AddRecordServive
    {
        public static IncidentBean GetBean()
        {
            IncidentBean bean = IncidentMapper.GetLastBean();
            if (bean == null)
                bean = new IncidentBean();
            bean.Title = string.Empty;
            bean.Explain = string.Empty;
            return bean;
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="bean">事件bean</param>
        public static void AddIncident(IncidentBean bean)
        {
            AddRecord addRecord = new AddRecord();
            Log.Info("开始添加记录");
            ProgramWindow window = new ProgramWindow();
            object[] objs = { window, bean };
            //建立异步线程来记录全盘文件
            addRecord.thread = new Thread(addRecord.AddIncidentAsyn) { Name = "addRecord" };
            addRecord.thread.Start(objs);
            window.ShowDialog();
        }
        
    }
}