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
            bool isDebug = bool.Parse(ConfigurationManager.AppSettings["debug"]);
            if(isDebug)
                path = path.Remove(path.Length - 10);
            locolPath = path;
        }
    }
}
