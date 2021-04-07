using SpatialAnalysis.MyPage;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SpatialAnalysis.Dictionary
{
    //字典中的事件类
    partial class TagDictionary : ResourceDictionary
    {
        private void NewTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            ListView list = GetSender(sender) as ListView;
            byte plies;
            switch (list.Name)
            {
                case "firstNode":
                    plies = 1;
                    break;
                case "secondNode":
                    plies = 2;
                    break;
                case "thirdNode":
                    plies = 3;
                    break;
                default: throw new ApplicationException("无法识别新加标签的层数");
            }
            main.tagPage.NewTag_Click(plies);
        }
        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            main.tagPage.EditTag_Click(GetTagId(sender));
        }
        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            main.tagPage.DeleteTag_Click(GetTagId(sender));
        }
        //获取触发事件控件
        private UIElement GetSender(object sender)
        {
            DependencyObject dependency = LogicalTreeHelper.GetParent(sender as MenuItem);
            return ContextMenuService.GetPlacementTarget(dependency);
        }
        //获取触发事件的标签id
        private uint GetTagId(object sender)
        {
            ListViewItem item = GetSender(sender) as ListViewItem;
            Grid grid = item.Content as Grid;
            return uint.Parse(grid.Uid);
        }
    }
}
