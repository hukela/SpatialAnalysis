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
            DataTable table;
            if (parentId == 0)
                table = TagMapper.GetRootTag();
            else
                table = TagMapper.GetChildTag(parentId);
            Grid[] list = new Grid[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                byte[] rgb = ColorUtil.GetRGB((string)table.Rows[i]["color"]);
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
                TextBlock textBlock = new TextBlock()
                {
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(24, 0, 0, 0),
                    Text = table.Rows[i]["name"] as string,
                };
                Grid grid = new Grid()
                {
                    Uid = table.Rows[i]["id"].ToString()
                };
                grid.Children.Add(color);
                grid.Children.Add(textBlock);
                list[i] = grid;
            }
            return list;
        }
    }
}
