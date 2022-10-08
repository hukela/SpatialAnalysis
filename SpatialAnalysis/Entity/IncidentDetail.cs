using LiveChartsCore;
using System.Numerics;
using SpatialAnalysis.Utils;

namespace SpatialAnalysis.Entity
{
/// <summary>
/// 记录详情页 对应标签信息
/// </summary>
internal class IncidentDetail
{
    public TagBean Tag { get; set; }

    public TagBean[] ChildrenTags { get; set; }

    public string TagName => '[' + Tag.Name + ']';

    public uint FileCount { get; set; }
    public string FileCountFormatted => "文件数：" + FileCount;

    public uint DirCount { get; set; }
    public string DirCountFormatted => "文件夹数：" + DirCount;

    public BigInteger Size { get; set; }
    public string SizeFormatted => "大小：" + ConversionUtil.StorageFormat(Size, false);

    public BigInteger SpaceUsage { get; set; }
    public string SpaceUsageFormatted => "占用空间：" + ConversionUtil.StorageFormat(SpaceUsage, false);

    public ISeries<BigInteger>[] pieChart { get; set; } // 饼图数据

    public string[] paths { get; set; } // 路径
} }