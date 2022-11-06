using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace SpatialAnalysis.IO.Log
{
internal static class Log
{
    private static readonly string logPath = IoBase.localPath + @"\log";
    //在不读取全部内容的情况下在末尾添加内容(如果文件不存在，则新建一个) 默认UTF-8
    private static readonly StreamWriter writer = File.AppendText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

    /// <summary>
    /// 添加日志信息
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="i">堆栈回溯</param>
    public static void Info(string message, int i = 0)
    {
        AddLog(message, "info", 2 + i);
    }
    /// <summary>
    /// 添加警告级别的日志信息
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="i">堆栈回溯</param>
    public static void Warn(string message, int i = 0)
    {
        AddLog(message, "warn", 2 + i);
    }
    /// <summary>
    /// 添加错误级别的日志信息
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="i">堆栈回溯</param>
    public static void Error(string message, int i = 0)
    {
        AddLog(message, "error", 2 + i);
    }
    /// <summary>
    /// 添加错误级别的日志信息
    /// </summary>
    /// <param name="e">异常</param>
    /// <param name="i">堆栈回溯</param>
    public static void Add(Exception e, int i = 0)
    {
        StringBuilder builder = new StringBuilder();
        do
        {
            builder.AppendLine(e.Message + " (" + e.GetType() + ")")
                .AppendLine(e.StackTrace);
            e = e.InnerException;
        } while (e != null);
        AddLog(builder.ToString(), "error", i + 2);
    }
    // i: 回溯堆栈的索引
    private static void AddLog(string message, string type, int i)
    {
        DateTime now = DateTime.Now;
        string time = now.ToString("hh:mm:ss.fff");
        //获取添加日志的类的路径
        StackTrace trace = new StackTrace(true);
        MethodBase method = trace.GetFrame(i).GetMethod();
        string classPath = method.DeclaringType?.FullName;
        message = time + " " + type + " " + classPath + " [" + method + "]: " + message;
        writer.WriteLine(message);
        writer.Flush();
        Console.WriteLine(message);
    }
} }
