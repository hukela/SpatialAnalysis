using SpatialAnalysis.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpatialAnalysis.Dictionary
{
    //字典中的事件类
    partial class TagDictionary : ResourceDictionary
    {
        //新建标签
        private void NewTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            ListView list = GetSender(sender) as ListView;
            main.tagPage.NewTag_Click(list);
        }
        //修改标签
        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            ListViewItem item = GetSender(sender) as ListViewItem;
            Grid grid = item.Content as Grid;
            //遍历gird中的控件来获取bean
            TagBean bean = new TagBean();
            foreach (UIElement element in grid.Children)
            {
                if (element.Visibility == Visibility.Collapsed)
                    bean = TagBean.Parse((element as TextBlock).Text);
            }
            main.tagPage.EditTag_Click(bean);
        }
        //删除标签  
        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            ListViewItem item = GetSender(sender) as ListViewItem;
            Grid grid = item.Content as Grid;
            //通过ListViewItem获取ListView
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is ListView))
                parent = VisualTreeHelper.GetParent(parent);
            main.tagPage.DeleteTag_Click(uint.Parse(grid.Uid), parent as ListView);
        }
        //获取触发事件控件
        private UIElement GetSender(object sender)
        {
            DependencyObject dependency = LogicalTreeHelper.GetParent(sender as MenuItem);
            return ContextMenuService.GetPlacementTarget(dependency);
        }
    }
}
