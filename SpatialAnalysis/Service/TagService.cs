using System;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpatialAnalysis.Service
{
    internal static class TagService
    {
        /// <summary>
        /// 获得标签列表
        /// </summary>
        /// <param name="parentId">所需标签的父级id</param>
        public static TagBean[] GetTagItemSource(uint parentId)
        {
            if (parentId == 0)
                return TagMapper.GetRootTag();
            else
                return TagMapper.GetChildTag(parentId);
        }
        /// <summary>
        /// 递归删除标签及其所有子标签
        /// </summary>
        /// <param name="tagId"></param>
        public static void DeleteTag(uint tagId)
        {
            TagBean[] beanList = TagMapper.GetChildTag(tagId);
            foreach (TagBean bean in beanList)
                DeleteTag(bean.Id);
            TagMapper.DeleteOne(tagId);
            DirTagMapper.DeleteByTagId(tagId);
        }
        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="tagId">标签id</param>
        public static DirTagBean[] GetPathItemSource(uint tagId)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            DirTagBean[] beans = DirTagMapper.GetAllByTag(tagId);
            int length = beans.Length;
            DirTagBean[] items = new DirTagBean[length + 1];
            Array.Copy(beans, items, length);
            //添加新建行
            DirTagBean newBean = new DirTagBean()
            {
                TagId = tagId,
                Path = "双击添加地址"
            };
            items[length] = newBean;
            return items;
        }
    }
}
