using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Custom
{
    /// <summary>
    /// 判断是否是构造当前层的最后一个元素
    /// </summary>
    class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            //System.Console.WriteLine(ic.Items.Count + "  " + item.Header.ToString());
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
