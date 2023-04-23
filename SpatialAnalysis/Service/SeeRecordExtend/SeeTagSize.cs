using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;

namespace SpatialAnalysis.Service.SeeRecordExtend
{
public class CountBean
{
    public uint fileCount, dirCount;
    public BigInteger size, spaceUsage;
}

/// <summary>
/// 查看当前标签内文件夹大小
/// </summary>
public class SeeTagSize
{
    /// <summary>
    /// 统计对应标签下的所有文件文件夹大小和占用空间(批量)
    /// </summary>
    /// <returns>key 标签id value 统计结果</returns>
    private static Dictionary<uint, CountBean> CountTagSize(uint incidentId, uint[] tagIds)
    {
        List<string> allPaths = new List<string>();
        // 统计所有需要查询的路径
        foreach (uint tagId in tagIds)
        {
            string[] tagPaths = DirTagCache.GetTagPathById(tagId);
            if (tagPaths == null) continue;
            HashSet<string> paths = new HashSet<string>(tagPaths);
            allPaths.AddRange(tagPaths);
        }
        //  todo 当前进度
    }
} }