using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;
using System.Data.SQLite;

namespace SpatialAnalysis.Mapper
{
internal static class DirTagMapper
{
    /// <summary>
    /// 添加一行数据
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void InsertOne(DirTagBean bean)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "INSERT INTO [dir_tag] ([tag_id], [path]) VALUES (@tag_id, @path);";
            cmd.Parameters.Add("tag_id", DbType.UInt32).Value = bean.TagId;
            cmd.Parameters.Add("path", DbType.String).Value = bean.Path;
            SQLiteClient.Write(cmd);
        }
    }
    //将表格数据转换为bean数据
    private static DirTagBean[] GetBeanByTable(DataTable table)
    {
        int count = table.Rows.Count;
        DirTagBean[] beans = new DirTagBean[count];
        for (int i = 0; i < count; i++)
        {
            DirTagBean bean = new DirTagBean()
            {
                Id = Convert.ToUInt32(table.Rows[i]["id"]),
                TagId = Convert.ToUInt32(table.Rows[i]["tag_id"]),
                Path = table.Rows[i]["path"] as string,
            };
            beans[i] = bean;
        }
        return beans;
    }
    /// <summary>
    /// 通过id设定标签标注的路径
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void UpdateById(uint id, string path)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "UPDATE [dir_tag] SET [path]=@path WHERE id=@id";
            cmd.Parameters.Add("id", DbType.UInt32).Value = id;
            cmd.Parameters.Add("path", DbType.String).Value = path;
            SQLiteClient.Write(cmd);
        }
    }
    /// <summary>
    /// 通过path修改标注的标签
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void UpdateByPath(string path, uint tagId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "UPDATE [dir_tag] SET [tag_id]=@tag_id WHERE [path]=@path";
            cmd.Parameters.Add("tag_id", DbType.UInt32).Value = tagId;
            cmd.Parameters.Add("path", DbType.String, 255).Value = path;
            SQLiteClient.Write(cmd);
        }
    }
    /// <summary>
    /// 删除一行数据
    /// </summary>
    /// <param name="id">对应的数据id</param>
    public static void DeleteOneById(uint id)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "DELETE FROM [dir_tag] WHERE [id]=@id;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = id;
            SQLiteClient.Write(cmd);
        }
    }
    /// <summary>
    /// 删除一行数据
    /// </summary>
    /// <param name="id">对应的数据path</param>
    public static void DeleteOneByPath(string path)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "DELETE FROM [dir_tag] WHERE [path]=@path;";
            cmd.Parameters.Add("path", DbType.String).Value = path;
            SQLiteClient.Write(cmd);
        }
    }
    /// <summary>
    /// 删除该标签的所有数据
    /// </summary>
    /// <param name="tagId">标签id</param>
    public static void DeleteByTagId(uint tagId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "DELETE FROM [dir_tag] WHERE [tag_id]=@tag_id;";
            cmd.Parameters.Add("tag_id", DbType.UInt32).Value = tagId;
            SQLiteClient.Write(cmd);
        }
    }
    /// <summary>
    /// 获取所有的数据
    /// </summary>
    /// <returns></returns>
    public static DirTagBean[] SelectAll()
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "SELECT * FROM [dir_tag];";
            return GetBeanByTable(SQLiteClient.Read(cmd));
        }
    }
    /// <summary>
    /// 获取对应标签id的标注路径
    /// </summary>
    public static DirTagBean[] SelectByTagId(uint tagId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "SELECT * FROM [dir_tag] WHERE [tag_id] = @tag_id;";
            cmd.Parameters.Add("tag_id", DbType.UInt32).Value = tagId;
            return GetBeanByTable(SQLiteClient.Read(cmd));
        }
    }

    /// <summary>
    /// 获取对应标签id的标注路径
    /// </summary>
    public static string[] selectPathByTagId(uint tagId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
            cmd.CommandText = "SELECT [path] FROM [dir_tag] WHERE [tag_id] = @tag_id;";
            cmd.Parameters.Add("tag_id", DbType.UInt32).Value = tagId;
            DataTable table = SQLiteClient.Read(cmd);
            int count = table.Rows.Count;
            string[] paths = new string[count];
            for (int i = 0; i < count; i++)
            {
                paths[i] = table.Rows[i]["path"] as string;
            }
            return paths;
        }
    }
} }
