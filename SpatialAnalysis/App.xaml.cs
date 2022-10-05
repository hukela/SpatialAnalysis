using SpatialAnalysis.Service;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using SpatialAnalysis.IO.Log;

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
        DispatcherUnhandledException += ExceptionHandling;
        //非UI线程未捕获异常处理事件
        AppDomain.CurrentDomain.UnhandledException += ExceptionHandling;
    }
    //当程序启动时执行
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Log.Info("程序启动");
        StartupAndExit.ApplicationStartup();
    }
    //当程序关闭时执行
    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Log.Info("程序关闭");
        StartupAndExit.ApplicationExit();
    }
    //异常处理
    private void ExceptionHandling(object seader, UnobservedTaskExceptionEventArgs eventArgs)
    {
        Log.Error("Task线程内未捕获异常:");
        Log.Add(eventArgs.Exception);
    }
    private void ExceptionHandling(object seader, UnhandledExceptionEventArgs eventArgs)
    {
        Log.Error("非UI线程未捕获异常:");
        Log.Add((Exception)eventArgs.ExceptionObject);
    }
    private void ExceptionHandling(object seader, DispatcherUnhandledExceptionEventArgs eventArgs)
    {
        Log.Error("UI线程未捕获异常处:");
        Log.Add(eventArgs.Exception);
    }
} }
