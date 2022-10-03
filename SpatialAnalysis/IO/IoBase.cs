using System.Configuration;

namespace SpatialAnalysis.IO
{
    internal static class IoBase
    {
        public static readonly string localPath;
        //静态构造函数
        static IoBase()
        {
            string path = System.Environment.CurrentDirectory;
            string runMode = ConfigurationManager.AppSettings["RunMode"];
            switch (runMode)
            {
                case "Debug":
                    path = path.Remove(path.Length - 10);
                    break;
                case "Release":
                    path = path.Remove(path.Length - 12);
                    break;
                case "Test":
                    path = path.Remove(path.Length - 15) + @"\SpatialAnalysis";
                    break;
                default: break;
            }
            localPath = path;
        }
    }
}
