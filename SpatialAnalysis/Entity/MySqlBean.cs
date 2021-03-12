using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialAnalysis.Entity
{
    class MySqlBean
    {
        //添加访问器，这样才可以将数据绑定到页面上
        public string LocalMySql { get; set; }
        public string MySqlConnect { get; set; }
    }
}
