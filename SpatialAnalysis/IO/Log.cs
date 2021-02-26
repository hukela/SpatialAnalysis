using System;
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
            addLog(message, "info");
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
            addLog(message, "warn");
        }
        /// <summary>
        /// 添加错误级别的日志信息
        /// </summary>
        /// <param name="messages">信息</param>
        public static void erroe(params string[] messages)
        {
            string message = "";
            foreach (string str in messages)
                message += str;
            addLog(message, "error");
        }
        /// <summary>
        /// 添加错误级别的日志信息
        /// </summary>
        /// <param name="e">异常</param>
        public static void add(Exception e)
        {
            string message = e.Message;
            if(e.StackTrace != null)
                message += "\n" + e.StackTrace;
            addLog(message, "error");
        }
        private static void addLog(string message, string type)
        {
            DateTime now = DateTime.Now;
            string filePath = logPath + "\\" + now.ToString("yyyy-MM-dd") + ".txt";
            string time = now.ToString("hh:mm:ss.fff");
            //获取添加日志的类的路径
            StackTrace trace = new StackTrace(true);
            MethodBase method = trace.GetFrame(2).GetMethod();
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
