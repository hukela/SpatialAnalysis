using System;

namespace SpatialAnalysis.Entity
{
    class IncidentBean
    {
        public long Id { get; set; }
        public DateTime CreatTime { get; set; }
        public string Title { get; set; }
        public string Explain { get; set; }
        public IncidentType Type { get; set; }
    }
    enum IncidentType
    {
        daily, install, clear
    }
}
