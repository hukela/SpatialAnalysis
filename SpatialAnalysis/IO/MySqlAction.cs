using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialAnalysis.IO
{
    /// <summary>
    /// 针对MySql软件的各种操作
    /// </summary>
    class MySqlAction : Base
    {

        /// <summary>
        /// 实例化FastZip
        /// </summary>
        public static FastZip fz = new FastZip();
        /// <summary>
        /// 解压Zip
        /// </summary>
        /// <param name="DirPath">解压后存放路径</param>
        /// <param name="ZipPath">Zip的存放路径</param>
        /// <param name="ZipPWD">解压密码（null代表无密码）</param>
        /// <returns></returns>
        public static string Compress(string DirPath, string ZipPath, string ZipPWD)
        {
            string state = "Fail...";
            try
            {
                fz.Password = ZipPWD;
                fz.ExtractZip(ZipPath, DirPath, null);

                state = "Success !";
            }
            catch (Exception ex)
            {
                state += "," + ex.Message;
            }
            return state;
        }
    }
}
