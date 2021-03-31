using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service.AddRecordPatter;
using SpatialAnalysis.Utils;
using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;

namespace SpatialAnalysis.Service
{
    class AddRecordServive
    {
        public static IncidentBean GetBean()
        {
            return new IncidentBean { Type = IncidentType.daily };
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="bean">事件bean</param>
        public static void AddIncident(IncidentBean bean)
        {
            AddRecordServive addRecord = new AddRecordServive();
            Log.Info("开始添加记录");
            ProgramWindow window = new ProgramWindow();
            object[] objs = { window, bean };
            Thread thread = new Thread(addRecord.AddIncidentAsyn) { Name = "addRecord" };
            thread.Start(objs);
            window.ShowDialog();
        }
        //事件id
        private uint incidentId;
        private void AddIncidentAsyn(object obj)
        {
            long startTime = DateTimeUtil.GetTimeStamp();
            Thread.Sleep(1000);
            object[] objs = (object[])obj;
            ProgramWindow programWindow = (ProgramWindow)objs[0];
            IncidentBean bean = (IncidentBean)objs[1];
            programWindow.WriteLine("初始化...");
            //try
            //{
                bean.CreatTime = DateTime.Now;
                incidentId = IncidentMapper.AddOne(bean);
                //新建记录表
                string templatePath = Base.locolPath + @"\Data\BuildRecord.template.sql";
                string path = Base.locolPath + @"\Data\BuildRecord.sql";
                string sql = TextFile.ReadAll(templatePath, Encoding.UTF8);
                sql = sql.Replace("[id]", incidentId.ToString());
                TextFile.WriteAll(path, Encoding.UTF8, sql);
                MySqlAction.ExecuteSqlFile(path);
                //向记录表中添加数据
                programWindow.WriteLine("开始记录硬盘使用空间...");
                programWindow.WriteLine("(建议此时不要修改硬盘上的文件，以免影响最终的分析结果)");
                programWindow.Freeze();
                isRunning = true;
                Thread showProgress = new Thread(ShowProgress) { Name = "showProgress" };
                showProgress.Start(programWindow);
                DriveInfo[] drives = DriveInfo.GetDrives();
                //遍历分区
                /*
                foreach (DriveInfo drive in drives)
                {
                    DirectoryInfo rootDir = new DirectoryInfo(drive.Name);
                    SeeDirectory(rootDir, 0, textWindow);
                }
                */
                SeeDirectory(new DirectoryInfo(@"C:\"), 0);
                isRunning = false;
                ulong count = RecordMapper.Count(incidentId);
                long consumptionTime = DateTimeUtil.GetTimeStamp() - startTime;
                Log.Info("硬盘使用清空记录完成, 记录：" + count + "，耗时：" + consumptionTime);
                programWindow.WriteAll("数据记录完成");
                programWindow.RunOver();
            //}
            //catch (Exception e)
            //{
            //    Log.Add(e);
            //    programWindow.WriteLine("错误：");
            //    programWindow.WriteLine(e.Message);
            //}
        }
        //用于告知当前进度的
        BigInteger beanCount = BigInteger.Parse("0");
        string plies2Path = "";
        bool isRunning;
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
                    window.WriteLine("         *");
                }
                else
                    a = true;
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
            //计算平均数和方差用到
            double countTime = 0;
            //获取文件和文件夹列表
            DirectoryInfo[] dirs;
            FileInfo[] files;
            try
            {
                dirs = baseDir.GetDirectories();
                files = baseDir.GetFiles();
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Warn("没有权限。" + e.Message);
                bean.ExceptionCode = 1;
                return bean;
            }
            //用于记录子节点的id
            uint idIndex = 0;
            ulong[] ids = new ulong[dirs.Length + files.Length];
            //遍历整个文件夹
            foreach (DirectoryInfo dir in dirs)
            {
                RecordBean dirBean = SeeDirectory(dir, plies + 1);
                bean.Add(dirBean);
                countTime += DateTimeUtil.GetTimeStamp(dirBean.CerateTime);
                ids[idIndex] = dirBean.Id; idIndex++;
            }
            //遍历整个文件
            foreach (FileInfo file in files)
            {
                RecordBean fileBean = BeanFactory.GetFileBean(file, plies);
                bean.Add(fileBean);
                countTime += DateTimeUtil.GetTimeStamp(file.CreationTime);
                //将文件bean存入数据库
                ids[idIndex] = RecordMapper.AddOne(fileBean, incidentId); idIndex++;
            }
            //计算平均数和方差
            int number = dirs.Length + files.Length;
            if (number != 0)
            {
                double averageTime = countTime / number;
                bean.CreateAverage = DateTimeUtil.TimeStampToDateTime(Convert.ToInt64(averageTime));
                double variance = 0;
                foreach (DirectoryInfo dir in dirs)
                {
                    long dirCreateTime = DateTimeUtil.GetTimeStamp(dir.CreationTime);
                    variance += Math.Pow(averageTime - dirCreateTime, 2);
                }
                foreach (FileInfo file in files)
                {
                    long fileCreateTime = DateTimeUtil.GetTimeStamp(file.CreationTime);
                    variance += Math.Pow(averageTime - fileCreateTime, 2);
                }
                variance = variance / number;
                bean.CreateVariance = variance;
            }
            //记录并获取当前id
            bean.Id = RecordMapper.AddOne(bean, incidentId);
            beanCount++;
            //设置子一级的父id
            foreach (ulong id in ids)
            {
                RecordMapper.SetParentId(id, bean.Id, incidentId);
                beanCount++;
            }
            return bean;
        }
    }
}
