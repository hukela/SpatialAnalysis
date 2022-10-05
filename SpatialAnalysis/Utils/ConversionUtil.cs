using System;
using System.Numerics;

namespace SpatialAnalysis.Utils
{
/// <summary>
/// 存储单位的换算
/// </summary>
internal static class ConversionUtil
{
    /// <summary>
    /// 将存大小数据换算成合适的单位
    /// </summary>
    /// <param name="bigInt">数据(字节)</param>
    /// <param name="haveSign">是否带有正号</param>
    public static string StorageFormat(BigInteger bigInt, bool haveSign)
    {
        //最后发现该数值最高也就12位，根本不需要用BigInteger
        //取log不能为0
        if (bigInt == BigInteger.Zero)
            return "0.00B";
        bool isNegative = bigInt < 0;
        //取绝对值，因为log不能处理负数
        double size = Math.Abs(double.Parse(bigInt.ToString()));
        int level = Convert.ToInt32(Math.Floor(Math.Log(size, 2) / 10));
        double result = size / Math.Pow(1024, level);
        if (isNegative)
            result = result * -1;
        string format = result.ToString("0.00");
        if (!isNegative && haveSign)
            format = string.Concat('+', format);
        switch (level)
        {
            case 0:
                format += "B";
                break;
            case 1:
                format += "KB";
                break;
            case 2:
                format += "MB";
                break;
            case 3:
                format += "GB";
                break;
            case 4:
                format += "TB";
                break;
        }
        return format;
    }
} }
