using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;
using System.Data.SQLite;
using System.Numerics;
using System.Text;

namespace SpatialAnalysis.Mapper
{
internal static class RecordMapper
{
    /// <summary>
    /// 添加一行数据
    /// </summary>
    /// <param name="incidentId">对应的事件id</param>
    /// <param name="bean">对应的数据实体</param>
    /// <param name="isFirst">是否是第一次记录</param>
    /// <returns>该行的id</returns>
    public static ulong InsertOne(uint incidentId, RecordBean bean, bool isFirst)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            string param = string.Empty;
            string value = string.Empty;
            if (!isFirst)
            {
                param = "[target_incident_id],";
                value = "@targetIncidentId,";
            }
            cmd.CommandText = string.Concat(
                "INSERT INTO [record_",
                incidentId,"] (" +
                "[parent_id],",
                param,
                "[from_incident_id]," +
                "[path]," +
                "[plies]," +
                "[size]," +
                "[space_usage]," +
                "[create_time]," +
                "[modify_time]," +
                "[visit_time]," +
                "[owner]," +
                "[exception_code]," +
                "[file_count]," +
                "[dir_count]) " +
                "VALUES (" +
                "@parent_id,",
                value,
                "@fromIncidentId" +
                "@path," +
                "@plies," +
                "@size," +
                "@space_usage," +
                "@create_time," +
                "@modify_time," +
                "@visit_time," +
                "@owner," +
                "@exception_code," +
                "@file_count," +
                "@dir_count);");
            cmd.Parameters.Add("parent_id", DbType.UInt64).Value = bean.ParentId;
            cmd.Parameters.Add("path", DbType.String).Value = bean.Path;
            cmd.Parameters.Add("plies", DbType.UInt32).Value = bean.Plies;
            cmd.Parameters.Add("size", DbType.String).Value = bean.Size.ToString();
            cmd.Parameters.Add("space_usage", DbType.String).Value = bean.SpaceUsage.ToString();
            cmd.Parameters.Add("create_time", DbType.DateTime).Value = bean.CreateTime;
            cmd.Parameters.Add("modify_time", DbType.DateTime).Value = bean.ModifyTime;
            cmd.Parameters.Add("visit_time", DbType.DateTime).Value = bean.VisitTime;
            cmd.Parameters.Add("owner", DbType.String).Value = bean.Owner;
            cmd.Parameters.Add("exception_code", DbType.Int32).Value = bean.ExceptionCode;
            cmd.Parameters.Add("file_count", DbType.UInt32).Value = bean.FileCount;
            cmd.Parameters.Add("dir_count", DbType.UInt32).Value = bean.DirCount;
            if (!isFirst)
                cmd.Parameters.Add("targetIncidentId", DbType.UInt32).Value = bean.TargetIncidentId;
            SQLiteClient.Write(cmd);
        }
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT LAST_INSERT_ROWID();";
            bean.Id = SQLiteClient.Read<ulong>(cmd)[0];
        }
        return bean.Id;
    }

    //将数据由表格转化为bean
    private static RecordBean[] GetBeansByTable(DataTable table)
    {
        int count = table.Rows.Count;
        RecordBean[] beans = new RecordBean[count];
        for (int i = 0; i < count; i++)
        {
            DataRow row = table.Rows[i];
            uint targetIncidentId = 0;
            if (table.Columns.Contains("target_incident_id"))
                targetIncidentId = Convert.ToUInt32(row["target_incident_id"]);
            RecordBean bean = new RecordBean()
            {
                Id = Convert.ToUInt64(row["id"]),
                ParentId = Convert.ToUInt64(row["parent_id"]),
                FromIncidentId = Convert.ToUInt32(row["from_incident_id"]),
                TargetIncidentId = targetIncidentId,
                Path = (string)row["path"],
                Plies = Convert.ToUInt32(row["plies"]),
                Size = BigInteger.Parse(row["size"] as string ?? "0"),
                SpaceUsage = BigInteger.Parse(row["space_usage"] as string ?? "0"),
                CreateTime = (DateTime)row["create_time"],
                ModifyTime = (DateTime)row["modify_time"],
                VisitTime = (DateTime)row["visit_time"],
                Owner = row["owner"] as string,
                ExceptionCode = Convert.ToInt32(row["exception_code"]),
                FileCount = Convert.ToUInt32(row["file_count"]),
                DirCount = Convert.ToUInt32(row["dir_count"]),
            };
            beans[i] = bean;
        }
        return beans;
    }

    /// <summary>
    /// 通过id设置父级id
    /// </summary>
    /// <param name="incidentId">对应的事件id</param>
    /// <param name="id">要修改的行id</param>
    /// <param name="ParentId">父级id</param>
    public static void UpdateParentId(uint incidentId, ulong id, ulong ParentId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "UPDATE [record_" + incidentId + "] SET [parent_id] = @parent_id WHERE [id] = @id;";
            cmd.Parameters.Add("id", DbType.UInt64).Value = id;
            cmd.Parameters.Add("parent_id", DbType.UInt64).Value = ParentId;
            SQLiteClient.Write(cmd);
        }
    }

    /// <summary>
    /// 更新记录的映射目标id
    /// </summary>
    /// <param name="incidentId">事件id</param>
    /// <param name="path">路径</param>
    /// <param name="targetIncidentId">映射目标事件id</param>
    public static void UpdateTargetIncidentId(uint incidentId, string path, uint targetIncidentId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "UPDATE [record_" + incidentId + "] SET [target_incident_id] = @targetIncidentId WHERE [path] = @path;";
            cmd.Parameters.Add("path", DbType.String).Value = path;
            cmd.Parameters.Add("targetIncidentId", DbType.UInt32).Value = targetIncidentId;
            SQLiteClient.Write(cmd);
        }
    }

    /// <summary>
    /// 更新记录的映射来源id
    /// </summary>
    /// <param name="incidentId">事件id</param>
    /// <param name="path">路径</param>
    /// <param name="fromIncidentId">映射来源事件id</param>
    public static void UpdateFromIncidentId(uint incidentId, string path, uint fromIncidentId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "UPDATE [record_" + incidentId + "] SET [from_incident_id] = @fromIncidentId WHERE [path] = @path;";
            cmd.Parameters.Add("path", DbType.String).Value = path;
            cmd.Parameters.Add("fromIncidentId", DbType.UInt32).Value = fromIncidentId;
            SQLiteClient.Write(cmd);
        }
    }

    /// <summary>
    /// 通过id获取一个数据实体
    /// </summary>
    /// <param name="incidentId">对应的事件id</param>
    /// <param name="id">id</param>
    /// <returns>数据实体</returns>
    public static RecordBean SelectById(uint incidentId, ulong id)
    {
        DataTable table;
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = string.Concat(
                "select * " +
                "from [record_", incidentId,
                "] where [id] = @id;");
            cmd.Parameters.Add("id", DbType.UInt64).Value = id;
            table = SQLiteClient.Read(cmd);
        }
        if (table.Rows.Count == 0)
            return null;
        else
            return GetBeansByTable(table)[0];
    }

    /// <summary>
    /// 查找根记录
    /// </summary>
    /// <param name="incidentId">对应事件id</param>
    public static RecordBean[] SelectRootRecords(uint incidentId)
    {
        DataTable table;
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = string.Concat(
                "select * " +
                "from [record_", incidentId,
                "] where [parent_id] = 0;");
            table = SQLiteClient.Read(cmd);
        }
        return GetBeansByTable(table);
    }

    /// <summary>
    /// 通过id获取其子一级文件夹
    /// </summary>
    /// <param name="ParentId">文件夹id</param>
    /// <param name="incidentId">事件id</param>
    /// <returns></returns>
    public static RecordBean[] SelectChildren(ulong ParentId, uint incidentId)
    {
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = string.Concat("SELECT * FROM [record_", incidentId,"] WHERE [parent_id] = @parent_id and [id] != 0");
            cmd.Parameters.Add("parent_id", DbType.UInt64).Value = ParentId;
            return GetBeansByTable(SQLiteClient.Read(cmd));
        }
    }

    /// <summary>
    /// 通过path查询对应记录
    /// </summary>
    /// <param name="incidentId">事件id</param>
    /// <param name="path">路径</param>
    public static RecordBean SelectByPath(uint incidentId, string path)
    {
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = string.Concat("SELECT * FROM [record_", incidentId,"] WHERE [path] = @path");
            cmd.Parameters.Add("path", DbType.String).Value = path;
            DataTable table = SQLiteClient.Read(cmd);
            return table.Rows.Count == 0 ? null : GetBeansByTable(table)[0];
        }
    }

    /// <summary>
    /// 通过path查询对应记录 (批量)
    /// </summary>
    /// <param name="incidentId">事件id</param>
    /// <param name="paths">多个路径</param>
    public static RecordBean[] SelectByPaths(uint incidentId, string[] paths)
    {
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM [record_").Append(incidentId).Append("] WHERE [path] IN (");
            for (int i = 0; i < paths.Length; i++)
            {
                builder.Append("@path").Append(i);
                if (i != paths.Length - 1)
                    builder.Append(",");
            }
            builder.Append(")");
            cmd.CommandText = builder.ToString();
            for (int i = 0; i < paths.Length; i++)
                cmd.Parameters.Add("path" + i, DbType.String).Value = paths[i];
            return GetBeansByTable(SQLiteClient.Read(cmd));
        }
    }

    /// <summary>
    /// 获取表中的记录总数
    /// </summary>
    /// <param name="incidentId">对应的事件id</param>
    /// <returns>记录总数</returns>
    public static ulong Count(uint incidentId)
    {
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT COUNT(1) FROM [record_" + incidentId + "];";
            return SQLiteClient.Read<ulong>(cmd)[0];
        }
    }

    /// <summary>
    /// 删除对应事件的记录表格
    /// </summary>
    /// <param name="incidentId">事件id</param>
    public static void DeleteTable(uint incidentId)
    {
        using (SQLiteCommand  cmd = new SQLiteCommand ())
        {
            cmd.CommandText = string.Concat("DROP TABLE IF EXISTS [record_", incidentId,"];");
            SQLiteClient.Write(cmd);
        }
    }
} }
