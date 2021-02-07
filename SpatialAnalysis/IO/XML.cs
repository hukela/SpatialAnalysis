using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SpatialAnalysis.IO.Xml
{
    class XML: Base
    {
        //存储文件位置
        private static readonly string filePath = locolPath + @"\Data\Core.xml";
        //Map相关参数
        public enum Params
        {
            test
        }
        public static dynamic Map(Params param)
        {
            string key = param.ToString();
            //跳转到map节点
            XElement map = XElement.Load(filePath).Element("Map");
            //遍历筛选key所指的节点
            IEnumerable<XElement> puts = from xml in map.Elements("put")
                                         where xml.Attribute("key").Value == key
                                         select xml;
            XElement put = puts.SingleOrDefault();
            if (put == null)
                throw new ApplicationException("xml: 读取了不存在的key");
            string value = put.Attribute("value").Value;
            //自动返回对应的数据类型
            try { return bool.Parse(value); }
            catch { }
            try { return int.Parse(value); }
            catch { }
            return value;
        }
        /// <summary>
        /// 保存key和value。
        /// </summary>
        /// <param name="param">key</param>
        /// <param name="value">value</param>
        public static void Map(Params param, object value)
        {
            string key = param.ToString();
            //根节点:Main
            XElement Main = XElement.Load(filePath);
            XElement map = Main.Element("Map");
            IEnumerable<XElement> puts = from xml in map.Elements("put")
                                         where xml.Attribute("key").Value == key
                                         select xml;
            XElement put = puts.SingleOrDefault();
            //如果不存在则新建一个
            if(put == null)
            {
                XAttribute keyAttribute = new XAttribute("key", key);
                XAttribute valueAttribute = new XAttribute("value", value.ToString());
                put = new XElement("put", keyAttribute, valueAttribute);
                map.Add(put);
            }
            else
                put.SetAttributeValue("value", value.ToString());
            //保存修改(必须是针对根节点的保存)
            Main.Save(filePath);
        }
    }
}
