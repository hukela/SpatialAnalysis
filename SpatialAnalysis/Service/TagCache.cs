using System.Timers;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;

namespace SpatialAnalysis.Service
{
/// <summary>
/// 提供查询路径标签的服务
/// </summary>
internal static class TagCache
{
    private static DirTagBean[] tagCache; // 标签缓存对象
    private static readonly Timer clearTimer = new Timer(); // 清理缓存的定时器

    /// <summary>
    /// 设置或刷新标签标注的数据
    /// </summary>
    private static void InitTagCache()
    {
        DirTagBean[] beans = DirTagMapper.SelectAll();
        //根据标注长度进行排序
        int count = beans.Length;
        for (int r = 0; r < count; r++)
        {
            for (int i = 0; i < count - 1; i++)
            {
                string a = beans[i].Path;
                string b = beans[i + 1].Path;
                if (a.Length < b.Length)
                    (beans[i], beans[i + 1]) = (beans[i + 1], beans[i]);
            }
        }
        tagCache = beans;
    }

    /// <summary>
    /// 当内存中没有数据时，放入数据
    /// </summary>
    public static void CheckTagCache()
    {
        if (tagCache == null)
            InitTagCache();
        RefreshClearTimer();
    }

    /// <summary>
    /// 刷新缓存清理定时器
    /// </summary>
    private static void RefreshClearTimer()
    {
        if (clearTimer.Enabled)
            clearTimer.Stop();
        clearTimer.Interval = 10 * 60 * 1000; // 10分钟清理一次
        clearTimer.Elapsed += (s, e) => DeleteTagCache();
        clearTimer.Start();
    }

    /// <summary>
    /// 删除标签缓存
    /// </summary>
    public static void DeleteTagCache() { tagCache = null; }

    /// <summary>
    /// 通过路径获取标签bean
    /// </summary>
    /// <param name="path">查询路径</param>
    /// <param name="isThis">该路径是否是标签所标注的路径</param>
    public static TagBean GetTagByPath(string path, out bool isThis)
    {
        CheckTagCache();
        DirTagBean[] dirTags = tagCache;
        uint tagId = 0;
        isThis = false;
        foreach (DirTagBean dirTag in dirTags)
        {
            if (path.IndexOf(dirTag.Path) == -1)
                continue;
            tagId = dirTag.TagId;
            if (dirTag.Path == path)
                isThis = true;
            break;
        }
        return tagId == 0 ? null : TagMapper.SelectById(tagId);
    }
} }
