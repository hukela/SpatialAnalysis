using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Utils;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpatialAnalysis.Service
{
    class TagService
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
        public static Grid[] GetPathItemSource(uint tagId)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            DirTagBean[] beans = DirTagMapper.GetAllByTag(tagId);
            int count = beans.Length;
            Grid[] items = new Grid[count + 1];
            for (int i = 0; i < count; i++)
            {
                TextBlock showPath = new TextBlock()
                {
                    FontSize = 16,
                    Text = beans[i].Path,
                };
                showPath.MouseLeftButtonDown += main.tagPage.Path_Click;
                showPath.MouseLeftButtonDown += main.tagPage.EditedItem_MouseDown;
                TextBox editPath = new TextBox()
                {
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 40, 0),
                    Text = beans[i].Path,
                    Visibility = Visibility.Collapsed,
                };
                Button deleteButton = new Button()
                {
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = "删除",
                    Visibility = Visibility.Collapsed,
                    Background = new SolidColorBrush()
                    {
                        Color = Color.FromArgb(0, 0, 0, 0)
                    },
                };
                deleteButton.Click += main.tagPage.DeletePath_Click;
                Grid grid = new Grid()
                {
                    Uid = beans[i].Id.ToString(),
                };
                grid.Children.Add(showPath);
                grid.Children.Add(editPath);
                grid.Children.Add(deleteButton);
                items[i] = grid;
            }
            //添加新建行
            TextBlock showNewPath = new TextBlock()
            {
                FontSize = 16,
                Text = "双击添加新地址",
            };
            showNewPath.MouseLeftButtonDown += main.tagPage.Path_Click;
            showNewPath.MouseLeftButtonDown += main.tagPage.EditedItem_MouseDown;
            TextBox editNewPath = new TextBox()
            {
                FontSize = 16,
                Visibility = Visibility.Collapsed,
            };
            Grid newPathGrid = new Grid()
            {
                Uid = "newPath",
            };
            newPathGrid.Children.Add(showNewPath);
            newPathGrid.Children.Add(editNewPath);
            items[count] = newPathGrid;
            return items;
        }
    }
}
