﻿using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;
using System.Windows;
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
        //被选中树节点
        TreeViewItem selectedItem;
        //绑定事件选择数据
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
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
            //在页面刚加载的时候，设置selectedIndex会触发这里的事件,回导致bean为null
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
        //展开节点事件
        private void TreeViewItem_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            selectedItem = sender as TreeViewItem;
            DirNode dirNode = selectedItem.DataContext as DirNode;
            ComparisonService.BuiledNodeChildren(ref dirNode);
        }
        //选中文件夹的事件
        private void DirTree_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            ComparisonInfo info = ComparisonService.GetInfoByNode(dirTree.SelectedItem as DirNode);
            //将页面上已经存在的数据放入，减少与数据库的交互和重复的计算
            info.OldTime = (oldIncident.SelectedItem as IncidentBean).TimeFormat;
            info.NewTime = (newIncident.SelectedItem as IncidentBean).TimeFormat;
            comparisonGrid.DataContext = info;
        }
        //为所选目录添加标签
        private void AddTag_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(dirTree.SelectedItem is DirNode dirNode))
            {
                MessageBox.Show("未选中文件夹", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            ComparisonService.AllOrEditTag(dirNode.Path, dirNode.Tag.Id == 0);
            //刷新页面数据
            ComparisonService.RefreshNode(ref dirNode);
            TreeViewItem_Expanded(selectedItem, null);
        }
    }
}
