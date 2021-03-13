﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SpatialAnalysis.IO.Log
{
    class Log : Base
    {
        private static readonly string logPath = locolPath + @"\log";
        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="messages">信息</param>
        public static void Info(params string[] messages)
        {
            string message = "";
            foreach (string str in messages)
                message += str;
            AddLog(message, "info", 2);
        }
        /// <summary>
        /// 添加警告级别的日志信息
        /// </summary>
        /// <param name="messages">信息</param>
        public static void Warn(params string[] messages)
        {
            string message = "";
            foreach (string str in messages)
                message += str;
            AddLog(message, "warn", 2);
        }
        /// <summary>
        /// 添加错误级别的日志信息
        /// </summary>
        /// <param name="messages">信息</param>
        public static void Erroe(params string[] messages)
        {
            string message = "";
            foreach (string str in messages)
                message += str;
            AddLog(message, "error", 2);
        }
        /// <summary>
        /// 添加错误级别的日志信息
        /// </summary>
        /// <param name="e">异常</param>
        public static void Add(Exception e, int i = 0)
        {
            string message = e.Message;
            if(e.StackTrace != null)
                message += "\n" + e.StackTrace;
            AddLog(message, "error", i + 2);
        }
        // i: 回溯堆栈的索引
        private static void AddLog(string message, string type, int i)
        {
            DateTime now = DateTime.Now;
            string filePath = logPath + "\\" + now.ToString("yyyy-MM-dd") + ".txt";
            string time = now.ToString("hh:mm:ss.fff");
            //获取添加日志的类的路径
            StackTrace trace = new StackTrace(true);
            MethodBase method = trace.GetFrame(i).GetMethod();
            string classPath = method.DeclaringType.FullName;
            message = time + " " + type + " " + classPath
                + " [" + method.ToString() + "]: " + message;
            //在不读取全部内容的情况下在末尾添加内容(如果文件不存在，则新建一个)
            //默认UTF-8
            StreamWriter writer = File.AppendText(filePath);
            writer.WriteLine(message);
            writer.Close();
        }
    }
}
