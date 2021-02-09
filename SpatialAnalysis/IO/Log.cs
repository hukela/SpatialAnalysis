using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SpatialAnalysis.IO.Log
{
    class Log: Base
    {
        private static readonly string logPath = locolPath + @"\log";
        /// <summary>
        /// 日志类型
        /// </summary>
        public enum Type
        {
            info,
            warn,
            error
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="messages">信息</param>
        public static void add(Type type, params string[] messages)
        {
            string message = "";
            foreach (string str in messages)
                message += str;
            addLog(message, type);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="e">异常</param>
        public static void add(Exception e)
        {
            string message = e.Message;
            if(e.StackTrace != null)
                message += "\n" + e.StackTrace;
            addLog(message, Type.error);
        }
        private static void addLog(string message, Type type)
        {
            DateTime now = DateTime.Now;
            string filePath = logPath + "\\" + now.ToString("yyyy-MM-dd") + ".txt";
            string time = now.ToString("hh:mm:ss.fff");
            //获取添加日志的类的路径
            StackTrace trace = new StackTrace(true);
            MethodBase method = trace.GetFrame(2).GetMethod();
            string classPath = method.DeclaringType.FullName;
            message = time + " " + type.ToString() + " " + classPath
                + " [" + method.ToString() + "]: " + message;
            //在不读取全部内容的情况下在末尾添加内容(如果文件不存在，则新建一个)
            //默认UTF-8
            StreamWriter writer = File.AppendText(filePath);
            writer.WriteLine(message);
            writer.Close();
        }
    }
}
