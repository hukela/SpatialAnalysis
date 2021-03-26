using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System.Data;

namespace SpatialAnalysis.Mapper
{
    class RecordMapper
    {
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="bean">对应的bean</param>
        /// <returns>该行的id</returns>
        public static int AddOne(RecordBean bean)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "INSERT INTO record " +
                    "(parent_record, fall_name, `type`, plies, " +
                    "size, space_usage, create_time, modify_time, visit_time, `owner`, " +
                    "file_count, picture_count, video_count, project_count, exe_count, " +
                    "dll_count. txt_count, config_count, null_count, other_count, " +
                    "create_variance, create_average) " +
                    "VALUES " +
                    "(@parent_record, @fall_name, @type, " +
                    "@plies, @size, @space_usage, @create_time, @modify_time, " +
                    "@visit_time, @owner, @file_count, @picture_count, @video_count, " +
                    "@project_count, @exe_count, @dll_count. txt_count, @config_count, " +
                    "@null_count, @other_count, @create_variance, @create_average);";
                cmd.Parameters.Add("parent_record", MySqlDbType.UInt32).Value = bean.ParentRecord;
                cmd.Parameters.Add("fall_name", MySqlDbType.VarChar, 80).Value = bean.FullName;
                cmd.Parameters.Add("type", MySqlDbType.Bit).Value = bean.Type;
                cmd.Parameters.Add("plies", MySqlDbType.UInt32).Value = bean.Plies;
                cmd.Parameters.Add("size", MySqlDbType.VarChar, 14).Value = bean.Size.ToString();
                cmd.Parameters.Add("space_usage", MySqlDbType.VarChar, 14).Value = bean.SpaceUsage.ToString();
                cmd.Parameters.Add("create_time", MySqlDbType.DateTime).Value = bean.CerateTime;
                cmd.Parameters.Add("modify_time", MySqlDbType.DateTime).Value = bean.ModifyTime;
                cmd.Parameters.Add("visit_time", MySqlDbType.DateTime).Value = bean.VisitTime;
                cmd.Parameters.Add("owner", MySqlDbType.VarChar, 30).Value = bean.Owner;
                cmd.Parameters.Add("file_count", MySqlDbType.UInt64).Value = bean.FileCount;
                cmd.Parameters.Add("picture_count", MySqlDbType.UInt64).Value = bean.PictureCount;
                cmd.Parameters.Add("video_count", MySqlDbType.UInt64).Value = bean.VideoCount;
                cmd.Parameters.Add("project_count", MySqlDbType.UInt64).Value = bean.ProjectCount;
                cmd.Parameters.Add("exe_count", MySqlDbType.UInt64).Value = bean.ExeCount;
                cmd.Parameters.Add("dll_count", MySqlDbType.UInt64).Value = bean.DllCount;
                cmd.Parameters.Add("txt_count", MySqlDbType.UInt64).Value = bean.TxtCount;
                cmd.Parameters.Add("config_count", MySqlDbType.UInt64).Value = bean.ConfigCount;
                cmd.Parameters.Add("null_count", MySqlDbType.UInt64).Value = bean.NullCount;
                cmd.Parameters.Add("other_count", MySqlDbType.UInt64).Value = bean.OtherCount;
                cmd.Parameters.Add("create_variance", MySqlDbType.Double).Value = bean.CreateVariance;
                cmd.Parameters.Add("Create_average", MySqlDbType.DateTime).Value = bean.CreateAverage;
                MySqlAction.Write(cmd);
            }
            int id;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                DataTable table = MySqlAction.Read(cmd);
                id = (int)table.Rows[0][0];
            }
            return id;
        }
        public static void SetParentId(ulong childId, ulong ParentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE record SET parent_id = @parent_id WHERE id = @id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt64).Value = childId;
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt64).Value = ParentId;
                MySqlAction.Write(cmd);
            }
        }
    }
}
