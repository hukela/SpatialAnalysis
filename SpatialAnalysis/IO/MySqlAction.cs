using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Xml;
using System.Data;
using System.ServiceProcess;
using System.Text;

namespace SpatialAnalysis.IO
{
    /// <summary>
    /// 针对MySql软件的各种操作
    /// </summary>
    static class MySqlAction
    {
        /// <summary>
        /// 查看是否连接
        /// </summary>
        public static bool IsConnected
        {
            get
            {
                if (con == null)
                    return false;
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    return false;
                else
                    return true;
            }
        }
        private static MySqlConnection con = null;
        /// <summary>
        /// 刷新连接配置
        /// </summary>
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
        /// <summary>
        /// 执行sql文件
        /// </summary>
        /// <param name="path"></param>
        public static void ExecuteSqlFile(string path)
        {
            string sql = TextFile.ReadAll(path, Encoding.UTF8);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 获取MySql服务状态
        /// </summary>
        public static ServerState State
        {
            get
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == "MySQL")
                    {
                        ServiceControllerStatus status = service.Status;
                        service.Dispose();
                        if (status == ServiceControllerStatus.Stopped)
                            return ServerState.Stopped;
                        else
                            return ServerState.Running;
                    }
                }
                return ServerState.NoServer;
            }
        }
        /// <summary>
        /// 开启MySql服务
        /// </summary>
        public static void StartServer()
        {
            using (ServiceController service = new ServiceController("MySQL"))
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                    service.Start();
            }
        }
        /// <summary>
        /// 关闭MySql服务
        /// </summary>
        public static void StopServer()
        {
            using (ServiceController service = new ServiceController("MySQL"))
            {
                if (service.Status == ServiceControllerStatus.Running)
                    service.Stop();
            }
        }
    }
}
