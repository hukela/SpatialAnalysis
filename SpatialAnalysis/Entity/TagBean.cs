using System;

namespace SpatialAnalysis.Entity
{
/// <summary>
/// 标签表数据实体
/// </summary>
internal class TagBean
{
    public uint Id { get; set; }
    public uint ParentId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
} }
