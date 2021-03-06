﻿using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace SpatialAnalysis.IO
{
    /// <summary>
    /// 安装和卸载MySql的相关操作
    /// </summary>
    class InstallAndUninstallMySql : Base
    {
        /// <summary>
        /// 解压MySql
        /// </summary>
        public static void Compress()
        {
            //使用第三方程序解压压缩包
            FastZip fastZip = new FastZip();
            string ZipPath = locolPath + @"\Data\mysql-8.0.22-winx64.zip";
            fastZip.ExtractZip(ZipPath, locolPath, null);
        }
        /// <summary>
        /// 删除MySql文件
        /// </summary>
        /// <returns>false代表文件不存在</returns>
        public static bool Remove()
        {
            string path = locolPath + @"\MySql";
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 修改配置文件
        /// </summary>
        public static void Configurate()
        {
            string path = locolPath + @"\MySql\my.ini";
            string mysqlPath = locolPath + @"\MySql";
            string config = TextFile.ReadAll(path, Encoding.UTF8);
            mysqlPath = mysqlPath.Replace(@"\", @"\\");
            config = config.Replace("[InstallPath]", mysqlPath);
            //MySql要求使用ANSI编码，这里使用ANSI的GB2312
            TextFile.WriteAll(path, Encoding.GetEncoding("GB2312"), config);
        }
        /// <summary>
        /// 建立安装脚本
        /// </summary>
        public static void BuildCmd()
        {
            string[] filePath = new string[]
            {
                locolPath + @"\Data\InstallMySQL.template.cmd",
                locolPath + @"\Data\InitializeMySQL.template.cmd"
            };
            string rooPath = locolPath.Substring(0, 2);
            string binPath = locolPath + @"\MySql\bin";
            foreach (string path in filePath)
            {
                string cmd = TextFile.ReadAll(path, Encoding.UTF8);
                cmd = cmd.Replace("[rootPath]", rooPath);
                cmd = cmd.Replace("[binPath]", binPath);
                //cmd要求使用的是GBK-936编码
                string outputPath = path.Replace(".template.cmd", ".cmd");
                TextFile.WriteAll(outputPath, Encoding.GetEncoding(936), cmd);
            }
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns>执行结果</returns>
        public static string[] Initialize()
        {
            string path = locolPath + @"\Data\InitializeMySQL.cmd";
            string[] strs = Cmd.RunCmdFile(path);
            File.Delete(path);
            return strs;
        }
        /// <summary>
        /// 安装数据库
        /// </summary>
        /// <returns>执行结果</returns>
        public static string[] Install()
        {
            string path = locolPath + @"\Data\InstallMySQL.cmd";
            string[] strs = Cmd.RunCmdFile(path);
            File.Delete(path);
            return strs;
        }
        /// <summary>
        /// 删除MySql服务
        /// </summary>
        /// <returns>false代表未找到服务</returns>
        public static bool DeleteService()
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach(ServiceController service in services)
            {
                if(service.ServiceName == "MySQL")
                {
                    //先关闭相关进程
                    Process[] arr = Process.GetProcessesByName("mysqld");
                    arr[0].Kill();
                    arr[1].Kill();
                    Thread.Sleep(1000);
                    //删除服务
                    Cmd cmd = new Cmd();
                    cmd.RunCmd("sc delete MySQL");
                    string[] message = cmd.Close();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 清理MySql注册表
        /// </summary>
        public static void DeleteRegedit()
        {
            string[] keyPath = new string[]
            {
                @"SYSTEM\ControlSet001\Services\EventLog\Application\MySQLD Service",
                @"SYSTEM\CurrentControlSet\Services\EventLog\Application\MySQLD Service",
                @"SYSTEM\Setup\FirstBoot\Services\MySQL"
            };
            RegistryKey key = Registry.LocalMachine;
            foreach(string path in keyPath)
                try { key.DeleteSubKey(path, true); }
                catch { /*屏蔽可能的异常*/ }
            key.Close();
        }
        /// <summary>
        /// 建立数据表
        /// </summary>
        public static void ChangePassword(string password)
        {
            MySqlAction.StartServer();
            MySqlAction.OpenConnect();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "ALTER USER root@localhost IDENTIFIED BY '" + password + "', root@localhost PASSWORD EXPIRE NEVER;";
                MySqlAction.Write(cmd);
            }
            MySqlAction.CloseConnect();
        }
        /// <summary>
        /// 建立数据库和相应表格
        /// </summary>
        public static void BuildTable()
        {
            string path = locolPath + @"\Data\BuildDatabase.sql";
            MySqlAction.OpenConnect();
            MySqlAction.ExecuteSqlFile(path);
        }
    }
}
