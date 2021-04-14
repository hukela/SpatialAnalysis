using SpatialAnalysis.Mapper;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpatialAnalysis.Service
{
    class ComparisonService
    {
        /// <summary>
        /// 获取事件选择列表
        /// </summary>
        public static Grid[] GetComboBoxResource()
        {
            DataTable table = IncidentMapper.GetSuccessIncident();
            int count = table.Rows.Count;
            Grid[] items = new Grid[count];
            for (int i = 0; i < count; i++)
            {
                TextBlock title = new TextBlock()
                {
                    Text = table.Rows[i]["title"] as string,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(8, 0, 0, 0),
                };
                TextBlock time = new TextBlock()
                {
                    Text = ((DateTime)table.Rows[i]["create_time"]).ToString("yy-MM-dd HH:mm"),
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 8, 0),
                };
                Grid grid = new Grid()
                {
                    Uid = table.Rows[i]["title"].ToString()
                };
                grid.Children.Add(title);
                grid.Children.Add(time);
                items[i] = grid;
            }
            return items;
        }
        public static void
    }
}
