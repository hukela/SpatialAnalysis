using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.Mapper;
using System.Text;

namespace SpatialAnalysis.Service.AddRecordExtend
{
    internal class Extend
    {
        //建立表格
        public static void BuildTable(uint incidentId, bool isFiest)
        {
            string path = Base.locolPath + @"\Data\record.sql";
            string sql = TextFile.ReadAll(path, Encoding.UTF8);
            sql = sql.Replace("{incidentId}", incidentId.ToString());
            if (isFiest)
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
        //删除作废表格
        public static void DeleteErrorTable(uint incidentId)
        {
            if (incidentId == 1)
                return;
            for (uint i = 1; i < incidentId; i++)
                RecordMapper.DeleteTable(i);
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
