using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.Service;

namespace SpatialAnalysis.MyPage
{
/// <summary>
/// RecordDetailPage.xaml 的交互逻辑
/// </summary>
public partial class RecordDetailPage : Page
{
    internal RecordDetailPage(IncidentInfo incidentInfo)
    {
        InitializeComponent();
        incident = IncidentMapper.SelectById(incidentInfo.Id);
        this.incidentInfo = incidentInfo;
    }

    private readonly IncidentBean incident;
    private readonly IncidentInfo incidentInfo;
    private bool isSpaceUsage;
    private IncidentDetail incidentDetail;

    // 加载页面数据
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        incidentTitleTextBlock.Text = incident.Title;
        incidentDescriptionTextBlock.Text = incident.Description;
        UpdatePage(PageUpdateType.refresh, null);
    }

    // 返回事件列表按键
    private void BackBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow main = Application.Current.MainWindow as MainWindow;
        main.ToPage(main.seeRecordPage);
    }

    private enum PageUpdateType
    {
        refresh,
        toChildrenTag,
        toParentTag,
    }

    /// <summary>
    /// 通用页面更新方法
    /// </summary>
    /// <param name="type">更新方式</param>
    /// <param name="tag">若进入子一级标签则 则需要传递该参数</param>
    private void UpdatePage(PageUpdateType type, TagBean tag)
    {
        switch (type)
        {
            case PageUpdateType.refresh:
                tag = incidentDetail == null ? new TagBean() : incidentDetail.Tag;
                if (tag.Id == 0)
                    incidentDetail = RecordDetailService.BuildIncidentDetail(incidentInfo, isSpaceUsage);
                else
                    incidentDetail = RecordDetailService.BuildIncidentDetail(tag, incidentInfo.Id, isSpaceUsage);
                break;
            case PageUpdateType.toParentTag:
                TagBean parentTag = incidentDetail.Tag;
                if (parentTag.ParentId == 0)
                {
                    returnBtn.Visibility = Visibility.Hidden;
                    incidentDetail = RecordDetailService.BuildIncidentDetail(incidentInfo, isSpaceUsage);
                }
                else
                {
                    parentTag = TagMapper.SelectById(parentTag.ParentId);
                    incidentDetail = RecordDetailService.BuildIncidentDetail(parentTag, incidentInfo.Id, isSpaceUsage);
                }
                break;
            case PageUpdateType.toChildrenTag:
                returnBtn.Visibility = Visibility.Visible;
                incidentDetail = RecordDetailService.BuildIncidentDetail(tag, incidentInfo.Id, isSpaceUsage);
                break;
        }
        incidentPieChart.Series = incidentDetail.pieChart;
        childrenTagListBox.ItemsSource = incidentDetail.ChildrenTags;
        incidentDetailGrid.DataContext = incidentDetail;
        pathListBox.ItemsSource = incidentDetail.paths;
    }

    /// <summary>
    /// 用于展示详细信息的事件
    /// </summary>
    private void IncidentDetail_OnMouseEnter(object sender, MouseEventArgs e)
    {
        incidentPopup.IsOpen = true;
    }

    /// <summary>
    /// 单选框大小点击事件
    /// </summary>
    private void RadioButtonSize_OnClick(object sender, RoutedEventArgs e)
    {
        if (!isSpaceUsage) return;
        isSpaceUsage = false;
        UpdatePage(PageUpdateType.refresh, null);
    }

    /// <summary>
    /// 单选框占用空间点击事件
    /// </summary>
    private void RadioButtonSpaceUsage_OnClick(object sender, RoutedEventArgs e)
    {
        if (isSpaceUsage) return;
        isSpaceUsage = true;
        UpdatePage(PageUpdateType.refresh, null);
    }

    /// <summary>
    /// 子标签双击事件
    /// </summary>
    private void ChildrenTagListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        TagBean tag = (TagBean) childrenTagListBox.SelectedItem;
        UpdatePage(PageUpdateType.toChildrenTag, tag);
    }

    /// <summary>
    /// 饼图点击事件
    /// </summary>
    private void IncidentPieChart_OnChartPointPointerDown(IChartView chart, ChartPoint point)
    {
        uint tagId = (uint)point.TertiaryValue;
        foreach (TagBean tag in incidentDetail.ChildrenTags)
        {
            if (tag.Id != tagId)
                continue;
            UpdatePage(PageUpdateType.toChildrenTag, tag);
            break;
        }
    }

    /// <summary>
    /// 返回上一级按键
    /// </summary>
    private void ReturnBtn_OnClick(object sender, RoutedEventArgs e)
    {
        UpdatePage(PageUpdateType.toParentTag, null);
    }

    /// <summary>
    /// 路径双击事件
    /// </summary>
    private void PathListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        string path = (string) pathListBox.SelectedItem;
        // 判断路径是否存在
        if (!Directory.Exists(path))
        {
            MessageBox.Show("路径不存在", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        // 在资源管理器中打开
        System.Diagnostics.Process.Start("explorer.exe", path);
    }
} }