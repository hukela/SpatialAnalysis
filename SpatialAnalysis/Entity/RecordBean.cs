using System;
using System.Numerics;

namespace SpatialAnalysis.Entity 
{
/// <summary>
/// 记录表数据实体
/// </summary>
internal class RecordBean
{
    public ulong Id { get; set; }
    public ulong ParentId { get; set; }
    public uint FromIncidentId { get; set; }
    public uint TargetIncidentId { get; set; }
    public string Path { get; set; }
    public uint Plies { get; set; }
    public BigInteger Size { get; set; }
    public BigInteger SpaceUsage { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime ModifyTime { get; set; }
    public DateTime VisitTime { get; set; }
    public string Owner { get; set; }
    //错误编码，对应错误类型在RecordExCode枚举类中
    public int ExceptionCode { get; set; }
    public uint FileCount { get; set; }
    public uint DirCount { get; set; }
    //是否是文件夹
    public bool IsFile { get; set; }
    //当前bean是否被改变
    public bool IsChange { get; set; }
    //获取文件夹的名称
    public string Name
    {
        get
        {
            if (ParentId == 0)
                return Path;
            else
                return Path.Substring(Path.LastIndexOf('\\') + 1);
        }
    }
    //获取该文件夹的位置
    public string Location
    {
        get
        {
            if (ParentId == 0)
                return string.Empty;
            else
                return Path.Remove(Path.LastIndexOf('\\'));
        }
    }
    /// <summary>
    /// 将另外一个RecordBean中的相关数据加入的该bean中
    /// </summary>
    /// <param name="bean">要添加的bean</param>
    public void Add(RecordBean bean)
    {
        //继承文件或文件夹的大小和数量
        Size += bean.Size;
        SpaceUsage += bean.SpaceUsage;
        FileCount += bean.FileCount;
        // 继承是否被改变
        IsChange = IsChange || bean.IsChange;
        // 若是文件，则继承子一级文件的异常码；
        if (bean.IsFile)
            ExceptionCode = bean.ExceptionCode | ExceptionCode;
        else
            DirCount += bean.DirCount + 1;
        // 继承最新的修改和访问时间
        if (bean.ModifyTime > ModifyTime)
            ModifyTime = bean.ModifyTime;
        if (bean.VisitTime > VisitTime)
            VisitTime = bean.VisitTime;
    }
    /// <summary>
    /// 比较两个文件夹是否一样
    /// </summary>
    /// <param name="bean"></param>
    public bool Equals(RecordBean bean)
    {
        return Size == bean.Size
            && FileCount == bean.FileCount
            && DirCount == bean.DirCount
            && SpaceUsage == bean.SpaceUsage;
    }
} }
