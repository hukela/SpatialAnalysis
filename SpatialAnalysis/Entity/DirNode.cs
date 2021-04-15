namespace SpatialAnalysis.Entity
{
    class DirNode
    {
        public uint OldIncidentId { get; set; }
        public ulong OldId { get; set; }
        public uint NewIncidentId { get; set; }
        public ulong NewId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string TagColor { get; set; }
        public string TagMame { get; set; }
        public DirNode[] Children { get; set; }
        //方便该类在字符串之间转换
        public override string ToString()
        {
            string name = Name.Replace("&", "[{and}]");
            string tagName = TagMame.Replace("&", "[{and}]");
        }
    }
}
