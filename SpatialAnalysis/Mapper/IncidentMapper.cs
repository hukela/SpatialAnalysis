using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;

namespace SpatialAnalysis.Mapper
{
    static class IncidentMapper
    {
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
                return MySqlAction.Write(cmd);
            }
        }
    }
}
