using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Xml;
using SpatialAnalysis.IO.Log;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace SpatialAnalysis
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //判断数据库是否可用
            ServerState state = MySqlAction.State;
            if (state == ServerState.NoServer)
                XML.Map(XML.Params.haveLocalMySql, false);
            else
                XML.Map(XML.Params.haveLocalMySql, true);
            //设置主线程名称，方便调试
            Thread.CurrentThread.Name = "Main";
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += ExceptionHandling;
            //UI线程未捕获异常处理事件
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(ExceptionHandling);
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandling);
        }
        //异常处理
        private void ExceptionHandling(object seader, UnobservedTaskExceptionEventArgs eventArgs)
        {
            Log.erroe("Task线程内未捕获异常:");
            Log.add(eventArgs.Exception);
        }
        private void ExceptionHandling(object seader, UnhandledExceptionEventArgs eventArgs)
        {
            Log.erroe("非UI线程未捕获异常:");
            Log.add((Exception)eventArgs.ExceptionObject);
        }
        private void ExceptionHandling(object seader, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            Log.erroe("UI线程未捕获异常处:");
            Log.add(eventArgs.Exception);
        }
    }
}
