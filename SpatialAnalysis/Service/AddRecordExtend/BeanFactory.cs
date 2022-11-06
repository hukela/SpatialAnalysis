using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using SpatialAnalysis.IO.Log;

namespace SpatialAnalysis.Service.AddRecordExtend
{
internal static class BeanFactory
{
    /// <summary>
    /// 获取文件相关信息
    /// </summary>
    /// <param name="file">文件info</param>
    /// <param name="plies">层数</param>
    /// <returns></returns>
    public static RecordBean BuildFileBean(FileInfo file, uint plies)
    {
        RecordBean bean = new RecordBean()
        {
            Path = file.FullName,
            Plies = plies,
            Size = file.Length,
            CreateTime = file.CreationTime,
            ModifyTime = file.LastWriteTime,
            VisitTime = file.LastAccessTime,
            ExceptionCode = (int)RecordExCode.Normal,
            FileCount = 1,
            IsFile = true,
            IsChange = false,
        };
        // 获取文件占用空间
        try
        { bean.SpaceUsage = SpaceUsage.Get(file.FullName); }
        catch (Win32Exception e)
        {
            bean.ExceptionCode = (int)RecordExCode.SpaceUsageException;
            Log.Warn($"获取文件占用空间失败。{file.FullName} {e.Message}, [error code: {bean.ExceptionCode}]");
        }
        return bean;
    }

    /// <summary>
    /// 获取文件夹相关信息
    /// </summary>
    /// <param name="dir">文件夹info</param>
    /// <param name="plies">层数</param>
    /// <returns></returns>
    public static RecordBean BuildDirBean(DirectoryInfo dir, uint plies)
    {
        string owner;
        RecordExCode errorCode = RecordExCode.Normal;
        try
        {
            DirectorySecurity security = dir.GetAccessControl();
            IdentityReference identity = security.GetOwner(typeof(NTAccount));
            owner = identity.ToString();
        }
        catch (IdentityNotMappedException)
        {
            owner = "IdentityNotMappedException";
        }
        catch (ArgumentException)
        {
            owner = "ArgumentException";
        }
        catch (UnauthorizedAccessException)
        {
            owner = "UnauthorizedAccessException";
        }
        catch (InvalidOperationException)
        {
            owner = "InvalidOperationException";
        }
        catch (FileNotFoundException e)
        {
            errorCode = RecordExCode.NotFound;
            Log.Warn($"文件夹不存在。{e.Message}, [error code: {errorCode}]");
            return new RecordBean()
            {
                Path = dir.FullName,
                ExceptionCode = (int)errorCode,
            };
        }
        return new RecordBean()
        {
            Path = dir.FullName,
            Plies = plies,
            CreateTime = dir.CreationTime,
            Owner = owner,
            ExceptionCode = (int)errorCode,
            DirCount = 0,
            IsFile = false,
            IsChange = false,
        };
    }
} }
