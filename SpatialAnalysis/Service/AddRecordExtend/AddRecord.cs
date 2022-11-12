using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using System;
using System.IO;
using System.Text;
using System.Threading;
using SpatialAnalysis.IO.Log;

namespace SpatialAnalysis.Service.AddRecordExtend
{
internal class AddRecord
{
    public AddRecord(IncidentBean incidentBean, ProgramWindow programWindow)
    {
        this.incidentBean = incidentBean;
        this.programWindow = programWindow;
    }

    // 初始化时传递的参数
    private readonly IncidentBean incidentBean;
    private readonly ProgramWindow programWindow;
    // 是否是第一次记录
    private bool isFirst;
    // 事件id
    private uint incidentId;
    // 当前类的线程
    public Thread thread;

    /// <summary>
    /// 添加记录方法
    /// </summary>
    /// <param name="programWindow">用于输出进度的ProgramWindow</param>
    public void AddOne()
    {
        DateTime startTime = DateTime.Now;
        Thread.Sleep(1000);
        programWindow.WriteLine("初始化...");
        try
        {
            // 查找上一次记录的事件
            IncidentBean lastIncident = IncidentMapper.SelectLastSuccessIncident();
            uint targetIncidentId = lastIncident?.Id ?? 0;
            isFirst = targetIncidentId == 0; // 0表示没有上一次事件 无需对照
            // 写入新事件
            incidentBean.CreateTime = DateTime.Now;
            incidentBean.StateEnum = IncidentStateEnum.failure;
            incidentBean.RecordType = isFirst ? (sbyte)1 : (sbyte)0;
            incidentId = IncidentMapper.InsertOne(incidentBean);
            // 新建记录表
            BuildTable();
            programWindow.WriteLine("开始记录硬盘使用空间...");
            programWindow.WriteLine("(建议此时不要修改硬盘上的文件)");
            isRunning = true;
            Thread showProgress = new Thread(ShowProgress) { Name = "showProgress" };
            showProgress.Start();
            DriveInfo[] drives = DriveInfo.GetDrives();
            // 遍历分区
            foreach (DriveInfo drive in drives)
            {
                DirectoryInfo rootDir = new DirectoryInfo(drive.Name);
                SeeDirectory(rootDir, 0, targetIncidentId);
            }
            isRunning = false;
            programWindow.WriteAll("记录完成，建立索引...\n");
            BuildIndex();
            // 收尾工作
            ulong count = RecordMapper.Count(incidentId);
            IncidentMapper.UpdateStateById(incidentId, IncidentStateEnum.success);
            TimeSpan consumption = DateTime.Now - startTime;
            Log.Info($"数据记录完成, 记录：{count}({beanCount}), 耗时：{consumption}");
            programWindow.WriteLine(string.Format("数据记录完成，耗时：{0}小时{1}分。",
                consumption.Days * 24 + consumption.Hours, consumption.Minutes));
            programWindow.RunOver();
        }
        catch (Exception e)
        {
            Log.Add(e);
            isRunning = false;
            programWindow.WriteLine("\n错误：");
            programWindow.WriteLine(e.Message);
            programWindow.RunOver();
        }
    }

    // 用于告知当前进度的
    private ulong beanCount;
    private string plies2Path = "";
    private volatile bool isRunning;
    // 单独一个线程用于显示进度
    private void ShowProgress()
    {
        programWindow.Freeze();
        bool a = true;
        while (isRunning)
        {
            programWindow.WriteAll(plies2Path + "\n已记录：" + beanCount);
            if (a)
            {
                a = false;
                programWindow.WriteLine(" *");
            }
            else
            {
                a = true;
                programWindow.Write("\n");
            }
            programWindow.Write("当前线程状态：" + thread.ThreadState);
            // 窗口的刷新时间
            Thread.Sleep(300);
        }
    }
    // 使用回调的方式，遍历整个硬盘
    private RecordBean SeeDirectory(DirectoryInfo baseDir, uint plies, uint targetIncidentId)
    {
        Log.Info("进入路径[" + baseDir.FullName + "]，对照事件：" + targetIncidentId);
        // 告知当前进度
        if (plies == 2)
            plies2Path = baseDir.FullName;
        RecordBean bean = BeanFactory.BuildDirBean(baseDir, plies);
        // C盘有传送门，两个路径可以同时访问一个文件，所以这里放弃其中一个路径
        if (bean.Path == @"C:\Users\All Users") return bean;
        // 获取子文件和子文件夹列表
        DirectoryInfo[] dirs;
        FileInfo[] files;
        try
        {
            dirs = baseDir.GetDirectories();
            files = baseDir.GetFiles();
        }
        catch (UnauthorizedAccessException e)
        {
            bean.ExceptionCode = (int)RecordExCode.UnauthorizedAccess | bean.ExceptionCode;
            Log.Warn(string.Format("没有权限。{0}, [error code: {1}]",
                                   e.Message, RecordExCode.UnauthorizedAccess));
            return bean;
        }
        catch (IOException e)
        {
            bean.ExceptionCode = (int)RecordExCode.IOExceptionForGetFile | bean.ExceptionCode;
            Log.Warn(string.Format("{0}: {1}, [error code: {2}]",
                baseDir.FullName, e.Message, RecordExCode.IOExceptionForGetFile));
            return bean;
        }
        // 再遍历子节点前查询对照组以确定子节点的对照事件id
        uint childrenTargetIncidentId = targetIncidentId;
        RecordBean targetBean;
        do
        {
            targetBean = isFirst || targetIncidentId == 0
                ? null
                : RecordMapper.SelectByPath(childrenTargetIncidentId, bean.Path);
            if (targetBean == null)
            {
                childrenTargetIncidentId = 0;
                break;
            }
            if (targetBean.TargetIncidentId == 0)
                break;
            childrenTargetIncidentId = targetBean.TargetIncidentId;
        } while(true);
        if (childrenTargetIncidentId != targetIncidentId)
            Log.Info("添加记录 修改对照id 原：" + targetIncidentId +
                     " 新：" + childrenTargetIncidentId + " 路径： " + bean.Path);
        // 用于记录子节点的bean
        RecordBean[] childDirBeans = new RecordBean[dirs.Length];
        // 遍历并获取子记录节点
        for (int i = 0; i < dirs.Length; i++)
        {
            RecordBean dirBean = SeeDirectory(dirs[i], plies + 1, childrenTargetIncidentId);
            bean.Add(dirBean);
            childDirBeans[i] = dirBean;
        }
        // 遍历并获取子文件
        foreach (FileInfo file in files)
        {
            RecordBean fileBean = BeanFactory.BuildFileBean(file, plies);
            bean.Add(fileBean);
        }
        // 存储记录结果
        if (isFirst)
            // 对于第一次记录 直接存储 无需搭建映射
            SaveBeanForFirstRecord(bean, childDirBeans);
        else
        {
            // 对于后续记录 只存储变化的记录
            bean.IsChange = bean.IsChange || targetBean == null || !bean.Equals(targetBean);
            Log.Info("保存路径[" + bean.Path + "]，是否改变：" + bean.IsChange);
            if (bean.IsChange)
                SaveBeanForOtherRecord(bean, childDirBeans);
            else
                bean.TargetIncidentId = targetIncidentId;
        }

        return bean;
    }

    // 第一次记录
    private void SaveBeanForFirstRecord(RecordBean bean, RecordBean[] childDirBeans)
    {
        // 记录并获取当前id
        bean.Id = RecordMapper.InsertOne(incidentId, bean, true);
        beanCount++;
        // 设置子一级的父id
        foreach (RecordBean dirBean in childDirBeans)
            RecordMapper.UpdateParentId(incidentId, dirBean.Id, bean.Id);
    }
    // 后续记录
    private void SaveBeanForOtherRecord(RecordBean bean, RecordBean[] childDirBeans)
    {
        bean.Id = RecordMapper.InsertOne(incidentId, bean, false);
        beanCount++;
        foreach (RecordBean child in childDirBeans)
        {
            if (child.IsChange)
                RecordMapper.UpdateParentId(incidentId, child.Id, bean.Id);
            else
            {
                child.ParentId = bean.Id;
                // 将下一层未改变的bean也记录下来
                Log.Info("记录未变化的路径[" + child.Path + "]，对照事件id：" + child.TargetIncidentId);
                RecordMapper.InsertOne(incidentId, child, false);
                // 更新映射目标的映射来源事件id
                if (child.TargetIncidentId != 0)
                    RecordMapper.UpdateFromIncidentId(child.TargetIncidentId, child.Path, incidentId);
                beanCount++;
            }
        }
    }

    // 建立表格
    private void BuildTable()
    {
        string path = IoBase.localPath + @"\Data\record.sql";
        string sql = TextFile.ReadAll(path, Encoding.UTF8);
        sql = sql.Replace("{incidentId}", incidentId.ToString());
        if (isFirst)
        {
            while (sql.IndexOf("{isNotFirst}") != -1)
            {
                int startIndex = sql.IndexOf("{isNotFirst}");
                int endIndex = sql.IndexOf("{/isNotFirst}");
                sql = sql.Remove(startIndex, endIndex - startIndex + 13);
            }
        }
        else
        {
            sql = sql.Replace("{isNotFirst}", "");
            sql = sql.Replace("{/isNotFirst}", "");
        }
        SQLiteClient.ExecuteSql(sql);
    }
    // 建立索引
    private void BuildIndex()
    {
        string path = IoBase.localPath + @"\Data\index.sql";
        string sql = TextFile.ReadAll(path, Encoding.UTF8);
        sql = sql.Replace("{incidentId}", incidentId.ToString());
        SQLiteClient.ExecuteSql(sql);
    }
} }
