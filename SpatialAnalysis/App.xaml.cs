using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.IO.Xml;
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
            //设置主线程名称，方便调试
            Thread.CurrentThread.Name = "Main";
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += ExceptionHandling;
            //UI线程未捕获异常处理事件
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(ExceptionHandling);
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandling);
        }
        //自动启动和连接
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Log.Info("程序启动");
            bool[] autoPaeams = GetAutoParams();
            bool autoStartServer = autoPaeams[0];
            bool autoConnent = autoPaeams[1];
            if (autoStartServer)
            {
                Log.Info("打开服务");
                MySqlAction.StartServer();
            }
            if (autoConnent)
            {
                Log.Info("打开连接");
                MySqlAction.OpenConnect();
            }
        }
        //当程序关闭时执行
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.Info("程序退出");
            bool[] autoPaeams = GetAutoParams();
            bool autoStartServer = autoPaeams[0];
            bool autoConnent = autoPaeams[1];
            if (autoConnent || autoStartServer)
            {
                Log.Info("关闭连接");
                MySqlAction.CloseConnect();
            }
            if (autoStartServer)
            {
                Log.Info("关闭服务");
                MySqlAction.StopServer();
            }
        }
        //获取auto参数同时防止空异常
        private bool[] GetAutoParams()
        {
            bool[] autoPaeams = new bool[2];
            object auto;
            auto = XML.Map(XML.Params.autoStartServer);
            if (auto == null)
                autoPaeams[0] = false;
            else
                autoPaeams[0] = (bool)auto;
            auto = XML.Map(XML.Params.autoConnent);
            if (auto == null)
                autoPaeams[1] = false;
            else
                autoPaeams[1] = (bool)auto;
            return autoPaeams;
        }
        //异常处理
        private void ExceptionHandling(object seader, UnobservedTaskExceptionEventArgs eventArgs)
        {
            Log.Erroe("Task线程内未捕获异常:");
            Log.Add(eventArgs.Exception);
        }
        private void ExceptionHandling(object seader, UnhandledExceptionEventArgs eventArgs)
        {
            Log.Erroe("非UI线程未捕获异常:");
            Log.Add((Exception)eventArgs.ExceptionObject);
        }
        private void ExceptionHandling(object seader, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            Log.Erroe("UI线程未捕获异常处:");
            Log.Add(eventArgs.Exception);
        }
    }
}
