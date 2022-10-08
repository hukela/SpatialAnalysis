using System;
using System.Numerics;
using SpatialAnalysis.Utils;

namespace SpatialAnalysis.Entity
{
    internal class IncidentInfo
    {
        public IncidentInfo() { }

        public IncidentInfo(IncidentBean bean)
        {
            Id = bean.Id;
            Title = bean.Title;
            switch (bean.StateEnum)
            {
                case IncidentStateEnum.success: State = "成功"; break;
                case IncidentStateEnum.failure: State = "失败"; break;
                case IncidentStateEnum.deleted: State = "删除"; break;
                default: throw new ArgumentOutOfRangeException();
            }
            CreateTime = bean.CreateTime.ToString("yy-MM-dd HH:ss");
        }

        public uint Id { get; }
        public string Title { get; set; }
        public string State { get; set; }
        public uint FileCount { get; set; }
        public uint DirCount { get; set; }
        public BigInteger Size { get; set; }
        public BigInteger SpaceUsage { get; set; }
        public string SizeFormat => ConversionUtil.StorageFormat(Size, false);
        public string CreateTime { get; set; }
    }
}