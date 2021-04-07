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
        /// 以表格形式获取对应标签id的标注路径
        /// </summary>
        public static DataTable GetAllByTag(uint tagId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `dir_tag` WHERE `tag_id` = @tag_id;";
                cmd.Parameters.Add("tag_id", MySqlDbType.UInt32).Value = tagId;
                return MySqlAction.Read(cmd);
            }
        }
    }
}
