using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.IO.Xml;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
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
            TextWindow window = new TextWindow();
            object[] objs = { window, bean };
            Thread thread = new Thread(addRecord.AddIncidentAsyn) { Name = "addRecord" };
            thread.Start(objs);
            window.ShowDialog();
        }
        private void AddIncidentAsyn(object obj)
        {
            Thread.Sleep(1000);
            object[] objs = (object[])obj;
            TextWindow textWindow = (TextWindow)objs[0];
            IncidentBean bean = (IncidentBean)objs[1];
            textWindow.WriteLine("初始化...");
            try
            {
                bean.CreatTime = DateTime.Now;
                int id = IncidentMapper.AddOne(bean);
                //新建记录表
                string templatePath = Base.locolPath + @"\Data\BuildRecord.template.sql";
                string path = Base.locolPath + @"\Data\BuildRecord.sql";
                string sql = TextFile.ReadAll(templatePath, Encoding.UTF8);
                sql = sql.Replace("[id]", id.ToString());
                TextFile.WriteAll(path, Encoding.UTF8, sql);
                MySqlAction.ExecuteSqlFile(path);
                //向记录表中添加数据
                textWindow.WriteLine("开始记录硬盘使用清空...");
                textWindow.WriteLine("(建议此时不要修改硬盘上的文件，以免影响最终的分析结果)");
            }
            catch (Exception e)
            {
                Log.Add(e);
                textWindow.WriteLine("错误：");
                textWindow.WriteLine(e.Message);
            }
        }
        //使用回调的方式，遍历整个硬盘
        private RecordBean SeeDirectory(DirectoryInfo baseDir, uint plies)
        {
            RecordBean bean = new RecordBean();
            //计算平均数和方差用到
            ulong averageTime = 0;
            //遍历整个文件夹
            DirectoryInfo[] dirs = baseDir.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                RecordBean dirBean = SeeDirectory(dir, plies + 1);
                bean.Add(dirBean);
                averageTime += (ulong)DateTimeUtil.GetTimeStamp(dirBean.CerateTime);
            }
            return bean;
        }
    }
}
