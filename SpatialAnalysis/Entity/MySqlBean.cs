namespace SpatialAnalysis.Entity
{
    class MySqlBean
    {
        //添加访问器，这样才可以将数据绑定到页面上
        public string LocalMySql { get; set; }
        public string MySqlConnect { get; set; }
        //是否有本地服务器
        public bool haveLocalMySql;
    }
}
