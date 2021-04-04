using SpatialAnalysis.IO.Xml;
using SpatialAnalysis.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Service.AddRecordPatter
{
    class FileCount : XML
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public enum FileType
        {
            file, picture, video, project, zip, dll, txt, data
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
        /// 将数据由硬盘载入内存中,减少之后频繁读取的IO操作
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
        /// <summary>
        /// 用于记录未知后缀名的
        /// </summary>
        /// <param name="postfix">后缀名</param>
        public static void AddOtherPostfix(string postfix)
        {
            if (otherPostfix.ContainsKey(postfix))
                otherPostfix[postfix]++;
            else
                otherPostfix[postfix] = 1;
        }
        private static Dictionary<string, int> otherPostfix = new Dictionary<string, int>();
        public static string GetOtherPostfix()
        {
            //以字典Value值逆序排序
            IOrderedEnumerable<KeyValuePair<string, int>> sortResult =
                from pair in otherPostfix orderby pair.Value descending select pair;
            StringBuilder builder = new StringBuilder();
            builder.Append("{ ");
            foreach (KeyValuePair<string, int> pair in sortResult)
            {
                builder.Append(pair.Key);
                builder.Append(": ");
                builder.Append(pair.Value);
                if (pair.Value > 10)
                    builder.Append(',');
                else break;
            }
            builder.Append(" }");
            //将遍历设置为null，来释放资源
            otherPostfix = null;
            return builder.ToString();
        }
    }
}
