using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SpatialAnalysis.IO.Xml
{
internal static class XML
{
    //存储文件位置
    private static readonly string filePath = IoBase.localPath + @"\Data\Core.xml";
    /// <summary>
    /// xml中可选的key
    /// </summary>
    public enum Params
    {
        //标签颜色
        tagColor
    }
    /// <summary>
    /// 通过key读取value
    /// </summary>
    /// <param name="param">key</param>
    /// <returns>value</returns>
    public static dynamic Map(Params param)
    {
        string value = Read(param.ToString(), "Dictionary", "Add");
        if (value == null)
        {
            if (param.ToString().Substring(0, 2) == "is")
                return false;
            else
                return null;
        }
        //自动返回对应的数据类型
        try { return bool.Parse(value); }
        catch { /*ignored*/ }
        try { return int.Parse(value); }
        catch { /*ignored*/ }
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
        Write(key, value.ToString(), "Dictionary", "Add");
    }
    //读取
    private static string Read(string key, string firstNode, string secondNode)
    {
        //跳转到firstNode节点
        XElement dict = XElement.Load(filePath).Element(firstNode);
        //遍历筛选key所指的secondNode节点
        IEnumerable<XElement> a = from xml in dict.Elements(secondNode)
                                  where xml.Attribute("key").Value == key
                                  select xml;
        XElement add = a.SingleOrDefault();
        if (add == null)
            return null;
        return add.Attribute("value").Value;
    }
    //写入
    private static void Write(string key, string value, string firstNode, string secondNode)
    {
        //根节点:Main
        XElement main = XElement.Load(filePath);
        XElement dict = main.Element(firstNode);
        IEnumerable<XElement> a = from xml in dict.Elements(secondNode)
                                  where xml.Attribute("key").Value == key
                                  select xml;
        XElement add = a.SingleOrDefault();
        //如果不存在则新建一个
        if (add == null)
        {
            XAttribute keyAttribute = new XAttribute("key", key);
            XAttribute valueAttribute = new XAttribute("value", value.ToString());
            add = new XElement(secondNode, keyAttribute, valueAttribute);
            dict.Add(add);
        }
        else
            add.SetAttributeValue("value", value.ToString());
        //保存修改(必须是针对根节点的保存)
        main.Save(filePath);
    }
} }
