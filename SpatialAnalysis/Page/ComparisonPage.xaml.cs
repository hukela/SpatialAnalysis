using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
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
    private TreeViewItem selectedItem;
    //绑定事件选择数据
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        int index = oldIncident.SelectedIndex;
        oldIncident.ItemsSource = ComparisonService.GetComboBoxResource();
        oldIncident.SelectedIndex = index == -1 ? 0 : index;
        index = newIncident.SelectedIndex;
        newIncident.ItemsSource = ComparisonService.GetComboBoxResource();
        newIncident.SelectedIndex = index == -1 ? 0 : index;
        //添加标签标注缓存
        TagSupport.CheckTagSort();
    }
    //实现选择框的选中事件
    private void Incident_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //在页面刚加载的时候 设置selectedIndex会触发这里的事件 会导致bean为null
        if (!(newIncident.SelectedItem is IncidentBean bean))
            return;
        uint newIncidentId = bean.Id;
        bean = oldIncident.SelectedItem as IncidentBean;
        if (bean == null)
            return;
        uint oldIncidentId = bean.Id;
        if (oldIncidentId == 0 || newIncidentId == 0)
            return;
        dirTree.ItemsSource = ComparisonService.GetRootNodes(oldIncidentId, newIncidentId);
    }

    // 节点刷新按键点击事件
    private void RefreshTree_OnClick(object sender, RoutedEventArgs e)
    {
        Incident_SelectionChanged(null, null);
    }

    //展开节点事件
    private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
    {
        TreeViewItem selectedItem = sender as TreeViewItem;
        DirNode dirNode = selectedItem.DataContext as DirNode;
        //事件为空 表示内部调用 强制刷新子节点
        if (e == null)
            dirNode.Children = new DirNode[1];
        bool needUpdate = ComparisonService.BuildNodeChildren(dirNode);
        if (needUpdate)
            selectedItem.ItemsSource = dirNode.Children;
    }
    //用于记录被选中控件的方法
    private void DirTree_Selected(object sender, RoutedEventArgs e)
    { selectedItem = e.OriginalSource as TreeViewItem; }
    //选中文件夹的事件
    private void DirTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        // 显示比对信息控件
        locationTextBlock.Visibility = Visibility.Visible;
        AddTag.Visibility = Visibility.Visible;
        compareTable.Visibility = Visibility.Visible;
        ComparisonInfo info = ComparisonService.GetInfoByNode(dirTree.SelectedItem as DirNode);
        // 放入事件信息
        IncidentBean oldBean = oldIncident.SelectedItem as IncidentBean;
        IncidentBean newBean = newIncident.SelectedItem as IncidentBean;
        info.OldTime = oldBean.CreateTimeFormat;
        info.NewTime = newBean.CreateTimeFormat;
        comparisonGrid.DataContext = info;
    }
    //为所选目录添加标签
    private void AddTag_Click(object sender, RoutedEventArgs e)
    {
        if (!(dirTree.SelectedItem is DirNode dirNode))
        {
            MessageBox.Show("未选中文件夹", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        ComparisonService.AllOrEditTag(dirNode.Path, !dirNode.IsRootTag);
        //刷新页面数据
        ComparisonService.RefreshNode(dirNode);
        TreeViewItem_Expanded(ItemsControl.ItemsControlFromItemContainer(selectedItem), null);
    }
} }
