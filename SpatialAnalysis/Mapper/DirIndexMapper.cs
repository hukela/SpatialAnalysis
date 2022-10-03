using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;
using System.Data.SQLite;

namespace SpatialAnalysis.Mapper
{
internal static class DirIndexMapper
{
    /// <summary>
    /// 添加一行数据
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void AddOne(DirIndexBean bean)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "INSERT INTO [dir_index] VALUES (@path, @incident_id, @target_id);";
            cmd.Parameters.Add("path", DbType.String).Value = bean.Path;
            cmd.Parameters.Add("incident_id", DbType.UInt32).Value = bean.IncidentId;
            cmd.Parameters.Add("target_id", DbType.UInt64).Value = bean.TargetId;
            SQLiteClient.Write(cmd);
        }
    }
    //将数据由表格转化为bean
    private static DirIndexBean GetBeanByTable(DataRow row)
    {
        return new DirIndexBean()
        {
            Path = (string)row["path"],
            IncidentId = Convert.ToUInt32(row["incident_id"]),
            TargetId = Convert.ToUInt64(row["target_id"]),
        };
    }
    /// <summary>
    /// 通过路径获取一个数据实体
    /// </summary>
    /// <param name="path">文件夹路径</param>
    /// <returns>数据实体</returns>
    public static DirIndexBean GetOneByPath(string path)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "SELECT * FROM [dir_index] WHERE [path] = @path;";
            cmd.Parameters.Add("path", DbType.String).Value = path;
            DataTable table = SQLiteClient.Read(cmd);
            return table.Rows.Count == 0 ? null : GetBeanByTable(table.Rows[0]);
        }
    }
    /// <summary>
    /// 刷新一行数据
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void RefreshIndex(DirIndexBean bean)
    {
        int n;
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "UPDATE [dir_index] SET [incident_id] = @incident_id, [target_id] = @target_id WHERE [path] = @path;";
            cmd.Parameters.Add("path", DbType.String).Value = bean.Path;
            cmd.Parameters.Add("incident_id", DbType.UInt32).Value = bean.IncidentId;
            cmd.Parameters.Add("target_id", DbType.UInt64).Value = bean.TargetId;
            n = SQLiteClient.Write(cmd);
        }
        if (n == 0)
            AddOne(bean);
    }
    /// <summary>
    /// 清空索引表数据
    /// </summary>
    public static void CleanAll()
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "DELETE FROM [dir_index] WHERE TRUE;";
            SQLiteClient.Write(cmd);
        }
    }
} }
