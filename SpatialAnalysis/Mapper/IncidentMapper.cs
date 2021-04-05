using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
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
        public static uint AddOne(IncidentBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "INSERT INTO " +
                "incident (`creat_time`,`title`,`explain`,`incident_state`) " +
                "value (@creat_time,@title,@explain,@incident_state);";
                cmd.Parameters.Add("creat_time", MySqlDbType.DateTime).Value = bean.CreatTime.ToString();
                cmd.Parameters.Add("title", MySqlDbType.VarChar, 20).Value = bean.Title;
                cmd.Parameters.Add("explain", MySqlDbType.VarChar, 500).Value = bean.Explain;
                cmd.Parameters.Add("incident_state", MySqlDbType.Byte).Value = bean.Incident_state;
                MySqlAction.Write(cmd);
            }
            uint id;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                DataTable table = MySqlAction.Read(cmd);
                //查询id默认类型为ulong
                id = Convert.ToUInt32(table.Rows[0][0]);
            }
            return id;
        }
        public static bool IsFirstRecord()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "select id from incident where incident_state = 1";
                DataTable table = MySqlAction.Read(cmd);
                if (table.Rows.Count == 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
