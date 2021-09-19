using System.Configuration;

namespace SpatialAnalysis.IO
{
    internal class Base
    {
        public static readonly string locolPath;
        //静态构造函数
        static Base()
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
            locolPath = path;
        }
    }
}
