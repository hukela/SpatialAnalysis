using System;
using LiveChartsCore;
using System.Numerics;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Utils;

namespace SpatialAnalysis.Service
{
/// <summary>
/// 记录详情展示逻辑
/// </summary>
internal static class RecordDetailService
{
    /// <summary>
    /// 建立页面所需的数据
    /// </summary>
    public static IncidentDetail BuildIncidentDetail(IncidentInfo info, bool isSpaceUsage)
    {
        TagBean[] tags = TagMapper.SelectRoot();
        int length = tags.Length;
        BigInteger totalSize = isSpaceUsage ? info.SpaceUsage : info.Size;
        BigInteger otherSize = totalSize;
        ISeries<BigInteger>[] pieChart = new ISeries<BigInteger>[length + 1];
        for (int i = 0; i < length; i++)
        {
            TagBean tag = tags[i];
            BigInteger size = CountSize(tag.Id, info.Id, isSpaceUsage);
            otherSize -= size;
            pieChart[i] = BuildPieChart(tag, size, totalSize);
        }
        pieChart[length] = BuildPieChart(new TagBean() { Name = "未分类", Color = "#D3D3D3" }, otherSize, totalSize);
        return new IncidentDetail()
        {
            Tag = new TagBean() { Name = "标签" },
            ChildrenTags = tags,
            paths = Array.Empty<string>(),
            FileCount = info.FileCount,
            DirCount = info.DirCount,
            Size = info.Size,
            SpaceUsage = info.SpaceUsage,
            pieChart = pieChart
        };
    }

    /// <summary>
    /// 通过标签id获取对应大小或占用空间
    /// </summary>
    private static BigInteger CountSize(uint tagId, uint incidentId, bool isSpaceUsage)
    {
        string[] paths = DirTagMapper.selectPathByTagId(tagId);
        BigInteger size = BigInteger.Zero;
        foreach (string path in paths)
        {
            RecordBean record = RecordMapper.SelectByPath(incidentId, path);
            if (record == null)
                continue;
            if (isSpaceUsage)
                size += record.SpaceUsage;
            else
                size += record.Size;
        }
        return size;
    }

    /// <summary>
    /// 通过标签获取对应饼图数据
    /// </summary>
    private static PieSeries<BigInteger> BuildPieChart(TagBean tag, BigInteger value, BigInteger total)
    {
        float percent = ((float)value / (float)total); // 计算百分比
        byte[] rgb = ConversionUtil.HexToRgb(tag.Color);
        return new PieSeries<BigInteger>
        {
            Name = tag.Name,
            Values = new []{ value },
            Fill = new SolidColorPaint(new SKColor(rgb[0], rgb[1], rgb[2])),
            Mapping = (v, point) => { point.PrimaryValue = (double)v; },
            DataLabelsFormatter = point => $"{percent:P2}",
            TooltipLabelFormatter = point => tag.Name + " " + ConversionUtil.StorageFormat(value, false),
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
        };
    }
} }