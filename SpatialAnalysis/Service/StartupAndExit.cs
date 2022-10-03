using SpatialAnalysis.IO;

namespace SpatialAnalysis.Service
{
    internal static class StartupAndExit
    {
        //当程序启动时执行
        public static void ApplicationStartup()
        {
            SQLiteClient.OpenConnect();
        }
        //当程序关闭时执行
        public static void ApplicationExit()
        {
            SQLiteClient.CloseConnect();
        }
    }
}
