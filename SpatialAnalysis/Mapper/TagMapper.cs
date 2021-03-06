﻿using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.Data;

namespace SpatialAnalysis.Mapper
{
    class TagMapper
    {
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        public static void AddOne(TagBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "INSERT INTO `tag` (`parent_id`, `name`, `color`) VALUE (@parent_id, @name, @color);";
                object pid = bean.ParentId == 0 ? null : (object)bean.ParentId;
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt32).Value = pid;
                cmd.Parameters.Add("name", MySqlDbType.VarChar, 30).Value = bean.Name;
                cmd.Parameters.Add("color", MySqlDbType.String, 7).Value = bean.Color;
                MySqlAction.Write(cmd);
            }
        }
        //将表格转换为bean
        private static TagBean[] GetBeanListByTable(DataTable table)
        {
            TagBean[] list = new TagBean[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                uint parentId = table.Rows[i]["parent_id"] is DBNull ? 0 : (uint)table.Rows[i]["parent_id"];
                TagBean bean = new TagBean()
                {
                    Id = (uint)table.Rows[i]["id"],
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `tag` WHERE `parent_id` IS NULL;";
                return GetBeanListByTable(MySqlAction.Read(cmd));
            }
        }
        /// <summary>
        /// 获取所有子标签
        /// </summary>
        /// <param name="ParentId">父级标签id</param>
        public static TagBean[] GetChildTag(uint ParentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `tag` WHERE `parent_id` = @parent_id;";
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt32).Value = ParentId;
                return GetBeanListByTable(MySqlAction.Read(cmd));
            }
        }
        /// <summary>
        /// 通过标签id获取数据实体
        /// </summary>
        public static TagBean GetOneById(uint tagId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `tag` WHERE `id` = @id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt32).Value = tagId;
                return GetBeanListByTable(MySqlAction.Read(cmd))[0];
            }
        }
        /// <summary>
        /// 更新已有标签
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        public static void UpdataOne(TagBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE `tag` SET `name`=@name, `color`=@color WHERE `id`=@id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt32).Value = bean.Id;
                cmd.Parameters.Add("name", MySqlDbType.VarChar, 30).Value = bean.Name;
                cmd.Parameters.Add("color", MySqlDbType.String, 7).Value = bean.Color;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 根据id删除一个标签
        /// </summary>
        public static void DeleteOne(uint tagId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "DELETE FROM `tag` WHERE `id`=@id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt32).Value = tagId;
                MySqlAction.Write(cmd);
            }
        }
    }
}
