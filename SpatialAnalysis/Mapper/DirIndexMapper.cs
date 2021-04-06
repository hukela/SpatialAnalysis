using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System.Data;

namespace SpatialAnalysis.Mapper
{
    class DirIndexMapper
    {
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        public static void AddOne(DirIndexBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "INSERT INTO `dir_index` VALUE (@path, @incident_id, @targect_id);";
                cmd.Parameters.Add("path", MySqlDbType.VarChar, 255).Value = bean.Path;
                cmd.Parameters.Add("incident_id", MySqlDbType.UInt32).Value = bean.IncidentId;
                cmd.Parameters.Add("targect_id", MySqlDbType.UInt64).Value = bean.TargectId;
                MySqlAction.Write(cmd);
            }
        }
        //将数据由表格转化为bean
        private static DirIndexBean GetBeanByTable(DataRow row)
        {
            return new DirIndexBean()
            {
                Path = (string)row["path"],
                IncidentId = (uint)row["incident_id"],
                TargectId = (ulong)row["targect_id"],
            };
        }
        /// <summary>
        /// 通过路径获取一个数据实体
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>数据实体</returns>
        public static DirIndexBean GetOneByPath(string path)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM `dir_index` WHERE `path` = @path;";
                cmd.Parameters.Add("path", MySqlDbType.VarChar, 255).Value = path;
                DataTable table = MySqlAction.Read(cmd);
                if (table.Rows.Count == 0)
                    return null;
                else
                    return GetBeanByTable(table.Rows[0]);
            }
        }
        /// <summary>
        /// 刷新一行数据
        /// </summary>
        /// <param name="bean">对应的数据实体</param>
        public static void RefreshIndex(DirIndexBean bean)
        {
            int n;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE `dir_index` SET `incident_id` = @incident_id, `targect_id` = @targect_id WHERE `path` = @path;";
                cmd.Parameters.Add("path", MySqlDbType.VarChar, 255).Value = bean.Path;
                cmd.Parameters.Add("incident_id", MySqlDbType.UInt32).Value = bean.IncidentId;
                cmd.Parameters.Add("targect_id", MySqlDbType.UInt64).Value = bean.TargectId;
                n = MySqlAction.Write(cmd);
            }
            if (n == 0)
                AddOne(bean);
        }
        /// <summary>
        /// 清空索引表数据
        /// </summary>
        public static void CleanAll()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "DELETE FROM `dir_index` WHERE TRUE;";
                MySqlAction.Write(cmd);
            }
        }
    }
}
