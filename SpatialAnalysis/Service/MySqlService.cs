using System;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Xml;

namespace SpatialAnalysis.Service
{
    class MySqlService
    {
        public static MySqlBean GetBean()
        {
            MySqlBean bean = new MySqlBean();
            ServerState server = MySqlAction.State;
            switch (server)
            {
                case ServerState.NoServer:
                    bean.LocalMySql = "没有检测到";
                    break;
                case ServerState.Stopped:
                    bean.LocalMySql = "没有运行";
                    break;
                case ServerState.Running:
                    bean.LocalMySql = "正在运行";
                    break;
            }
            if (MySqlAction.IsConnected)
                bean.MySqlConnect = "已连接";
            else
                bean.MySqlConnect = "未连接";
            return bean;
        }
    }
}
