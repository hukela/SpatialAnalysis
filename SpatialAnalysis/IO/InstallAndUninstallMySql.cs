using ICSharpCode.SharpZipLib.Zip;
using System.IO;

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
            if(File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
