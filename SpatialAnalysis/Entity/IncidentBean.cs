using System;

namespace SpatialAnalysis.Entity
{
    //事件表数据实体
    internal class IncidentBean
    {
        public uint Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public string Explain { get; set; }
        public sbyte State { get; set; }
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
}
