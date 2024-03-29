﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SpatialAnalysis.Entity
{
/// <summary>
/// 获取文件异常类
/// </summary>
[Flags]
internal enum RecordExCode
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
}

internal static class RecordExCodeMap
{
    public static string[] GetValues(int code)
    {
        if (code == 0)
            return null;
        RecordExCode exCode = (RecordExCode)code;
        List<string> list = new List<string>();
        if ((exCode & RecordExCode.UnauthorizedAccess) != 0)
            list.Add("权限不足");
        if ((exCode & RecordExCode.IOExceptionForGetFile) != 0)
            list.Add("读取时IO异常");
        if ((exCode & RecordExCode.NotFound) != 0)
            list.Add("未找到文件");
        if ((exCode & RecordExCode.SpaceUsageException) != 0)
            list.Add("读取文件占用空间失败");
        return list.ToArray();
    }
} }
