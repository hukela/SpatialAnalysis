using MySql.Data.MySqlClient;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.IO.Xml;

namespace SpatialAnalysis.Service
{
    static class StartupAndExit
    {
        //当程序启动时执行
        public static void ApplicationStartup()
        {
            if (MySqlAction.State == ServerState.NoServer)
                return;
            bool autoStartServer = XML.Map(XML.Params.isAutoStartServer);
            bool autoConnent = XML.Map(XML.Params.isAutoConnent);
            if (autoStartServer)
            {
                Log.Info("打开服务");
                MySqlAction.StartServer();
            }
            try
            {
                if (autoConnent)
                {
                    Log.Info("打开连接");
                    MySqlAction.OpenConnect();
                }
            }
            catch (MySqlException ex)
            {
                Log.Error("数据库连接失败。" + ex.Message);
            }
        }
        //当程序关闭时执行
        public static void ApplicationExit()
        {
            Log.Info("程序退出");
            bool autoStartServer = XML.Map(XML.Params.isAutoStartServer);
            bool autoConnent = XML.Map(XML.Params.isAutoConnent);
            if (autoConnent || autoStartServer)
            {
                Log.Info("关闭连接");
                MySqlAction.CloseConnect();
            }
            if (autoStartServer)
            {
                Log.Info("关闭服务");
                MySqlAction.StopServer();
            }
        }
    }
}
