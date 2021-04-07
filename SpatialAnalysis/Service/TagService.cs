using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Utils;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpatialAnalysis.Service
{
    class TagService
    {
        /// <summary>
        /// 获得ItemsSource可直接使用的数据源
        /// </summary>
        /// <param name="parentId">所需标签的父级id</param>
        public static Grid[] GetItemDataSource(uint parentId)
        {
            TagBean[] list;
            if (parentId == 0)
                list = TagMapper.GetRootTag();
            else
                list = TagMapper.GetChildTag(parentId);
            Grid[] itemList = new Grid[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                byte[] rgb = ColorUtil.GetRGB(list[i].Color);
                //用于显示颜色
                Border color = new Border()
                {
                    Width = 12,
                    Height = 12,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(8, 0, 0, 0),
                    Background = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(rgb[0], rgb[1], rgb[2])
                    },
                    BorderBrush = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(197, 193, 170)
                    },
                    BorderThickness = new Thickness(1, 1, 1, 1),
                };
                //用于显示名称
                TextBlock textBlock = new TextBlock()
                {
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(24, 0, 0, 0),
                    Text = list[i].Name,
                };
                //用于存储bean
                TextBlock textBean = new TextBlock()
                {
                    Visibility = Visibility.Collapsed,
                    Text = list[i].ToString(),
                };
                Grid grid = new Grid()
                {
                    Uid = list[i].Id.ToString()
                };
                grid.Children.Add(color);
                grid.Children.Add(textBlock);
                grid.Children.Add(textBean);
                itemList[i] = grid;
            }
            return itemList;
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
        }
    }
}
