using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;
using System.Numerics;

namespace SpatialAnalysis.Mapper
{
    class RecordMapper
    {
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        /// <param name="incidentId">对应的事件id</param>
        /// <returns>该行的id</returns>
        public static ulong AddOne(RecordBean bean, uint incidentId, bool isFirst)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                string param = string.Empty;
                string value = string.Empty;
                if (!isFirst)
                {
                    param = "`incident_id`, `target_id`, ";
                    value = "@incident_id, @target_id, ";
                }
                cmd.CommandText = string.Concat(
                    "INSERT INTO `record_",
                    incidentId, "` (" +
                    "`parent_id`, ",
                    param,
                    "`path`, " +
                    "`plies`, " +
                    "`size`, " +
                    "`space_usage`, " +
                    "`create_time`, " +
                    "`modify_time`, " +
                    "`visit_time`, " +
                    "`owner`, " +
                    "`exception_code`, " +
                    "`file_count`, " +
                    "`dir_count`) " +
                    "VALUES (" +
                    "@parent_id, ",
                    value,
                    "@path, " +
                    "@plies, " +
                    "@size, " +
                    "@space_usage, " +
                    "@create_time, " +
                    "@modify_time, " +
                    "@visit_time, " +
                    "@owner, " +
                    "@exception_code, " +
                    "@file_count, " +
                    "@dir_count);");
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt64).Value = bean.ParentId;
                cmd.Parameters.Add("path", MySqlDbType.VarChar, 80).Value = bean.Path;
                cmd.Parameters.Add("plies", MySqlDbType.UInt32).Value = bean.Plies;
                cmd.Parameters.Add("size", MySqlDbType.VarChar, 14).Value = bean.Size.ToString();
                cmd.Parameters.Add("space_usage", MySqlDbType.VarChar, 14).Value = bean.SpaceUsage.ToString();
                cmd.Parameters.Add("create_time", MySqlDbType.DateTime).Value = bean.CerateTime;
                cmd.Parameters.Add("modify_time", MySqlDbType.DateTime).Value = bean.ModifyTime;
                cmd.Parameters.Add("visit_time", MySqlDbType.DateTime).Value = bean.VisitTime;
                cmd.Parameters.Add("owner", MySqlDbType.VarChar, 30).Value = bean.Owner;
                cmd.Parameters.Add("exception_code", MySqlDbType.Byte).Value = bean.ExceptionCode;
                cmd.Parameters.Add("file_count", MySqlDbType.UInt32).Value = bean.FileCount;
                cmd.Parameters.Add("dir_count", MySqlDbType.UInt32).Value = bean.DirCount;
                if (!isFirst)
                {
                    cmd.Parameters.Add("incident_id", MySqlDbType.UInt32).Value = bean.IncidentId;
                    cmd.Parameters.Add("target_id", MySqlDbType.UInt64).Value = bean.TargetId;
                }
                MySqlAction.Write(cmd);
            }
            ulong id;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                DataTable table = MySqlAction.Read(cmd);
                id = (ulong)table.Rows[0][0];
            }
            return id;
        }
        //将数据由表格转化为bean
        private static RecordBean[] GetBeansByTable(DataTable table)
        {
            int count = table.Rows.Count;
            RecordBean[] beans = new RecordBean[count];
            for (int i = 0; i < count; i++)
            {
                DataRow row = table.Rows[i];
                uint incidentId = 0;
                ulong targetId = 0;
                if (table.Columns.Contains("incident_id"))
                {
                    incidentId = (uint)row["incident_id"];
                    targetId = (ulong)row["target_id"];
                }
                RecordBean bean = new RecordBean()
                {
                    Id = (ulong)row["id"],
                    ParentId = (ulong)row["parent_id"],
                    IncidentId = incidentId,
                    TargetId = targetId,
                    Path = (string)row["path"],
                    Plies = (uint)row["plies"],
                    Size = BigInteger.Parse(row["size"] as string),
                    SpaceUsage = BigInteger.Parse(row["space_usage"] as string),
                    CerateTime = (DateTime)row["create_time"],
                    ModifyTime = (DateTime)row["modify_time"],
                    VisitTime = (DateTime)row["visit_time"],
                    Owner = row["owner"] as string,
                    ExceptionCode = (sbyte)row["exception_code"],
                    FileCount = (uint)row["file_count"],
                    DirCount = (uint)row["dir_count"],
                };
                beans[i] = bean;
            }
            return beans;
        }
        /// <summary>
        /// 通过id设置父级id
        /// </summary>
        /// <param name="id">要修改的行id</param>
        /// <param name="incidentId">对应的事件id</param>
        /// <param name="ParentId">父级id</param>
        public static void SetParentId(ulong id, ulong ParentId, uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE `record_" + incidentId + "` SET `parent_id` = @parent_id WHERE `id` = @id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt64).Value = id;
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt64).Value = ParentId;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 通过id获取一个数据实体
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="incidentId">对应的事件id</param>
        /// <returns>数据实体</returns>
        public static RecordBean GetOneById(ulong id, uint incidentId)
        {
            DataTable table;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = string.Concat(
                    "select * " +
                    "from `record_", incidentId,
                    "` where `id` = @id;");
                cmd.Parameters.Add("id", MySqlDbType.UInt64).Value = id;
                table = MySqlAction.Read(cmd);
            }
            if (table.Rows.Count == 0)
                return null;
            else
                return GetBeansByTable(table)[0];
        }
        /// <summary>
        /// 通过id获取其子一级文件夹
        /// </summary>
        /// <param name="ParentId">文件夹id</param>
        /// <param name="incidentId">事件id</param>
        /// <returns></returns>
        public static RecordBean[] GetBeansByPid(ulong ParentId, uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = string.Concat("SELECT * FROM `record_", incidentId, "` WHERE `parent_id` = @parent_id and `id` != 0");
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt64).Value = ParentId;
                return GetBeansByTable(MySqlAction.Read(cmd));
            }
        }
        /// <summary>
        /// 获取表中的记录总数
        /// </summary>
        /// <param name="incidentId">对应的事件id</param>
        /// <returns>记录总数</returns>
        public static long Count(uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT COUNT(1) FROM `record_" + incidentId + "`;";
                DataTable table = MySqlAction.Read(cmd);
                return (long)table.Rows[0][0];
            }
        }
        /// <summary>
        /// 删除对应事件的记录表格
        /// </summary>
        /// <param name="incidentId">事件id</param>
        public static void DeleteTable(uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = string.Concat("DROP TABLE IF EXISTS `record_", incidentId, "`;");
                MySqlAction.Write(cmd);
            }
        }
    }
}
