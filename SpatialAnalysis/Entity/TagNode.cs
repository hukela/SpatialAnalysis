using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialAnalysis.Entity
{
    class TagNode
    {
        public TagBean Tag { get; set; }
        public TagNode[] Children { get; set; }
    }
}
