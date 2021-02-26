﻿using SpatialAnalysis.MyWindow;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using System.Threading;
using System;

namespace SpatialAnalysis.Service
{
    /// <summary>
    /// 安装和卸载MySql
    /// </summary>
    class InstallAndUninstall
    {
        /// <summary>
        /// 开始安装
        /// </summary>
        public void BeginInstall()
        {
            Log.Info("开始安装MySql");
            //只有主线程可以新建窗口
            ProgramWindow program = new ProgramWindow();
            //需要较长的时间来安装，防止主线程因长时间未响应而被误认为软件卡死
            //所以这里设置一个线程专门异步安装数据库
            Thread thread = new Thread(AsynInstall);
            thread.Start(program);
            program.ShowDialog();
        }
        private void AsynInstall(object obj)
        {
            //设置线程名称，方便调试。
            Thread.CurrentThread.Name = "installMySql";
            //线程传递参数只能使用object
            ProgramWindow program = (ProgramWindow)obj;
            //等待一秒，等待program窗口完全打开
            Thread.Sleep(1000);
            program.WriteLine("开始安装：");
            try
            {
                program.WriteLine("解压中...");
                InstallAndUninstallMySql.Compress();
            }
            catch (Exception e)
            {
                program.WriteLine("错误:");
                program.WriteLine(e.Message);
                Log.erroe("MySql安装失败");
                Log.add(e);
            }
            program.RunOver();
        }
        public void BeginUnInstall()
        {
            Log.Info("开始卸载MySql");
            ProgramWindow program = new ProgramWindow();
            Thread thread = new Thread(AsynUninstall);
            thread.Start(program);
            program.ShowDialog();
        }
        private void AsynUninstall(object obj)
        {
            Thread.CurrentThread.Name = "uninstallMySql";
            ProgramWindow program = (ProgramWindow)obj;
            Thread.Sleep(1000);
            program.WriteLine("开始卸载");
            try
            {
                program.WriteLine("删除文件...");
                InstallAndUninstallMySql.Remove();
            }
            catch (Exception e)
            {
                program.WriteLine("错误:");
                program.WriteLine(e.Message);
                Log.erroe("MySql安装失败");
                Log.add(e);
            }
            program.RunOver();
        }
    }
}
