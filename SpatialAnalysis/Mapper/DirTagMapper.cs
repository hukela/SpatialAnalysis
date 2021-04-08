using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System.Data;

namespace SpatialAnalysis.Mapper
{
    class DirTagMapper
    {
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        public static void AddOne(DirTagBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "INSERT INTO `dir_tag` (`tag_id`, `path`) VALUE (@tag_id, @path);";
                cmd.Parameters.Add("tag_id", MySqlDbType.UInt32).Value = bean.TagId;
                cmd.Parameters.Add("path", MySqlDbType.VarChar, 255).Value = bean.Path;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 修改一行数据
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        public static void EditOne(DirTagBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE `dir_tag` SET `path`=@path WHERE id=@id";
                cmd.Parameters.Add("id", MySqlDbType.UInt32).Value = bean.Id;
                cmd.Parameters.Add("path", MySqlDbType.VarChar, 255).Value = bean.Path;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 删除一行数据
        /// </summary>
        /// <param name="id">对应的数据id</param>
        public static void DeleteOneById(uint id)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "DELETE FROM `dir_tag` WHERE `id`=@id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt32).Value = id;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 删除该标签的所有数据
        /// </summary>
        /// <param name="tagId">标签id</param>
        public static void DeleteByTagId(uint tagId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "DELETE FROM `dir_tag` WHERE `tag_id`=@tag_id;";
                cmd.Parameters.Add("tag_id", MySqlDbType.UInt32).Value = tagId;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 以表格形式获取对应标签id的标注路径
        /// </summary>
        public static DataTable GetAllByTag(uint tagId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT `id`,`path` FROM `dir_tag` WHERE `tag_id` = @tag_id;";
                cmd.Parameters.Add("tag_id", MySqlDbType.UInt32).Value = tagId;
                return MySqlAction.Read(cmd);
            }
        }
    }
}
