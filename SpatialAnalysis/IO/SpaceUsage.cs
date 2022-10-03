using SpatialAnalysis.Utils;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SpatialAnalysis.IO
{
internal static class SpaceUsage
{
    //获取文件占用空间
    public static ulong Get(string fullName)
    {
        string rootPath = fullName.Substring(0, 3);
        uint tall = 0;
        uint low = GetCompressedFileSize(fullName, ref tall);
        if (low == uint.MaxValue)
            throw new Win32Exception(Marshal.GetLastWin32Error());
        ulong result = ((ulong)tall << 32) + low;
        uint size = clusterCache.Get(rootPath);
        if (result % size != 0)
        {
            decimal res = result / size;
            ulong clu = (ulong)Convert.ToInt32(Math.Ceiling(res)) + 1;
            result = size * clu;
        }
        return result;
    }
    //本地缓存
    private static readonly LocalCache<uint, string> clusterCache = new LocalCache<uint, string> (30, GetClusterSize);
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
} }
