using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System.Data;

namespace SpatialAnalysis.Mapper
{
    static class IncidentMapper
    {
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="bean">对应的bean</param>
        /// <returns>该行的id</returns>
        public static int AddOne(IncidentBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "INSERT INTO " +
                "incident (creat_time,title,`explain`,`type`) " +
                "value (@creat_time,@title,@explain,@type);";
                cmd.Parameters.Add("creat_time", MySqlDbType.DateTime).Value = bean.CreatTime.ToString();
                cmd.Parameters.Add("title", MySqlDbType.VarChar, 20).Value = bean.Title;
                cmd.Parameters.Add("explain", MySqlDbType.VarChar, 500).Value = bean.Explain;
                cmd.Parameters.Add("type", MySqlDbType.UByte).Value = (int)bean.Type;
                MySqlAction.Write(cmd);
            }
            int id;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                DataTable table = MySqlAction.Read(cmd);
                id = (int)table.Rows[0][0];
            }
            return id;
        }
    }
}
