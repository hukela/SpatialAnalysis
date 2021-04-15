namespace SpatialAnalysis.Entity
{
    class DirNode
    {
        public uint OldIncidentId { get; set; }
        public ulong OldId { get; set; }
        public uint NewIncidentId { get; set; }
        public ulong NewId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
        public string TagName { get; set; }
        public string TagColor { get; set; }
        public DirNode[] Children { get; set; }
    }
}
