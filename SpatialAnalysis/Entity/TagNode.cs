namespace SpatialAnalysis.Entity
{
    /// <summary>
    /// 标签页面 标签数据
    /// </summary>
    internal class TagNode
    {
        public TagBean Tag { get; set; }
        public TagNode[] Children { get; set; }
    }
}
