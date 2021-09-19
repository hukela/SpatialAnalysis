using System.IO;
using System.Text;

namespace SpatialAnalysis.IO
{
    internal class TextFile
    {
        /// <summary>
        /// 读取文件内所有内容。
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ReadAll(string path, Encoding encoding)
        {
            StreamReader reader = new StreamReader(path, encoding);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }
        /// <summary>
        /// 向文件中写入内容，如果文件不存在，会新建一个。
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="encoding">编码</param>
        /// <param name="content">内容</param>
        public static void WriteAll(string path, Encoding encoding, string content)
        {
            //如果文件不存在就新建一个,false代表覆盖原有数据
            StreamWriter writer = new StreamWriter(path, false, encoding);
            writer.Write(content);
            writer.Flush();
            writer.Close();
        }
    }
}
