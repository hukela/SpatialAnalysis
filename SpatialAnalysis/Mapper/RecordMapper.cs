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
        /// <param name="bean">对应的数据实体</param>
        /// <param name="incidentId">对应的事件id</param>
        /// <returns>该行的id</returns>
        public static ulong AddOne(RecordBean bean, uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText =
                    "INSERT INTO record_" +
                    incidentId + " (" +
                    "`parent_record`, " +
                    "`fall_name`, " +
                    "`plies`, " +
                    "`size`, " +
                    "`space_usage`, " +
                    "`create_time`, " +
                    "`modify_time`, " +
                    "`visit_time`, " +
                    "`owner`, " +
                    "`exception_code`, " +
                    "`all_count`, " +
                    "`file_count`, " +
                    "`picture_count`, " +
                    "`video_count`, " +
                    "`project_count`, " +
                    "`zip_count`, " +
                    "`dll_count`, " +
                    "`txt_count`, " +
                    "`data_count`, " +
                    "`null_count`, " +
                    "`other_count`, " +
                    "`create_variance`, " +
                    "`create_average`) " +
                    "VALUES (" +
                    "@parent_record, " +
                    "@fall_name, " +
                    "@plies, " +
                    "@size, " +
                    "@space_usage, " +
                    "@create_time, " +
                    "@modify_time, " +
                    "@visit_time, " +
                    "@owner, " +
                    "@exception_code, " +
                    "@all_count, " +
                    "@file_count, " +
                    "@picture_count, " +
                    "@video_count, " +
                    "@project_count, " +
                    "@zip_count, " +
                    "@dll_count, " +
                    "@txt_count, " +
                    "@data_count, " +
                    "@null_count, " +
                    "@other_count, " +
                    "@create_variance, " +
                    "@create_average);";
                cmd.Parameters.Add("parent_record", MySqlDbType.UInt32).Value = bean.ParentRecord;
                cmd.Parameters.Add("fall_name", MySqlDbType.VarChar, 80).Value = bean.FullName;
                cmd.Parameters.Add("plies", MySqlDbType.UInt32).Value = bean.Plies;
                cmd.Parameters.Add("size", MySqlDbType.VarChar, 14).Value = bean.Size.ToString();
                cmd.Parameters.Add("space_usage", MySqlDbType.VarChar, 14).Value = bean.SpaceUsage.ToString();
                cmd.Parameters.Add("create_time", MySqlDbType.DateTime).Value = bean.CerateTime;
                cmd.Parameters.Add("modify_time", MySqlDbType.DateTime).Value = bean.ModifyTime;
                cmd.Parameters.Add("visit_time", MySqlDbType.DateTime).Value = bean.VisitTime;
                cmd.Parameters.Add("owner", MySqlDbType.VarChar, 30).Value = bean.Owner;
                cmd.Parameters.Add("exception_code", MySqlDbType.Byte).Value = bean.ExceptionCode;
                cmd.Parameters.Add("all_count", MySqlDbType.UInt32).Value = bean.AllCount;
                cmd.Parameters.Add("file_count", MySqlDbType.UInt32).Value = bean.FileCount;
                cmd.Parameters.Add("picture_count", MySqlDbType.UInt32).Value = bean.PictureCount;
                cmd.Parameters.Add("video_count", MySqlDbType.UInt32).Value = bean.VideoCount;
                cmd.Parameters.Add("project_count", MySqlDbType.UInt32).Value = bean.ProjectCount;
                cmd.Parameters.Add("zip_count", MySqlDbType.UInt32).Value = bean.ZipCount;
                cmd.Parameters.Add("dll_count", MySqlDbType.UInt32).Value = bean.DllCount;
                cmd.Parameters.Add("txt_count", MySqlDbType.UInt32).Value = bean.TxtCount;
                cmd.Parameters.Add("data_count", MySqlDbType.UInt32).Value = bean.DataCount;
                cmd.Parameters.Add("null_count", MySqlDbType.UInt32).Value = bean.NullCount;
                cmd.Parameters.Add("other_count", MySqlDbType.UInt32).Value = bean.OtherCount;
                cmd.Parameters.Add("create_variance", MySqlDbType.Double).Value = bean.CreateVariance;
                cmd.Parameters.Add("create_average", MySqlDbType.DateTime).Value = bean.CreateAverage;
                MySqlAction.Write(cmd);
            }
            ulong id;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                DataTable table = MySqlAction.Read(cmd);
                id = (ulong)table.Rows[0][0];
            }
            return id;
        }
        /// <summary>
        /// 通过id设置父级id
        /// </summary>
        /// <param name="id">要修改的行id</param>
        /// <param name="incidentId">对应的事件id</param>
        /// <param name="ParentId">父级id</param>
        public static void SetParentId(ulong id, ulong ParentId, uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "UPDATE record_" + incidentId +" SET parent_id = @parent_id WHERE id = @id;";
                cmd.Parameters.Add("id", MySqlDbType.UInt64).Value = id;
                cmd.Parameters.Add("parent_id", MySqlDbType.UInt64).Value = ParentId;
                MySqlAction.Write(cmd);
            }
        }
        /// <summary>
        /// 获取表中的记录总数
        /// </summary>
        /// <param name="incidentId">对应的事件id</param>
        /// <returns>记录总数</returns>
        public static long Count(uint incidentId)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT COUNT(1) FROM record_" + incidentId + ";";
                DataTable table = MySqlAction.Read(cmd);
                return (long)table.Rows[0][0];
            }
        }
    }
}
