using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using System.Numerics;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Service.SeeRecordExtend;
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
    /// <param name="info">要查看的事件对象</param>
    /// <param name="isSpaceUsage">统计图是否展示占用空间</param>
    public static IncidentDetail BuildIncidentDetail(IncidentInfo info, bool isSpaceUsage)
    {
        // 参数准备
        TagBean[] tags = TagMapper.SelectRoot();
        int length = tags.Length;
        uint[] tagIds = new uint[length];
        for (int i = 0; i < length; i++)
            tagIds[i] = tags[i].Id;
        BigInteger totalSize = isSpaceUsage ? info.SpaceUsage : info.Size;
        BigInteger otherSize = totalSize;
        // 查询各标签的文件数 文件夹数 大小
        Dictionary<uint, CountBean> countMap = CountTagSize(info.Id, tagIds);
        // 生成饼图数据和页面信息
        ISeries<BigInteger>[] pieChart = new ISeries<BigInteger>[length + 1];
        for (int i = 0; i < length; i++)
        {
            TagBean tag = tags[i];
            CountBean count = countMap[tag.Id];
            BigInteger size = isSpaceUsage ? count.spaceUsage : count.size;
            pieChart[i] = BuildPieChart(tag, size, totalSize);
            otherSize -= size;
        }
        if (otherSize != 0)
            pieChart[length] = BuildPieChart(new TagBean
            {
                Name = "未分类", Color = "#D3D3D3"
            }, otherSize, totalSize);
        else
            pieChart = pieChart.Take(length).ToArray();
        return new IncidentDetail
        {
            Tag = new TagBean { Name = "标签" },
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
    /// 建立页面所需的数据
    /// </summary>
    /// <param name="tag">要查看事件下对应标签的</param>
    /// <param name="incidentId">要查看的事件</param>
    /// <param name="isSpaceUsage">统计图是否展示占用空间</param>
    public static IncidentDetail BuildIncidentDetail(TagBean tag, uint incidentId, bool isSpaceUsage)
    {
        // 准备参数
        TagBean[] tags = TagMapper.SelectChildren(tag.Id);
        bool noChildren = tags.Length == 0;
        if (noChildren)
            tags = new[] { tag };
        int length = tags.Length;
        uint[] tagIds = new uint[length];
        for (int i = 0; i < length; i++)
            tagIds[i] = tags[i].Id;
        // 查询标签的文件数、文件夹数、大小
        CountBean total = CountTagSize(incidentId, tag.Id);
        Dictionary<uint, CountBean> countMap = CountTagSize(incidentId, tagIds);
        // 生成饼图数据和页面信息
        BigInteger totalSize = isSpaceUsage ? total.spaceUsage : total.size;
        BigInteger otherSize = totalSize;
        ISeries<BigInteger>[] pieChart = new ISeries<BigInteger>[tags.Length + 1];
        for (int i = 0; i < length; i++)
        {
            TagBean tagBean = tags[i];
            CountBean count = countMap[tagBean.Id];
            BigInteger size = isSpaceUsage ? count.spaceUsage : count.size;
            pieChart[i] = BuildPieChart(tagBean, size, totalSize);
            otherSize -= size;
        }
        if (otherSize != 0)
            pieChart[length] = BuildPieChart(new TagBean
            {
                Name = "未分类", Color = "#D3D3D3"
            }, otherSize, totalSize);
        else
            pieChart = pieChart.Take(length).ToArray();
        return new IncidentDetail
        {
            Tag = tag,
            ChildrenTags = noChildren ? null : tags,
            paths = DirTagMapper.SelectPathByTagId(tag.Id),
            FileCount = total.fileCount,
            DirCount = total.dirCount,
            Size = total.size,
            SpaceUsage = total.spaceUsage,
            pieChart = pieChart
        };
    }

    /// <summary>
    /// 统计对应标签下的所有文件文件夹大小和占用空间
    /// </summary>
    private static CountBean CountTagSize(uint incidentId, uint tagId)
    {
        string[] paths = DirTagMapper.SelectPathByTagId(tagId);
        if (paths == null) return new CountBean();
        RecordBean[] records = RecordMapper.SelectByPaths(incidentId, paths);
        CountBean count = new CountBean();
        foreach (RecordBean record in records)
        {
            count.fileCount += record.FileCount;
            count.dirCount += record.DirCount;
            count.size += record.Size;
            count.spaceUsage += record.SpaceUsage;
        }
        return count;
    }

    /// <summary>
    /// 统计对应标签下的所有文件文件夹大小和占用空间(批量)
    /// </summary>
    /// <returns>key 标签id value 统计结果</returns>
    private static Dictionary<uint, CountBean> CountTagSize(uint incidentId, uint[] tagIds)
    {
        Dictionary<HashSet<string>, uint> tagPathMap = new Dictionary<HashSet<string>, uint>();
        List<string> allPaths = new List<string>();
        // 统计所有需要查询的路径
        foreach (uint tagId in tagIds)
        {
            string[] tagPaths = DirTagCache.GetTagPathById(tagId);
            if (tagPaths == null) continue;
            HashSet<string> paths = new HashSet<string>(tagPaths);
            allPaths.AddRange(tagPaths);
            tagPathMap.Add(paths, tagId);
        }
        // 通过路径查询对应记录
        RecordBean[] records = RecordMapper.SelectByPaths(incidentId, allPaths.ToArray());
        Dictionary<uint, CountBean> countMap = new Dictionary<uint, CountBean>(); // 批量查询 节省IO
        foreach (RecordBean record in records)
        {
            string path = record.Path;
            foreach (uint tagId in from set in tagPathMap.Keys where set.Contains(path) select tagPathMap[set])
            {
                if (!countMap.TryGetValue(tagId, out CountBean count))
                {
                    count = new CountBean();
                    countMap.Add(tagId, count);
                }
                count.fileCount += record.FileCount;
                count.dirCount += record.DirCount;
                count.size += record.Size;
                count.spaceUsage += record.SpaceUsage;
            }
        }
        // 填补没有路径的标签
        foreach (uint tagId in tagIds)
            if (!countMap.ContainsKey(tagId))
                countMap.Add(tagId, new CountBean());
        return countMap;
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
            Stroke = new SolidColorPaint(SKColors.Black),
            Fill = new SolidColorPaint(new SKColor(rgb[0], rgb[1], rgb[2])),
            Mapping = (v, point) => { point.PrimaryValue = (double)v; point.TertiaryValue = tag.Id; },
            DataLabelsFormatter = point => $"{percent:P2}",
            TooltipLabelFormatter = point => tag.Name + " " + ConversionUtil.StorageFormat(value, false),
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
        };
    }
} }