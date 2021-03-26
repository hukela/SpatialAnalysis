using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] str = new string[] { "fas", "ddf", "ffd" };
            Console.WriteLine(str.ToString());
            Console.ReadKey(true);
        }
    }
}
/*
DirectoryInfo info = new DirectoryInfo(@"C:\");
DirectoryInfo[] directoryInfos = info.GetDirectories();
FileInfo[] fileInfos = info.GetFiles();
Console.WriteLine("目录：");
foreach (DirectoryInfo directory in directoryInfos)
    Console.WriteLine(directory.FullName);
Console.WriteLine("文件：");
foreach (FileInfo file in fileInfos)
    Console.WriteLine(file.FullName);
Console.WriteLine("所有者");
var fs = File.GetAccessControl(@"C:\");
var sid = fs.GetOwner(typeof(SecurityIdentifier));
Console.WriteLine(sid); // SID
var ntAccount = sid.Translate(typeof(NTAccount));
Console.WriteLine(ntAccount);
Console.ReadKey(true);
*/
