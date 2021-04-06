namespace SpatialAnalysis.Entity
{
    //获取文件异常类
    enum RecordExCode
    {
        //正常
        normal,
        //权限不足导致无法查看文件夹内容
        UnauthorizedAccess,
        //文件或文件夹在读取时被删除，导致的异常
        FileNotFound
    }
}
