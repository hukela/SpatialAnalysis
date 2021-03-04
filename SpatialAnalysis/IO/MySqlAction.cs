using MySql.Data.MySqlClient;
using SpatialAnalysis.IO.Xml;
using System.Data;

namespace SpatialAnalysis.IO
{
    /// <summary>
    /// 针对MySql软件的各种操作
    /// </summary>
    static class MySqlAction
    {
        private static MySqlConnection con = null;
        //获取连接用字符串
        public static void RefreshCon()
        {
            CloseConnect();
            string conString =
                "server=" + XML.Map(XML.Params.server) + ";" +
                "port=" + XML.Map(XML.Params.port) + ";" +
                "user=" + XML.Map(XML.Params.user) + ";" +
                "password='" + XML.Map(XML.Params.password) + "';";
            string database = XML.Map(XML.Params.database);
            if (database != null)
                conString = conString + "database=" + database + ";";
            con = new MySqlConnection(conString);
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        public static void OpenConnect()
        {
            if (con == null)
                RefreshCon();
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void CloseConnect()
        {
            if (con == null)
                return;
            if (con.State != ConnectionState.Closed && con.State != ConnectionState.Broken)
                con.Close();
        }
        /// <summary>
        /// 读取数据库数据
        /// </summary>
        /// <param name="cmd">sql语句</param>
        /// <returns>表格数据</returns>
        public static DataTable Read(MySqlCommand cmd)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader.GetSchemaTable();
        }
        /// <summary>
        /// 更改数据库数据
        /// </summary>
        /// <param name="cmd">sql语句</param>
        /// <returns>被改变的行数</returns>
        public static int Write(MySqlCommand cmd)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            return cmd.ExecuteNonQuery();
        }
        public static void ExecuteSqlFile(string path)
        {
        }
    }
}
