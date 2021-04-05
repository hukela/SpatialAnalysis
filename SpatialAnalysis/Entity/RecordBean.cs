using System;
using System.Numerics;

namespace SpatialAnalysis.Entity
{
    class RecordBean
    {
        public ulong Id { get; set; }
        public ulong ParentId { get; set; }
        public uint IncidentId { get; set; }
        public ulong TargetId { get; set; }
        public string Path { get; set; }
        public uint Plies { get; set; }
        public BigInteger Size { get; set; }
        public BigInteger SpaceUsage { get; set; }
        public DateTime CerateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public DateTime VisitTime { get; set; }
        public string Owner { get; set; }
        public sbyte ExceptionCode { get; set; }
        public uint FileCount { get; set; }
        public uint DirCount { get; set; }
        //当前bean是否被改变
        public bool IsChange { get; set; }
        /// <summary>
        /// 将另外一个RecordBean中的相关数据加入的该bean中
        /// </summary>
        /// <param name="bean">要添加的bean</param>
        public void Add(RecordBean bean)
        {
            //继承文件或文件夹的大小和数量
            if (Size == null)
                Size = new BigInteger(0);
            Size += bean.Size;
            if (SpaceUsage == null)
                SpaceUsage = new BigInteger(0);
            SpaceUsage += bean.SpaceUsage;
            FileCount += bean.FileCount;
            DirCount += bean.DirCount;
            //继承是否被改变
            IsChange = IsChange || bean.IsChange;
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
