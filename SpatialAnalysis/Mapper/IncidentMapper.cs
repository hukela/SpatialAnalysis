using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;
using System.Data.SQLite;

namespace SpatialAnalysis.Mapper
{
internal static class IncidentMapper
{
    /// <summary>
    /// 添加一行数据
    /// </summary>
    /// <returns>该数据的自增id</returns>
    public static uint InsertOne(IncidentBean bean)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "INSERT INTO " +
            "[incident] ([title],[description],[state],[record_type],[create_time]) " +
            "VALUES (@title,@explain,@state,@recordType,@create_time);";
            cmd.Parameters.Add("title", DbType.String).Value = bean.Title;
            cmd.Parameters.Add("explain", DbType.String).Value = bean.Description;
            cmd.Parameters.Add("state", DbType.SByte).Value = bean.State;
            cmd.Parameters.Add("recordType", DbType.SByte).Value = bean.RecordType;
            cmd.Parameters.Add("create_time", DbType.DateTime).Value = bean.CreateTime;
            SQLiteClient.Write(cmd);
        }
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT LAST_INSERT_ROWID();";
            bean.Id = SQLiteClient.Read<uint>(cmd)[0];
        }
        return bean.Id;
    }

    //将表格结果改为bean
    private static IncidentBean[] GetBeanByTable(DataTable table)
    {
        int count = table.Rows.Count;
        IncidentBean[] beans = new IncidentBean[count];
        for (int i = 0; i < count; i++)
        {
            beans[i] = new IncidentBean()
            {
                Id = Convert.ToUInt32(table.Rows[i]["id"]),
                Title = table.Rows[i]["title"] as string,
                Description = table.Rows[i]["description"] as string,
                State = Convert.ToSByte(table.Rows[i]["state"]),
                RecordType = Convert.ToSByte(table.Rows[i]["record_type"]),
                CreateTime = (DateTime)table.Rows[i]["create_time"],
            };
        }
        return beans;
    }

    /// <summary>
    /// 通过id查询
    /// </summary>
    /// <param name="id">id</param>
    public static IncidentBean SelectById(uint id)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [incident] WHERE [id]=@id;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = id;
            DataTable table = SQLiteClient.Read(cmd);
            return table.Rows.Count == 0 ? null : GetBeanByTable(table)[0];
        }
    }

    /// <summary>
    /// 获取最近一条事件
    /// </summary>
    public static IncidentBean SelectLastBean()
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [incident] " +
                "WHERE [state] = 0 ORDER BY [create_time] DESC LIMIT 1;";
            DataTable table = SQLiteClient.Read(cmd);
            return table.Rows.Count == 0 ? null : GetBeanByTable(table)[0];
        }
    }

    /// <summary>
    /// 查询所有记录事件
    /// </summary>
    public static IncidentBean[] SelectAllIncidents()
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "SELECT * FROM [incident];";
            return GetBeanByTable(SQLiteClient.Read(cmd));
        }
    }

    /// <summary>
    /// 获取所有的记录成功的事件
    /// </summary>
    public static IncidentBean[] SelectSuccessIncidents()
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [incident] WHERE [state] = 0;";
            return GetBeanByTable(SQLiteClient.Read(cmd));
        }
    }

    /// <summary>
    /// 查询对应事件id的上一个事件 若不存在则返回null
    /// </summary>
    public static IncidentBean SelectLastSuccessIncident(uint id)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "SELECT * FROM [incident] WHERE [state] = 0 AND [id] < @id ORDER BY [id] DESC LIMIT 1;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = id;
            DataTable table = SQLiteClient.Read(cmd);
            return table.Rows.Count == 0 ? null : GetBeanByTable(table)[0];
        }
    }

    /// <summary>
    /// 通过id设置事件状态
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="state">状态</param>
    public static void UpdateStateById(uint id, IncidentStateEnum state)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "UPDATE [incident] SET [state] = @state WHERE [id] = @id;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = id;
            cmd.Parameters.Add("state", DbType.SByte).Value = state;
            SQLiteClient.Write(cmd);
        }
    }
} }
