using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace SpatialAnalysis.IO
{
    /// <summary>
    /// 用于获取NTFS下的MFT文件内容的底层代码
    /// </summary>
    class MftReader
    {
        #region 相关API以及常量

        //通用读取权限
        private const uint GENERIC_READ = 0x80000000;
        //允许在打开时其它进程去访问或操作该文件
        private const uint dwShareMode = 0x00000007;
        //仅打开已存在文件
        private const uint OpenExisting = 3;
        //表示访问的目的是从头到尾都是顺序的。系统可以以此为提示来优化文件缓存。
        private const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;

        //用于创建或打开文件的api
        //https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName,
            uint dwDesiredAccess, uint dwShareMode,
            ref object lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        //IO控制代码，枚举两个指定边界之间的更新序列号（USN）数据以获得主文件表（MFT）记录。
        private const uint FSCTL_ENUM_USN_DATA = 0x000900b3;

        //用于操作硬件设备的通用api
        //https://docs.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeviceIoControl(IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer, uint nInBufferSize,
            IntPtr lpOutBuffer, uint nOutBufferSize,
            out uint lpBytesReturned, ref object lpOverlapped);

        //用于关闭已打开文件的api
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        //用于遍历MTF结构体
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MFT_ENUM_DATA
        {
            //枚举位置索引
            public ulong StartFileReferenceNumber;
            //日志USN值范围的下边界
            public long LowUsn;
            //日志USN值范围的上边界
            public long HighUsn;
        }

        //将相应内存空间填充为0的api
        [DllImport("kernel32.dll")]
        private static extern void ZeroMemory(IntPtr ptr, int size);

        #endregion 相关API以及常量

        //读取所有的MFT表
        public static void ReadAll(string rootPath)
        {
            //打开并获取卷指针
            IntPtr root = GetRootPointer(rootPath);
            //建立缓冲区
            BuildInput(out IntPtr inputBuffer, out uint inputBufferSize);
            BuildOutput(out IntPtr outputBuffer, out uint outputBufferSize);
            //循环读取MFT列表
            bool result;
            do
            {
                object lpOverlapped = null;
                result = DeviceIoControl(root,
                    FSCTL_ENUM_USN_DATA,
                    inputBuffer, inputBufferSize,
                    outputBuffer, outputBufferSize,
                    out uint outputDataSize, ref lpOverlapped);
                //这里添加读取输出缓冲区的代码
                //Windows api只能一个文件一个文件的获取，导致效率特别的差劲
                //所见这里放弃该方案。
                //微软啊，给个批量获取能死吗？亏我翻了整整两天的开发文档啊
            } while (result);
            Console.WriteLine(Marshal.GetLastWin32Error());
            //释放相关资源
            CloseHandle(root);
            Marshal.FreeHGlobal(inputBuffer);
            Marshal.FreeHGlobal(outputBuffer);
        }

        //打开文件并获取其指针
        private static IntPtr GetRootPointer(string rootPath)
        {
            object lpSecurityAttributes = null;
            IntPtr root = CreateFile(rootPath,
                GENERIC_READ, dwShareMode,
                ref lpSecurityAttributes, OpenExisting,
                FILE_FLAG_SEQUENTIAL_SCAN, IntPtr.Zero);
            Console.WriteLine(Marshal.GetLastWin32Error());
            if (root.ToInt32() == -1)
                throw new IOException("无法打开卷:" + rootPath, new Win32Exception(Marshal.GetLastWin32Error()));
            return root;
        }

        //建立输入缓冲区
        private static void BuildInput(out IntPtr inputBuffer, out uint inputBufferSize)
        {
            MFT_ENUM_DATA inputData = new MFT_ENUM_DATA()
            {
                StartFileReferenceNumber = 0,
                LowUsn = 0,
                HighUsn = long.MaxValue,
            };
            //获取结构体大小
            int inputSize = Marshal.SizeOf(inputData);
            inputBufferSize = Convert.ToUInt32(inputSize);
            //获取非托管内存空间，并返回起始地址
            inputBuffer = Marshal.AllocHGlobal(inputSize);
            //清理内存空间
            ZeroMemory(inputBuffer, inputSize);
            //将数据放入内存中
            Marshal.StructureToPtr(inputData, inputBuffer, true);
        }
        //建立输出缓冲区
        private static void BuildOutput(out IntPtr outputBuffer, out uint outputBufferSize)
        {
            //输出缓冲区大小(1MB)
            const int outputSize = 1024 * 1024;
            outputBufferSize = Convert.ToUInt32(outputSize);
            outputBuffer = Marshal.AllocHGlobal(outputSize);
            ZeroMemory(outputBuffer, outputSize);
        }
        
        //尝试直接打开MFT文件
        public static void openMft()
        {
            GetRootPointer(@"C:\$MFT");
            //抱歉，权限不足，拒绝访问
        }
    }
}
