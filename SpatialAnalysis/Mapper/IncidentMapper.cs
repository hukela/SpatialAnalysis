﻿using MySql.Data.MySqlClient;
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
                "`incident` (`create_time`,`title`,`explain`,`incident_state`) " +
                "VALUE (@create_time,@title,@explain,@incident_state);";
                cmd.Parameters.Add("create_time", MySqlDbType.DateTime).Value = bean.CreateTime.ToString();
                cmd.Parameters.Add("title", MySqlDbType.VarChar, 20).Value = bean.Title;
                cmd.Parameters.Add("explain", MySqlDbType.VarChar, 500).Value = bean.Explain;
                cmd.Parameters.Add("incident_state", MySqlDbType.Byte).Value = bean.IncidentState;
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
        private static IncidentBean GetBeanByTable(DataRow row)
        {
            return new IncidentBean()
            {
                Id = (uint)row["id"],
                CreateTime = (DateTime)row["create_time"],
                Title = row["title"] as string,
                Explain = row["explain"] as string,
                IncidentState = (sbyte)row["incident_state"],
            };
        }
        /// <summary>
        /// 获取最近一条事件
        /// </summary>
        public static IncidentBean GetLastBean()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `incident` " +
                    "WHERE `incident_state` = 0 ORDER BY `create_time` DESC LIMIT 1;";
                DataTable table = MySqlAction.Read(cmd);
                if (table.Rows.Count == 0)
                    return null;
                else
                    return GetBeanByTable(table.Rows[0]);
            }
        }
        /// <summary>
        /// 查看是否是第一个有效记录
        /// </summary>
        public static bool IsFirstRecord()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT `id` FROM `incident` WHERE `incident_state` = 0";
                DataTable table = MySqlAction.Read(cmd);
                if (table.Rows.Count == 0)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 通过id设置事件状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="state">状态</param>
        public static void SetStateById(uint id, sbyte state)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE `incident` SET `incident_state` = @incident_state WHERE `id` = @id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt32).Value = id;
                cmd.Parameters.Add("incident_state", MySqlDbType.Byte).Value = state;
                MySqlAction.Write(cmd);
            }
        }
    }
}
