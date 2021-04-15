using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;
using System.Windows.Controls;

namespace SpatialAnalysis.MyPage
{
    /// <summary>
    /// ComparisonPage.xaml 的交互逻辑
    /// </summary>
    public partial class ComparisonPage : Page
    {
        public ComparisonPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //添加标签标注缓存
            TagSupport.CheckTagSort();
            oldIncident.ItemsSource = ComparisonService.GetComboBoxResource();
            oldIncident.SelectedIndex = 0;
            newIncident.ItemsSource = ComparisonService.GetComboBoxResource();
            newIncident.SelectedIndex = 0;
        }
        //展开节点事件
        private void TreeViewItem_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            DirNode dirNode = item.DataContext as DirNode;
            ComparisonService.BuiledNodeChildren(ref dirNode);
        }
    }
}
