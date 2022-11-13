using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows;
using LiveChartsCore.Kernel;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service.SeeRecordExtend;

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
            if (incidentBean.StateEnum == IncidentStateEnum.deleted)
                continue;
            RecordBean[] rootRecordBeans = RecordMapper.SelectRootRecords(incidentBean.Id);
            ulong recordCount = RecordMapper.Count(incidentBean.Id);
            uint fileCount = 0;
            uint dirCount = 0;
            BigInteger sizeCount = BigInteger.Zero;
            BigInteger spaceCount = BigInteger.Zero;
            Array.ForEach(rootRecordBeans, bean =>
            {
                fileCount += bean.FileCount;
                dirCount += bean.DirCount;
                sizeCount += bean.Size;
                spaceCount += bean.SpaceUsage;
            });
            incidentInfos[i].RecordCount = recordCount;
            incidentInfos[i].FileCount = fileCount;
            incidentInfos[i].DirCount = dirCount;
            incidentInfos[i].Size = sizeCount;
            incidentInfos[i].SpaceUsage = spaceCount;
        }
        return incidentInfos;
    }

    /// <summary>
    /// 对事件下的记录进行删除
    /// </summary>
    /// <param name="incidentId">记录id</param>
    public static void deleteRecord(uint incidentId)
    {
        IncidentBean[] incidents = IncidentMapper.SelectSuccessIncidents();
        IncidentBean bean = incidents.FirstOrDefault(i => i.Id == incidentId);
        bool needReorganize = bean != null && incidents.Length > 1; // 删除该数据时是否需要对数据结构进行重组镇整理
        string boxTest = needReorganize ? "是否确定删除该记录下所有数据？(该操作不可逆，且需要几分钟执行)" : "是否确定删除该记录下所有数据？(该操作不可逆)";
        if (MessageBox.Show(boxTest, "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
            return;
        if (needReorganize)
        {
            ProgramWindow programWindow = new ProgramWindow();
            DelRecord delRecord = new DelRecord(incidentId, programWindow);
            new Thread(delRecord.StartDelete) {Name = "delRecord"}.Start();
            programWindow.ShowDialog();
        }
        else
        {
            RecordMapper.DeleteTable(incidentId);
            IncidentMapper.UpdateStateById(incidentId, IncidentStateEnum.deleted);
            MessageBox.Show("删除完毕", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
} }