using SpatialAnalysis.MyWindow;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.IO.Xml;
using System.Threading;
using System;
using System.Windows;

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
                //刷新连接配置
                SaveInstall(password);
                MySqlAction.RefreshCon();
                program.WriteLine("初始化数据库...");
                //因为初始密码有很多限制，所以这里要修改一下密码
                password = "123456";
                InstallAndUninstallMySql.ChangePassword(password);
                //刷新连接配置
                XML.Map(XML.Params.password, password);
                MySqlAction.RefreshCon();
                //向数据库中建立相应库和表格
                InstallAndUninstallMySql.BuildTable();
                XML.Map(XML.Params.database, "spatial_analysis");
                program.WriteLine("安装完成");
                Log.Info("安装数据库完成");
            }
            catch (Exception e)
            {
                program.WriteLine("错误:");
                program.WriteLine(e.Message);
                Log.Error("MySql安装失败");
                Log.Add(e);
            }
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
            RefreshIsCanUse();
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
            MySqlAction.CloseConnect();
            try
            {
                program.WriteLine("卸载服务...");
                if (!InstallAndUninstallMySql.DeleteService())
                    program.WriteLine("未检测到服务");
                program.WriteLine("清理注册表...");
                InstallAndUninstallMySql.DeleteRegedit();
                XML.Map(XML.Params.database, null);
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
                Log.Error("MySql卸载失败");
                Log.Add(e);
            }
            RefreshIsCanUse();
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
        //刷新应用对于连接的识别状态
        private void RefreshIsCanUse()
        {
            Application app = Application.Current;
            app.Dispatcher.Invoke(delegate()
            {
                MainWindow main = (MainWindow)app.MainWindow;
                main.IsCanUse();
            });
        }
    }
}
