using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SpatialAnalysis.IO.Xml
{
    class XML : Base
    {
        //存储文件位置
        private static readonly string filePath = locolPath + @"\Data\Core.xml";
        /// <summary>
        /// xml中可选的key
        /// </summary>
        public enum Params
        {
            //MySql配置
            autoStartServer, autoConnent,
            //数据库相关参数
            server, port, user, password, database
        }
        /// <summary>
        /// 通过key读取value
        /// </summary>
        /// <param name="param">key</param>
        /// <returns>value</returns>
        public static dynamic Map(Params param)
        {
            string key = param.ToString();
            //跳转到Dictionary节点
            XElement dict = XElement.Load(filePath).Element("Dictionary");
            //遍历筛选key所指的节点
            IEnumerable<XElement> a = from xml in dict.Elements("add")
                                      where xml.Attribute("key").Value == key
                                      select xml;
            XElement add = a.SingleOrDefault();
            if (add == null)
                return null;
            string value = add.Attribute("value").Value;
            //自动返回对应的数据类型
            try { return bool.Parse(value); }
            catch { }
            try { return int.Parse(value); }
            catch { }
            if (value == "null")
                return null;
            else
            {
                //为了解决string型的数字
                if(value.IndexOf("str:") == 0)
                    try
                    {
                        string forInt = value.Substring(4);
                        int.Parse(forInt);
                        return forInt;
                    }
                    catch { }
                return value;
            }
        }
        /// <summary>
        /// 保存key和value。
        /// </summary>
        /// <param name="param">key</param>
        /// <param name="value">value</param>
        public static void Map(Params param, object value)
        {
            string key = param.ToString();
            //防止空异常
            if (value == null)
                value = "null";
            //为了解决string型的数字
            if (value is string)
                try
                {
                    int.Parse((string)value);
                    value = "str:" + value;
                }
                catch { }
            //根节点:Main
            XElement Main = XElement.Load(filePath);
            XElement dict = Main.Element("Dictionary");
            IEnumerable<XElement> a = from xml in dict.Elements("add")
                                      where xml.Attribute("key").Value == key
                                      select xml;
            XElement add = a.SingleOrDefault();
            //如果不存在则新建一个
            if(add == null)
            {
                XAttribute keyAttribute = new XAttribute("key", key);
                XAttribute valueAttribute = new XAttribute("value", value.ToString());
                add = new XElement("add", keyAttribute, valueAttribute);
                dict.Add(add);
            }
            else
                add.SetAttributeValue("value", value.ToString());
            //保存修改(必须是针对根节点的保存)
            Main.Save(filePath);
        }
    }
}
