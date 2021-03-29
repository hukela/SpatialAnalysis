using System;
using SpatialAnalysis.IO.Xml;

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
        /// 获取对应类型的后缀名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>后缀名列表</returns>
        public static string[] FilePostfix(FileType type)
        {
            string value = Read(type.ToString(), "FileCount", "Add");
            if (value == "null")
                return null;
            else
                return value.Split(',');
        }
        /// <summary>
        /// 获取对应类型的后缀名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>后缀名列表</returns>
        internal static string[] FilePostfix(string type)
        {
            string value = Read(type, "FileCount", "Add");
            if (value == "null")
                return null;
            else
                return value.Split(',');
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
