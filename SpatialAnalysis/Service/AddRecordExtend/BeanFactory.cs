using SpatialAnalysis.Entity;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SpatialAnalysis.Service.AddRecordExtend
{
    internal class BeanFactory
    {
        /// <summary>
        /// 获取文件相关信息
        /// </summary>
        /// <param name="file">文件info</param>
        /// <param name="plies">层数</param>
        /// <returns></returns>
        public static RecordBean GetFileBean(FileInfo file, uint plies)
        {
            RecordBean bean = new RecordBean()
            {
                Path = file.FullName,
                Plies = plies,
                Size = file.Length,
                CerateTime = file.CreationTime,
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
                Log.Warn(string.Format("获取文件占用空间失败。{0} {1}, [error code: {2}]",
                                        file.FullName, e.Message, bean.ExceptionCode));
            }
            return bean;
        }
        /// <summary>
        /// 获取文件夹相关信息
        /// </summary>
        /// <param name="dir">文件夹info</param>
        /// <param name="plies">层数</param>
        /// <returns></returns>
        public static RecordBean GetDirBean(DirectoryInfo dir, uint plies)
        {
            string owner;
            RecordExCode errorCode;
            try
            {
                DirectorySecurity security = dir.GetAccessControl();
                IdentityReference identity = security.GetOwner(typeof(NTAccount));
                owner = identity.ToString();
                errorCode = RecordExCode.Normal;
            }
            catch (IdentityNotMappedException e)
            {
                owner = "IdentityNotMappedException";
                errorCode = RecordExCode.IdentityNotMappedException;
                Log.Warn(string.Format("获取文件夹所有者失败。{0} {1}, [error code: {2}]",
                                        dir.FullName, e.Message, errorCode));
            }
            catch (ArgumentException e)
            {
                owner = "ArgumentException";
                errorCode = RecordExCode.ArgumentException;
                Log.Warn(string.Format("获取文件夹有者失败。{0} {1}, [error code: {2}]",
                                        dir.FullName, e.Message.Replace("\r\n", ""), errorCode));
            }
            catch (UnauthorizedAccessException e)
            {
                owner = "UnauthorizedAccessException";
                errorCode = RecordExCode.ArgumentException;
                Log.Warn(string.Format("获取文件夹所有者失败。{0} {1}, [error code: {2}]",
                                        dir.FullName, e.Message, errorCode));
            }
            catch (InvalidOperationException e)
            {
                owner = "InvalidOperationException";
                errorCode = RecordExCode.InvalidOperationException;
                Log.Warn(string.Format("获取文件夹所有者失败。{0} {1}, [error code: {2}]",
                                        dir.FullName, e.Message, errorCode));
            }
            catch (FileNotFoundException e)
            {
                errorCode = RecordExCode.NotFound;
                Log.Warn(string.Format("文件夹不存在。{0}, [error code: {1}]", e.Message, errorCode));
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
                CerateTime = dir.CreationTime,
                Owner = owner,
                ExceptionCode = (int)errorCode,
                DirCount = 0,
                IsFile = false,
                IsChange = false,
            };
        }
    }
}
