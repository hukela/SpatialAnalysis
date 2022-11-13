using System;
using System.Threading;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;

namespace SpatialAnalysis.Service.SeeRecordExtend
{
public class DelRecord
{
    public DelRecord(uint incidentId, ProgramWindow programWindow)
    {
        this.incidentId = incidentId;
        this.programWindow = programWindow;
    }

    private readonly uint incidentId;    //事件ID
    private readonly ProgramWindow programWindow;

    /// <summary>
    /// 对事件下记录进行删除
    /// </summary>
    public void StartDelete()
    {
        programWindow.WriteLine("初始化...");
        try
        {
            recordCount = RecordMapper.Count(incidentId);
            programWindow.WriteLine("事件中共有" + recordCount + "条记录");
            programWindow.WriteLine("开始删除...");
            programWindow.WriteLine("警告：删除过程中请勿关闭程序，否则会导致数据结构错误！");
            // 开启进度展示线程
            new Thread(ShowProgressThread).Start();
            // 对记录树进行遍历并整理带删除数据
            if (incidentId == IncidentMapper.SelectLastBean().Id)
                TraverseTree(clearLast);
            else
                TraverseTree(clearOther);
            isRunning = false;
            printProgress();
            programWindow.WriteLine("开始删除记录表");
            RecordMapper.DeleteTable(incidentId); // 删除整张表
            IncidentMapper.UpdateStateById(incidentId, IncidentStateEnum.deleted);
            programWindow.WriteLine("删除完成");
            programWindow.RunOver();
        }
        catch (Exception e)
        {
            Log.Error("删除记录失败");
            Log.Add(e);
            programWindow.WriteLine("\n删除时发生错误：");
            programWindow.WriteLine(e.Message);
            programWindow.RunOver();
        }
    }

    // 用于告知用户当前删除进度
    private ulong recordCount;
    private ulong clearCount;
    private volatile bool isRunning;
    // 单独一个线程用于显示进度
    private void ShowProgressThread()
    {
        programWindow.Freeze();
        while (isRunning)
        {
            printProgress();
            Thread.Sleep(300);
        }
    }
    // 打印进度
    private void printProgress()
    {
        programWindow.Clean(false);
        programWindow.WriteLine("整理代删除数据：" + clearCount);
        programWindow.WriteLine("整理进度：" + clearCount * 100 / recordCount + "%");
    }

    // 深度遍历记录树方法的委托
    private delegate void TraverseDelegate(uint fromIncidentId, RecordBean bean, bool beMapped);

    // 深度遍历记录树 (先序遍历)
    private void TraverseTree(TraverseDelegate traverseDelegate, RecordBean bean = null, uint fromIncidentId = 0)
    {
        RecordBean[] children = bean == null ? RecordMapper.SelectRootRecords(incidentId)
                                             : RecordMapper.SelectChildren(bean.Id, incidentId);
        // 查找映射到当前记录的来源记录 若找到 则将该节点下的所有字节点都归类到该映射来源的事件中
        bool beMapped = false;
        if (fromIncidentId == 0 && bean != null && bean.FromIncidentId != 0)
        {
            beMapped = true;
            fromIncidentId = bean.FromIncidentId;
        }
        traverseDelegate(fromIncidentId, bean, beMapped);
        if (children == null || children.Length == 0)
            return;
        foreach (RecordBean child in children)
        {
            child.ParentId = bean?.Id ?? 0;
            TraverseTree(traverseDelegate, child, fromIncidentId);
        }
    }

    // 清理最新一次的记录
    private void clearLast(uint fromIncidentId, RecordBean bean, bool beMapped)
    {
        if (bean.TargetIncidentId != 0)
            RecordMapper.UpdateFromIncidentId(bean.TargetIncidentId, bean.Path, 0);
        clearCount++;
    }

    // 清理其他记录
    private void clearOther(uint fromIncidentId, RecordBean bean, bool beMapped)
    {
        if (fromIncidentId == 0)
        {
            clearCount ++;
            return;
        }
        if (beMapped)
        {
            // 去除被映射记录的映射
            RecordBean fromBean = RecordMapper.SelectByPath(fromIncidentId, bean.Path);
            if (fromBean == null)
                throw new Exception("数据结构错误，映射来源记录不存在，事件id：" + fromIncidentId + "，记录路径：" + bean.Path);
            bean.Id = fromBean.Id;
            bean.ParentId = fromBean.ParentId;
            RecordMapper.UpdateTargetIncidentId(fromIncidentId, bean.Path, 0);
        }
        else
            // 搬运记录
            RecordMapper.InsertOne(fromIncidentId, bean, false);
        clearCount ++;
    }
} }