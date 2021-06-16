using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Log;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SpatialAnalysis.Service.AddRecordExtend
{
    class BeanFactory
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
            { bean.SpaceUsage = GetSpaceUsage(file.FullName); }
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
                DirCount = 1,
                IsFile = false,
                IsChange = false,
            };
        }
        //获取文件占用空间
        private static ulong GetSpaceUsage(string fullName)
        {
            string rootPath = fullName.Substring(0, 3);
            uint tall = 0;
            uint low = GetCompressedFileSize(fullName, ref tall);
            if (low == uint.MaxValue)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            ulong result = ((ulong)tall << 32) + low;
            uint size = GetClusterSize(rootPath);
            if (result % size != 0)
            {
                decimal res = result / size;
                ulong clu = (ulong)Convert.ToInt32(Math.Ceiling(res)) + 1;
                result = size * clu;
            }
            return result;
        }
        //获取每簇的字节数
        private static uint GetClusterSize(string rootPath)
        {
            //提前声明各项参数
            uint sectorsPerCluster = 0, bytesPerSector = 0, numberOfFreeClusters = 0, totalNumberOfClusters = 0;
            GetDiskFreeSpace(rootPath, ref sectorsPerCluster, ref bytesPerSector, ref numberOfFreeClusters, ref totalNumberOfClusters);
            return bytesPerSector * sectorsPerCluster;
        }
        //用于获取文件实际大小的api
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern uint GetCompressedFileSize(string fileName, ref uint fileSizeHigh);
        //用于获取盘信息的api
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPTStr)]string rootPathName, ref uint sectorsPerCluster, ref uint bytesPerSector, ref uint numberOfFreeClusters, ref uint totalNumbeOfClusters);
    }
}
