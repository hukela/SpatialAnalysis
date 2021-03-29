using SpatialAnalysis.Entity;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SpatialAnalysis.Service.AddRecordPatter
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
            FileSecurity security = file.GetAccessControl();
            IdentityReference owner = security.GetOwner(typeof(NTAccount));
            RecordBean bean = new RecordBean()
            {
                FullName = file.FullName,
                Type = false,
                Plies = plies,
                Size = file.Length,
                SpaceUsage = GetSpaceUsage(file.FullName),
                CerateTime = file.CreationTime,
                ModifyTime = file.LastWriteTime,
                VisitTime = file.LastAccessTime,
                Owner = owner.ToString(),
                IsChange = true
            };
            //这里后面添加上判断文件是否变化的功能
            //初始化时所有未赋值的不能为null的变量会自动赋值为0，这里不需要再去赋值。
            //遍历枚举类，设置文件类别
            string extension = file.Extension;
            if (extension == "")
            {
                bean.NullCount = 1;
                return bean;
            }
            foreach (string type in Enum.GetNames(typeof(FileCount.FileType)))
            {
                string[] postfixes = FileCount.FilePostfix(type);
                foreach (string postfix in postfixes)
                {
                    if (extension == postfix)
                    {
                        string countName = char.ToUpper(type[0]) + type.Substring(1) + "Count";
                        //设置对应属性名的数值
                        typeof(RecordBean).GetProperty(countName).SetValue(bean, 1);
                        return bean;
                    }
                }
            }
            bean.OtherCount = 1;
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
            DirectorySecurity security = dir.GetAccessControl();
            IdentityReference owner = security.GetOwner(typeof(NTAccount));
            return new RecordBean()
            {
                FullName = dir.FullName,
                Type = true,
                Plies = plies,
                CerateTime = dir.CreationTime,
                Owner = owner.ToString()
            };
        }
        //获取文件占用空间
        private static ulong GetSpaceUsage(string fullName)
        {
            string rootPath = fullName.Substring(0, 3);
            uint tall = 0;
            uint low = GetCompressedFileSize(fullName, ref tall);
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
        [DllImport("Kernel32.dll")]
        private static extern uint GetCompressedFileSize(string fileName, ref uint fileSizeHigh);
        //用于获取盘信息的api
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPTStr)]string rootPathName, ref uint sectorsPerCluster, ref uint bytesPerSector, ref uint numberOfFreeClusters, ref uint totalNumbeOfClusters);
    }
}
