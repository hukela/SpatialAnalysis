using System.Configuration;

namespace SpatialAnalysis.IO
{
    class Base
    {
        public static readonly string locolPath;
        //静态的构造函数
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
                default: break;
            }
            locolPath = path;
        }
    }
}
