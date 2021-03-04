using SpatialAnalysis.MyWindow;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.IO.Xml;
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
            string[] initialize = null;
            string[] install = null;
            try
            {
                program.WriteLine("解压压缩包...");
                InstallAndUninstallMySql.Compress();
                program.WriteLine("修改配置文件...");
                InstallAndUninstallMySql.Configurate();
                program.WriteLine("生成安装脚本...");
                InstallAndUninstallMySql.BuildCmd();
                program.WriteLine("生成数据库...");
                initialize = InstallAndUninstallMySql.Initialize();
                string password = GetPasswd(initialize[1]);
                program.WriteLine("安装数据库服务...");
                install = InstallAndUninstallMySql.Install();
                SaveInstall(password);
                program.WriteLine("初始化数据库...");
                InstallAndUninstallMySql.ChangePassword();
                //program.WriteLine("安装完成");
                //Log.Info("安装数据库完成");
            }
            catch (Exception e)
            {
                program.WriteLine("错误:");
                program.WriteLine(e.Message);
                Log.erroe("MySql安装失败");
                Log.add(e);
            }
            finally
            {
                if (initialize != null)
                {
                    //只有主线程可以新建窗口
                    TextWindow textWindow;
                    textWindow = App.Current.Dispatcher.Invoke(delegate ()
                    {
                        textWindow = new TextWindow();
                        textWindow.Show();
                        return textWindow;
                    });
                    textWindow.WriteLine("建立数据库:");
                    textWindow.WriteLine(initialize[0]);
                    textWindow.WriteLine(initialize[1]);
                    if (install != null)
                    {
                        textWindow.WriteLine("安装数据库:");
                        textWindow.WriteLine(install[0]);
                        textWindow.WriteLine(install[1]);
                    }
                }
            }
                program.RunOver();
        }
        /// <summary>
        /// 开始卸载
        /// </summary>
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
                program.WriteLine("卸载服务...");
                if (!InstallAndUninstallMySql.DeleteService())
                    program.WriteLine("未检测到服务");
                program.WriteLine("清理注册表...");
                InstallAndUninstallMySql.DeleteRegedit();
                program.WriteLine("删除文件...");
                if (!InstallAndUninstallMySql.Remove())
                    program.WriteLine("未检测到文件");
                program.WriteLine("卸载完成");
                Log.Info("数据库卸载完成");
            }
            catch (Exception e)
            {
                program.WriteLine("错误:");
                program.WriteLine(e.Message);
                Log.erroe("MySql卸载失败");
                Log.add(e);
            }
            program.RunOver();
        }

        //获取密码
        private string GetPasswd(string message)
        {
            int i = message.IndexOf("root@localhost:") + 16;
            if (i == -1)
                throw new ApplicationException("数据库初始化失败");
            string passwd = message.Substring(i, message.Length - i);
            return passwd.Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }
        //存储相关安装数据
        private void SaveInstall(string password)
        {
            XML.Map(XML.Params.server, "localhost");
            XML.Map(XML.Params.port, 3306);
            XML.Map(XML.Params.user, "root");
            XML.Map(XML.Params.password, password);
        }
    }
}
