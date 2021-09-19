using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using System;
using System.IO;
using System.Threading;

namespace SpatialAnalysis.Service.AddRecordExtend
{
    internal class AddRecordAsyn
    {
        //是否是第一次记录
        private bool isFirst;
        //事件id
        private uint incidentId;
        //当前类的线程
        public Thread thread;
        public void AddOne(object obj)
        {
            DateTime startTime = DateTime.Now;
            Thread.Sleep(1000);
            object[] objs = (object[])obj;
            ProgramWindow programWindow = (ProgramWindow)objs[0];
            IncidentBean bean = (IncidentBean)objs[1];
            programWindow.WriteLine("初始化...");
            try
            {
                //处理事件
                bean.CreateTime = DateTime.Now;
                bean.State = 1;
                incidentId = IncidentMapper.AddOne(bean);
                isFirst = IncidentMapper.IsFirstRecord();
                //第一次记录必须保证索引表为空
                if (isFirst)
                    DirIndexMapper.CleanAll();
                //新建记录表
                Extend.BuildTable(incidentId, isFirst);
                programWindow.WriteLine("开始记录硬盘使用空间...");
                programWindow.WriteLine("(建议此时不要修改硬盘上的文件，以免影响最终的分析结果)");
                programWindow.Freeze();
                isRunning = true;
                Thread showProgress = new Thread(ShowProgress) { Name = "showProgress" };
                showProgress.Start(programWindow);
                DriveInfo[] drives = DriveInfo.GetDrives();
                //遍历分区
                foreach (DriveInfo drive in drives)
                {
                    DirectoryInfo rootDir = new DirectoryInfo(drive.Name);
                    SeeDirectory(rootDir, 0);
                }
                isRunning = false;
                //收尾工作
                long count = RecordMapper.Count(incidentId);
                IncidentMapper.SetStateById(incidentId, 0);
                if (isFirst)
                    //删除以前记录失败的作废表格
                    Extend.DeleteErrorTable(incidentId);
                TimeSpan consumption = DateTime.Now - startTime;
                Log.Info(string.Concat("数据记录完成, 记录：", count, "(", beanCount, ")，耗时：", consumption));
                programWindow.WriteAll("数据记录完成，耗时：" +
                    string.Concat(consumption.Days * 24 + consumption.Hours, "小时", consumption.Minutes, "分"));
                programWindow.RunOver();
            }
            catch (Exception e)
            {
                Log.Add(e);
                programWindow.WriteLine("错误：");
                programWindow.WriteLine(e.Message);
                programWindow.RunOver();
                //throw e;
            }
        }
        //用于告知当前进度的
        private ulong beanCount = 0;
        private string plies2Path = "";
        private bool isRunning;
        private void ShowProgress(object obj)
        {
            ProgramWindow window = (ProgramWindow)obj;
            bool a = true;
            while (isRunning)
            {
                window.WriteAll(plies2Path + "\n已记录：" + beanCount);
                if (a)
                {
                    a = false;
                    window.WriteLine(" *");
                }
                else
                {
                    a = true;
                    window.Write("\n");
                }
                window.Write("当前线程状态：" + thread.ThreadState);
                //窗口的刷新时间
                Thread.Sleep(300);
            }
        }
        //使用回调的方式，遍历整个硬盘
        private RecordBean SeeDirectory(DirectoryInfo baseDir, uint plies)
        {
            //告知当前进度
            if (plies == 2)
                plies2Path = baseDir.FullName;
            RecordBean bean = BeanFactory.GetDirBean(baseDir, plies);
            //C盘有传送门，两个路径可以同时访问一个文件，所以这里放弃其中一个路径
            if (bean.Path == @"C:\Users\All Users") return bean;
            //获取子文件和子文件夹列表
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
            //用于记录子节点的bean
            RecordBean[] childDirBeans = new RecordBean[dirs.Length]; ;
            //遍历并获取子文件夹
            for (int i = 0; i < dirs.Length; i++)
            {
                RecordBean dirBean = SeeDirectory(dirs[i], plies + 1);
                bean.Add(dirBean);
                childDirBeans[i] = dirBean;
            }
            //遍历并获取子文件
            foreach (FileInfo file in files)
            {
                RecordBean fileBean = BeanFactory.GetFileBean(file, plies);
                bean.Add(fileBean);
            }
            // 存储记录结果
            if (isFirst)
                SaveBeanForFirstRecord(bean, childDirBeans);
            else
                SaveBeanForOtherRecord(bean, childDirBeans);
            return bean;
        }
        // 第一次记录
        private void SaveBeanForFirstRecord(RecordBean bean, RecordBean[] childDirBeans)
        {
            //记录并获取当前id
            bean.Id = RecordMapper.AddOne(bean, incidentId, true);
            //记录索引
            DirIndexMapper.AddOne(new DirIndexBean()
            {
                Path = bean.Path,
                IncidentId = incidentId,
                TargectId = bean.Id,
            });
            beanCount++;
            //设置子一级的父id
            foreach (RecordBean dirBean in childDirBeans)
                RecordMapper.SetParentId(dirBean.Id, bean.Id, incidentId);
        }
        // 后续记录需要过滤掉未变化的重复项
        private void SaveBeanForOtherRecord(RecordBean bean, RecordBean[] childDirBeans)
        {
            RecordBean targetBean = Extend.GetLastBean(bean.Path);
            if (targetBean == null)
                bean.IsChange = true;
            else
                bean.IsChange = bean.IsChange || !bean.Equals(targetBean);
            //如果该bean没有变化，则不再记录
            if (bean.IsChange)
            {
                bean.Id = RecordMapper.AddOne(bean, incidentId, false);
                beanCount++;
                //刷新索引
                DirIndexMapper.RefreshIndex(new DirIndexBean()
                {
                    Path = bean.Path,
                    IncidentId = incidentId,
                    TargectId = bean.Id,
                });
                foreach (RecordBean dirBean in childDirBeans)
                {
                    if (dirBean.IsChange)
                        RecordMapper.SetParentId(dirBean.Id, bean.Id, incidentId);
                    else
                    {
                        //将未改变的bena也记录下来
                        dirBean.ParentId = bean.Id;
                        RecordMapper.AddOne(dirBean, incidentId, false);
                        beanCount++;
                    }
                }
            }
            else
            {
                //为未改变节点添加索引
                bean.IncidentId = targetBean.IncidentId;
                bean.TargetId = targetBean.Id;
            }
        }
    }
}
