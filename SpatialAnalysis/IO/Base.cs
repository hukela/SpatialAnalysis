using System.Configuration;

namespace SpatialAnalysis.IO
{
    class Base
    {
        static string locolPath = ConfigurationManager.AppSettings["ServerIP"];
    }
}
