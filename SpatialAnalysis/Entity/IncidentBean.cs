using System;

namespace SpatialAnalysis.Entity
{
/// <summary>
/// 事件表数据实体
/// </summary>
internal class IncidentBean
{
    public uint Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public sbyte State { get; set; }

    public IncidentStateEnum StateEnum
    {
        get => (IncidentStateEnum) Enum.ToObject(typeof(IncidentStateEnum), State);
        set => State = (sbyte)value;
    }
    public string CreateTimeFormat
    {
        get
        {
            if (CreateTime == DateTime.MinValue)
                return null;
            else
                return CreateTime.ToString("yy-MM-dd HH:ss");
        }
    }
}
// 事件类型枚举
internal enum IncidentStateEnum
{
    success, failure, deleted
} }
