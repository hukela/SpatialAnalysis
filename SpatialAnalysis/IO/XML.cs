using System.Xml.Linq;

namespace SpatialAnalysis.IO.Xml
{
    class XML: Base
    {
        //存储文件位置
        private const string filePath = "/Data/Core.xml";
        //Map相关参数
        public enum Params
        {
            test
        }
        public static decimal Map(Params param)
        {
            return 1;
        }
        public static void Map(Params param, object obj)
        {
            string key = param.ToString();
        }
    }
}
