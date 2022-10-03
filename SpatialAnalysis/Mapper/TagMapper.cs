using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;
using System.Data.SQLite;

namespace SpatialAnalysis.Mapper
{
internal static class TagMapper
{
    /// <summary>
    /// 添加一行数据
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void AddOne(TagBean bean)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "INSERT INTO [tag] ([parent_id], [name], [color]) VALUES (@parent_id, @name, @color);";
            cmd.Parameters.Add("parent_id", DbType.UInt32).Value = bean.ParentId;
            cmd.Parameters.Add("name", DbType.String).Value = bean.Name;
            cmd.Parameters.Add("color", DbType.String).Value = bean.Color;
            SQLiteClient.Write(cmd);
        }
    }
    //将表格转换为bean
    private static TagBean[] GetBeanListByTable(DataTable table)
    {
        TagBean[] list = new TagBean[table.Rows.Count];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            uint parentId = table.Rows[i]["parent_id"] is DBNull ? 0 : Convert.ToUInt32(table.Rows[i]["parent_id"]);
            TagBean bean = new TagBean()
            {
                Id = Convert.ToUInt32(table.Rows[i]["id"]),
                ParentId = parentId,
                Name = table.Rows[i]["name"] as string,
                Color = table.Rows[i]["color"] as string,
            };
            list[i] = bean;
        }
        return list;
    }
    /// <summary>
    /// 获取所有根标签
    /// </summary>
    public static TagBean[] GetRootTag()
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [tag] WHERE [parent_id] = 0;";
            return GetBeanListByTable(SQLiteClient.Read(cmd));
        }
    }
    /// <summary>
    /// 获取所有子标签
    /// </summary>
    /// <param name="ParentId">父级标签id</param>
    public static TagBean[] GetChildTag(uint ParentId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [tag] WHERE [parent_id] = @parent_id;";
            cmd.Parameters.Add("parent_id", DbType.UInt32).Value = ParentId;
            return GetBeanListByTable(SQLiteClient.Read(cmd));
        }
    }
    /// <summary>
    /// 通过标签id获取数据实体
    /// </summary>
    public static TagBean GetOneById(uint tagId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "SELECT * FROM [tag] WHERE [id] = @id;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = tagId;
            return GetBeanListByTable(SQLiteClient.Read(cmd))[0];
        }
    }
    /// <summary>
    /// 更新已有标签
    /// </summary>
    /// <param name="bean">对应的数据实体</param>
    public static void UpdataOne(TagBean bean)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "UPDATE [tag] SET [name]=@name, [color]=@color WHERE [id]=@id;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = bean.Id;
            cmd.Parameters.Add("name", DbType.String).Value = bean.Name;
            cmd.Parameters.Add("color", DbType.String).Value = bean.Color;
            SQLiteClient.Write(cmd);
        }
    }
    /// <summary>
    /// 根据id删除一个标签
    /// </summary>
    public static void DeleteOne(uint tagId)
    {
        using (SQLiteCommand cmd = new SQLiteCommand ())
        {
            cmd.CommandText = "DELETE FROM [tag] WHERE [id]=@id;";
            cmd.Parameters.Add("id", DbType.UInt32).Value = tagId;
            SQLiteClient.Write(cmd);
        }
    }
} }
