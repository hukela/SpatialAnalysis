using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.Mapper;
using System.IO;
using System.Text;

namespace SpatialAnalysis.Service.AddRecordExtend
{
    class Extend
    {
        //建立表格
        public static void BuildTable(uint incidentId, bool isFiest)
        {
            string templatePath = Base.locolPath + @"\Data\BuildRecord.template.sql";
            string path = Base.locolPath + @"\Data\BuildRecord.sql";
            string sql = TextFile.ReadAll(templatePath, Encoding.UTF8);
            sql = sql.Replace("[id]", incidentId.ToString());
            if (isFiest)
            {
                while (sql.IndexOf("[isFirstRecord]") != -1)
                {
                    int startIndex = sql.IndexOf("[isFirstRecord]");
                    int endIndex = sql.IndexOf("[/isFirstRecord]");
                    sql = sql.Remove(startIndex, endIndex - startIndex + 16);
                }
            }
            else
            {
                sql = sql.Replace("[isFirstRecord]", "");
                sql = sql.Replace("[/isFirstRecord]", "");
            }
            TextFile.WriteAll(path, Encoding.UTF8, sql);
            MySqlAction.ExecuteSqlFile(path);
            File.Delete(path);
        }
        //删除作废表格
        public static void DeleteErrorTable(uint incidentId)
        {
            if (incidentId == 1)
                return;
            for (uint i = 1; i < incidentId; i++)
                RecordMapper.DeleteTable(i);
        }
        //判断两个bean是否相同
        public static bool IsSameBean(RecordBean bean, RecordBean targetBean)
        {
            return bean.Size == targetBean.Size
                && bean.FileCount == targetBean.FileCount
                && bean.DirCount == targetBean.DirCount
                && bean.SpaceUsage == targetBean.SpaceUsage;
        }
        //通过路径获取最新的记录节点
        public static RecordBean GetLastBean(string path)
        {
            DirIndexBean dirIndex = DirIndexMapper.GetOneByPath(path);
            if (dirIndex == null)
                return null;
            RecordBean bean = RecordMapper.GetOneById(dirIndex.TargectId, dirIndex.IncidentId);
            if (bean == null)
                return null;
            //告知该bean是属于哪一个事件的
            bean.IncidentId = dirIndex.IncidentId;
            return bean;
        }
    }
}
