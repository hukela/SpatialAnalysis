﻿namespace SpatialAnalysis.Entity
{
/// <summary>
/// 节点类别
/// </summary>
public enum DirNodeType
{
    Unchanged,
    Changed,
    Added,
    Deleted,
}
/// <summary>
/// 对比页面的文件夹节点
/// </summary>
internal class DirNode
{
    public uint OldIncidentId { get; set; }
    public ulong OldId { get; set; }
    public uint NewIncidentId { get; set; }
    public ulong NewId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public DirNodeType Type {get;set;}
    public TagBean Tag { get; set; }
    public bool IsRootTag { get; set; }
    public DirNode[] Children { get; set; }
    public string RootTagName => IsRootTag ? string.Concat('[', Tag.Name, ']') : string.Empty;

    public string Color
    {
        get
        {
            switch (Type)
            {
                case DirNodeType.Unchanged:
                    return null;
                case DirNodeType.Added:
                    return "#00FF7F";
                case DirNodeType.Changed:
                    return "#87CEFA";
                case DirNodeType.Deleted:
                    return "#FFC0CB";
                default:
                    return null;
            }
        }
    }
} }
