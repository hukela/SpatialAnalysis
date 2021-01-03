using System.Xml.Linq;

namespace SpatialAnalysis.IO
{
    class XML
    {
        //存储文件
        private const string filePath = "/Data/Core.xml";
        private static readonly XElement xmlDoc = XElement.Load(filePath);
    }
}
