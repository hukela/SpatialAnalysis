using System;

namespace SpatialAnalysis.Entity
{
    public class TagBean
    {
        public uint Id { get; set; }
        public uint ParentId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        //方便该类在字符串之间转换
        public override string ToString()
        {
            //防止字符串中也有&
            string name = Name.Replace("&", "[{and}]");
            return string.Concat(Id, '&', ParentId, "&", name, "&", Color);
        }
        public static TagBean Parse(string str)
        {
            try
            {
                string[] strs = str.Split('&');
                return new TagBean()
                {
                    Id = uint.Parse(strs[0]),
                    ParentId = uint.Parse(strs[1]),
                    Name = strs[2].Replace("[{and}]", "&"),
                    Color = strs[3],
                };
            }
            catch { throw new FormatException("输入格式不正确"); }
        }
    }
}
