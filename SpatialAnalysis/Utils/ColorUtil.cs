using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialAnalysis.Utils
{
    internal static class ColorUtil
    {
        /// <summary>
        /// 将RGB字符串转换为byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns>r,g,b</returns>
        public static byte[] GetRGB(string str)
        {
            byte[] rgb = new byte[3];
            for (int i = 0; i < 3; i++)
            {
                string oneByte = str.Substring(1 + i * 2, 2);
                rgb[i] = Convert.ToByte(oneByte, 16);
            }
            return rgb;
        }
    }
}
