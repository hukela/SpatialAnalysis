using System;
using System.Numerics;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Utils;

namespace SpatialAnalysis.Service
{
internal static class SeeRecordService
{
    /// <summary>
    /// 查询记录事件列表
    /// </summary>
    /// <param name="all">是否包含失败和被删除的事件</param>
    public static IncidentInfo[] getIncidentInfos(bool all)
    {
        // 查询所有事件
        IncidentBean[] incidentBeans = all ? IncidentMapper.SelectAllIncidents() : IncidentMapper.SelectSuccessIncidents();
        IncidentInfo[] incidentInfos = new IncidentInfo[incidentBeans.Length];
        for (int i = 0; i < incidentBeans.Length; i++)
        {
            IncidentBean incidentBean = incidentBeans[i];
            incidentInfos[i] = new IncidentInfo(incidentBean);
            RecordBean[] rootRecordBeans = RecordMapper.SelectRootRecords(incidentBean.Id);
            BigInteger sizeCount = BigInteger.Zero;
            BigInteger spaceCount = BigInteger.Zero;
            Array.ForEach(rootRecordBeans, bean => { sizeCount += bean.Size; spaceCount += bean.SpaceUsage; });
            incidentInfos[i].Size = sizeCount;
            incidentInfos[i].SpaceUsage = spaceCount;
        }
        return incidentInfos;
    }
} }