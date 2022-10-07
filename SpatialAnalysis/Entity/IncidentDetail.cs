using LiveChartsCore;
using System.Numerics;

namespace SpatialAnalysis.Entity
{
/// <summary>
/// 记录详情页 对应标签信息
/// </summary>
internal class IncidentDetail
{
    public TagBean Tag { get; set; }

    public TagBean[] ChildrenTags { get; set; }

    public string[] paths { get; set; }

    public BigInteger Size { get; set; }

    public BigInteger SpaceUsage { get; set; }

    public ISeries<BigInteger>[] pieChart { get; set; } // 饼图数据
} }