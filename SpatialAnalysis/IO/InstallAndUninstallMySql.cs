﻿using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
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
            config = config.Replace("[InstallPath]", mysqlPath);
            TextFile.WriteAll(path, Encoding.UTF8, config);
        }
        /// <summary>
        /// 建立安装脚本
        /// </summary>
        public static void BuildCmd()
        {
            string path = locolPath + @"\Data\InstallMySQL.template.cmd";
            string outputPath = locolPath + @"\Data\InstallMySQL.cmd";
            string rooPath = locolPath.Substring(0, 2);
            string binPath = locolPath + @"\MySql\bin";
            string cmd = TextFile.ReadAll(path, Encoding.UTF8);
            cmd = cmd.Replace("[rootPath]", rooPath);
            cmd = cmd.Replace("[binPath]", binPath);
            //cmd要求使用的是GBK-936编码
            TextFile.WriteAll(outputPath, Encoding.GetEncoding(936), cmd);
        }
        public static string[] Install()
        {
            string path = locolPath + @"\Data\InstallMySQL.cmd";
            return Cmd.RunCmdFile(path);
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
                    //先关闭服务
                    if(service.Status == ServiceControllerStatus.Running)
                    {
                        service.Close();
                        Thread.Sleep(3000);
                    }
                    //删除服务
                    Cmd cmd = new Cmd();
                    cmd.RunCmd("sc delete MySQL");
                    string[] message = cmd.Close();
                    if (message[0] != "" && message[1] != "")
                        throw new ApplicationException("服务删除失败:\n" + message[0] + message[1]);
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
            key.DeleteSubKey(keyPath[2], true);
            key.Close();
        }
    }
}
