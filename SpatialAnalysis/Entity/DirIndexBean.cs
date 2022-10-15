namespace SpatialAnalysis.Entity
{
    /// <summary>
    /// 索引表数据实体
    /// </summary>
    internal class DirIndexBean
    {
        public string Path { get; set; }
        public uint IncidentId { get; set; }
        public ulong TargetId { get; set; }
    }
}
