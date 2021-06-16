namespace SpatialAnalysis.Entity
{
    //节点对比信息，用于绑定页面
    class ComparisonInfo
    {
        public string Title { get; set; }
        public string Action { get; set; }
        public string CreateTime { get; set; }
        public string TagName { get; set; }
        public string OldTime { get; set; }
        public string NewTime { get; set; }
        public int OldFileCount { get; set; }
        public int NewFileCount { get; set; }
        public int OldDirCount { get; set; }
        public int NewDirCount { get; set; }
        public string OldSize { get; set; }
        public string NewSize { get; set; }
        public string SizeChanged { get; set; }
        public string OldUsage { get; set; }
        public string NewUsage { get; set; }
        public string UsageChanged { get; set; }
        public string FileCountChanged
        {
            get
            {
                int result = NewFileCount - OldFileCount;
                if (result > 0)
                    return string.Concat('+', result);
                else
                    return result.ToString();
            }
        }
        public string DirCountChanged
        {
            get
            {
                int result = NewDirCount - OldDirCount;
                if (result > 0)
                    return string.Concat('+', result);
                else
                    return result.ToString();
            }
        }
        public string Location
        {
            set { location = value; }
            get
            {
                if (location == string.Empty)
                    return "计算机硬盘";
                else
                    return location;
            }
        }
        private string location = string.Empty;
    }
}
