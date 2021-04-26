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
            ListBox list = GetSender(sender) as ListBox;
            main.tagPage.NewTag_Click(list);
        }
        //修改标签
        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            ListBoxItem item = GetSender(sender) as ListBoxItem;
            //获取bean
            TagBean bean = item.Content as TagBean;
            main.tagPage.EditTag_Click(bean);
        }
        //删除标签  
        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
            ListBoxItem item = GetSender(sender) as ListBoxItem;
            TagBean bean = item.Content as TagBean;
            //通过ListBoxItem获取ListBox
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is ListBox))
                parent = VisualTreeHelper.GetParent(parent);
            main.tagPage.DeleteTag_Click(bean.Id, parent as ListBox);
        }
        //获取触发事件控件
        private UIElement GetSender(object sender)
        {
            DependencyObject dependency = LogicalTreeHelper.GetParent(sender as MenuItem);
            return ContextMenuService.GetPlacementTarget(dependency);
        }
    }
}
