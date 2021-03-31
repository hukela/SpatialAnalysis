using System;
using System.Numerics;

namespace SpatialAnalysis.Entity
{
    class RecordBean
    {
        public ulong Id { get; set; }
        public ulong ParentId { get; set; }
        public uint ParentRecord { get; set; }
        public string FullName { get; set; }
        public bool Type { get; set; }
        public uint Plies { get; set; }
        public BigInteger Size { get; set; }
        public BigInteger SpaceUsage { get; set; }
        public DateTime CerateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public DateTime VisitTime { get; set; }
        public string Owner { get; set; }
        public sbyte ExceptionCode { get; set; }
        public ulong FileCount { get; set; }
        public ulong PictureCount { get; set; }
        public ulong VideoCount { get; set; }
        public ulong ProjectCount { get; set; }
        public ulong DllCount { get; set; }
        public ulong TxtCount { get; set; }
        public ulong DataCount { get; set; }
        public ulong NullCount { get; set; }
        public ulong OtherCount { get; set; }
        public double CreateVariance { get; set; }
        public DateTime CreateAverage { get; set; }
        //判断该文件或文件夹是否被改变
        public bool IsChange { get; set; }
        /// <summary>
        /// 将另外一个RecordBean中的相关数据加入的该bean中
        /// </summary>
        /// <param name="bean">要添加的bean</param>
        public void Add(RecordBean bean)
        {
            if (Size == null)
                Size = new BigInteger(0);
            Size += bean.Size;
            if (SpaceUsage == null)
                SpaceUsage = new BigInteger(0);
            SpaceUsage += bean.SpaceUsage;
            FileCount += bean.FileCount;
            PictureCount += bean.PictureCount;
            VideoCount += bean.VideoCount;
            ProjectCount += bean.ProjectCount;
            DllCount += bean.DllCount;
            TxtCount += bean.TxtCount;
            DataCount += bean.DataCount;
            NullCount += bean.NullCount;
            OtherCount += bean.OtherCount;
            IsChange = bean.IsChange && IsChange;
            //继承最新的修改和访问时间
            if (ModifyTime == null)
                ModifyTime = bean.ModifyTime;
            else
            {
                if (bean.ModifyTime > ModifyTime)
                    ModifyTime = bean.ModifyTime;
            }
            if (VisitTime == null)
                VisitTime = bean.VisitTime;
            else
            {
                if (bean.VisitTime > VisitTime)
                    VisitTime = bean.VisitTime;
            }
        }
    }
}
