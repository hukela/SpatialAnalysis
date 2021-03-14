using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.IO.Xml;
using System;
using System.Windows;

namespace SpatialAnalysis.Service
{
    class MySqlService
    {
        //获取主窗口
        static MainWindow main = (MainWindow)Application.Current.MainWindow;
        //获取页面相关数据
        public static MySqlBean GetBean()
        {
            MySqlBean bean = new MySqlBean();
            ServerState server = MySqlAction.State;
            switch (server)
            {
                case ServerState.NoServer:
                    bean.LocalMySql = "没有检测到";
                    bean.haveLocalMySql = false;
                    break;
                case ServerState.Stopped:
                    bean.LocalMySql = "没有运行";
                    bean.haveLocalMySql = true;
                    break;
                case ServerState.Running:
                    bean.LocalMySql = "正在运行";
                    bean.haveLocalMySql = true;
                    break;
            }
            if (MySqlAction.IsConnected)
                bean.MySqlConnect = "已连接";
            else
                bean.MySqlConnect = "未连接";
            bean.User = XML.Map(XML.Params.user);
            bean.Password = XML.Map(XML.Params.password);
            bean.Server = XML.Map(XML.Params.server);
            bean.Port = XML.Map(XML.Params.port);
            bean.Database = XML.Map(XML.Params.database);
            object auto; //防止空异常
            auto = XML.Map(XML.Params.autoStartServer);
            if (auto == null)
                bean.AutoStartServer = false;
            else
                bean.AutoStartServer = (bool)auto;
            auto = XML.Map(XML.Params.autoConnent);
            if (auto == null)
                bean.AutoConnent = false;
            else
                bean.AutoConnent = (bool)auto;
            return bean;
        }
        //保存相关配置
        public static void SaveConfig(MySqlBean bean)
        {
            XML.Map(XML.Params.server, bean.Server);
            XML.Map(XML.Params.port, bean.Port);
            XML.Map(XML.Params.user, bean.User);
            XML.Map(XML.Params.password, bean.Password);
            XML.Map(XML.Params.database, bean.Database);
            XML.Map(XML.Params.autoStartServer, bean.AutoStartServer);
            XML.Map(XML.Params.autoConnent, bean.AutoConnent);
        }
        public static void OpenConnect()
        {
            Log.Info("打开连接");
            try
            {
                MySqlAction.OpenConnect();
            }
            catch(Exception e)
            {
                ShowException(e);
            }
            finally
            {
                //刷新主窗口
                main.IsCanUse();
            }
        }
        public static void CloseConnect()
        {
            Log.Info("关闭连接");
            try
            {
                MySqlAction.CloseConnect();
            }
            catch (Exception e)
            {
                ShowException(e);
            }
            finally
            {
                main.IsCanUse();
            }
        }
        public static void StartServer()
        {
            Log.Info("打开服务");
            try
            {
                MySqlAction.StartServer();
            }
            catch (Exception e)
            {
                ShowException(e);
            }
            finally
            {
                main.IsCanUse();
            }
        }
        public static void StopServer()
        {
            Log.Info("关闭服务");
            try
            {
                MySqlAction.StopServer();
            }
            catch (Exception e)
            {
                ShowException(e);
            }
            finally
            {
                main.IsCanUse();
            }
        }
        //异常处理
        private static void ShowException(Exception e)
        {
            Log.Add(e, 1);
            MessageBox.Show(e.Message, "错误");
        }
    }
}
