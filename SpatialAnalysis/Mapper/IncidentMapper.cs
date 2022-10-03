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
    /// <param name="bean">对应的bean</param>
    /// <returns>该行的id</returns>
    public static uint AddOne(IncidentBean bean)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "INSERT INTO " +
            "[incident] ([create_time],[title],[explain],[state]) " +
            "VALUES (@create_time,@title,@explain,@state);";
            cmd.Parameters.Add("create_time", DbType.DateTime).Value = bean.CreateTime;
            cmd.Parameters.Add("title", DbType.String).Value = bean.Title;
            cmd.Parameters.Add("explain", DbType.String).Value = bean.Explain;
            cmd.Parameters.Add("state", DbType.SByte).Value = bean.State;
            SQLiteClient.Write(cmd);
        }
        uint id;
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT LAST_INSERT_ROWID();";
            DataTable table = SQLiteClient.Read(cmd);
            //查询id默认类型为ulong
            id = Convert.ToUInt32(table.Rows[0][0]);
        }
        return id;
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
                CreateTime = (DateTime)table.Rows[i]["create_time"],
                Title = table.Rows[i]["title"] as string,
                Explain = table.Rows[i]["explain"] as string,
                State = Convert.ToSByte(table.Rows[i]["state"]),
            };
        }
        return beans;
    }
    /// <summary>
    /// 获取最近一条事件
    /// </summary>
    public static IncidentBean GetLastBean()
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [incident] " +
                "WHERE [state] = 0 ORDER BY [create_time] DESC LIMIT 1;";
            DataTable table = SQLiteClient.Read(cmd);
            if (table.Rows.Count == 0)
                return null;
            else
                return GetBeanByTable(table)[0];
        }
    }
    /// <summary>
    /// 获取所有的记录成功的事件
    /// </summary>
    public static IncidentBean[] GetSuccessIncident()
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [incident] WHERE [state] = 0;";
            return GetBeanByTable(SQLiteClient.Read(cmd));
        }
    }
    /// <summary>
    /// 查看是否是第一个有效记录
    /// </summary>
    public static bool IsFirstRecord()
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT [id] FROM [incident] WHERE [state] = 0";
            DataTable table = SQLiteClient.Read(cmd);
            return table.Rows.Count == 0;
        }
    }
    /// <summary>
    /// 通过id设置事件状态
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="state">状态</param>
    public static void SetStateById(uint id, sbyte state)
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
