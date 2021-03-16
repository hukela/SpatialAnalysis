using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using System;

namespace SpatialAnalysis.Service
{
    class AddRecordServive
    {
        public static IncidentBean GetBean()
        {
            return new IncidentBean { Type = IncidentType.daily };
        }
        public static void AddIncident(IncidentBean bean)
        {
            bean.CreatTime = DateTime.Now;
            IncidentMapper.AddOne(bean);
        }
    }
}
