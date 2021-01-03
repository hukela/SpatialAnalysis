namespace SpatialAnalysis.Entity
{
    class Incident
    {
        //事件标题
        public string title;
        //事件备注
        public string comment;
        //事件类型
        public IncideatType type;
    }
    enum IncideatType
    {
        daily, install, clear
    }
}
