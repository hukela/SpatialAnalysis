using System;

namespace SpatialAnalysis.Entity
{
    class IncidentBean
    {
        public uint Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public string Explain { get; set; }
        public sbyte IncidentState { get; set; }
        public string TimeFormat
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
