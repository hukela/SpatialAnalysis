using System;
using System.Numerics;

namespace SpatialAnalysis.Entity
{
    class RecordBean
    {
        ulong Id { get; set; }
        ulong ParentId { get; set; }
        uint ParentRecord { get; set; }
        string Name { get; set; }
        string Postfix { get; set; }
        string Path { get; set; }
        bool Type { get; set; }
        uint NumberOfPlies { get; set; }
        BigInteger Size { get; set; }
        BigInteger SpaceUsage { get; set; }
        DateTime CerateTime { get; set; }
        DateTime ModifyTime { get; set; }
        DateTime VisitTime { get; set; }
        string User { get; set; }
        ulong FileCount { get; set; }
        ulong PictureCount { get; set; }
        ulong VideoCount { get; set; }
        ulong ProjectCount { get; set; }
        ulong ExeCount { get; set; }
        ulong DllCount { get; set; }
        ulong TxtCount { get; set; }
        ulong ConfigCount { get; set; }
        ulong NullCount { get; set; }
        ulong OtherCount { get; set; }
        double CreateVariance { get; set; }
        DateTime CreateAverage { get; set; }
    }
}
