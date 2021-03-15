using System;
using System.Globalization;
using System.Windows.Data;

namespace SpatialAnalysis.Entity
{
    class AddRecordBean
    {
        public string Title { get; set; }
        public string Remark { get; set; }
        public IncidentType Type { get; set; }
    }
    enum IncidentType
    {
        daily, install, clear
    }
}
