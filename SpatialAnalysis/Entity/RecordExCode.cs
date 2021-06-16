using System;

namespace SpatialAnalysis.Entity
{
    //获取文件异常类，使用位标志枚举，让多个异常可以共存
    [Flags]
    enum RecordExCode
    {
        //正常
        Normal = 0x0,
        // === 无法查看文件夹内容的异常 ===
        //权限不足
        UnauthorizedAccess = 0x1,
        //IO异常
        IOExceptionForGetFile = 0x2,
        // === 无法查看文件信息的异常 ===
        //文件或文件夹在读取时被删除，导致的异常
        NotFound = 0x4,
        //获取子文件占用空间失败
        SpaceUsageException = 0x8,
        // === 获取文件或文件夹所有者异常 ===
        IdentityNotMappedException = 0x10,
        ArgumentException = 0x20,
        UnauthorizedAccessException = 0x40,
        InvalidOperationException = 0x80,
    }
}
