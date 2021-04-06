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
                cmd.CommandText = "INSERT INTO `tag` (`parent_id`, `name`, `color`);) VALUE (@parent_id, @name, @color);";
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt32).Value = bean.ParentId;
                cmd.Parameters.Add("name", MySqlDbType.VarChar, 10).Value = bean.Name;
                cmd.Parameters.Add("color", MySqlDbType.VarChar, 7).Value = bean.Color;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 以表格形式获取全部数据
        /// </summary>
        public static DataTable GetAllInTable()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `tag`;";
                return MySqlAction.Read(cmd);
            }
        }
    }
}
