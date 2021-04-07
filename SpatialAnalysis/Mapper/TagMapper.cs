using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
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
                cmd.Parameters.Add("color", MySqlDbType.VarChar, 7).Value = bean.Color;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 以表格形式获取所有根标签
        /// </summary>
        public static DataTable GetRootTag()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `tag` WHERE `parent_id` IS NULL;";
                return MySqlAction.Read(cmd);
            }
        }
        public static DataTable GetChildTag(uint ParentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `tag` WHERE `parent_id` = @parent_id;";
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt32).Value = ParentId;
                return MySqlAction.Read(cmd);
            }
        }
    }
}
