using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.Mapper;
using System;
using System.Text;

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
        }
    }
}
