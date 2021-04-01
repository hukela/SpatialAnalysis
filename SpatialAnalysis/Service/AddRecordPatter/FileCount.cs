using SpatialAnalysis.IO.Xml;
using SpatialAnalysis.Utils;
using System;

namespace SpatialAnalysis.Service.AddRecordPatter
{
    class FileCount : XML
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public enum FileType
        {
            file, picture, video, project, dll, txt, data
        }
        /// <summary>
        /// 从内存中获取对应类型的后缀名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>后缀名列表</returns>
        public static string[] FilePostfix(FileType type)
        {
            return (string[])InternalStorage.Get(InternalStorage.Domain.FileCount, type.ToString());
        }
        /// <summary>
        /// 从内存中获取对应类型的后缀名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>后缀名列表</returns>
        public static string[] FilePostfix(string type)
        {
            return (string[])InternalStorage.Get(InternalStorage.Domain.FileCount, type);
        }
        /// <summary>
        /// 将数据由硬盘载入内存中
        /// </summary>
        public static void LoadIntoStorage()
        {
            foreach (string key in Enum.GetNames(typeof(FileCount.FileType)))
            {
                string value = Read(key, "FileCount", "Add");
                if (value == "null")
                    value = null;
                InternalStorage.Add(InternalStorage.Domain.FileCount, key, value.Split(','));
            }
        }
        /// <summary>
        /// 添加对应类型的后缀名
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="postfix">后缀名</param>
        public static void FilePostfix(FileType type, string[] postfix)
        {
            string value = "";
            if (postfix.Length == 0)
                value = "null";
            for (int i = 0; i < postfix.Length; i++)
            {
                value += postfix[i];
                if (i != postfix.Length - 1)
                    value += ",";
            }
            Write(type.ToString(), value, "FileCount", "Add");
        }
    }
}
