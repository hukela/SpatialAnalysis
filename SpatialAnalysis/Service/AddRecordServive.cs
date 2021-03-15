using SpatialAnalysis.Entity;

namespace SpatialAnalysis.Service
{
    class AddRecordServive
    {
        public static AddRecordBean GetBean()
        {
            AddRecordBean bean = new AddRecordBean();
            bean.Title = "aaa";
            bean.Remark = "ccc";
            bean.Type = IncidentType.clear;
            return bean;
        }
    }
}
