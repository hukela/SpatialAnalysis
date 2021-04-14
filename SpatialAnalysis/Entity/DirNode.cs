using System.Windows.Controls;

namespace SpatialAnalysis.Entity
{
    class DirNode
    {
        public uint IncidentId { get; set; }
        public ulong Id { get; set; }
        public TextBlock Name { get; set; }
        public DirNode[] Children { get; set; }
    }
}
