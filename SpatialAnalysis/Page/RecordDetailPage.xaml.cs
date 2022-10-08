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
    private IncidentDetail incidentDetail;

    // 加载页面数据
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        incidentTitleTextBlock.Text = incident.Title;
        incidentDescriptionTextBlock.Text = incident.Description;
        IncidentDetail detail = RecordDetailService.BuildIncidentDetail(incidentInfo, false);
        incidentDetail = detail;
        incidentPieChart.Series = detail.pieChart;
        childrenTagListBox.ItemsSource = detail.ChildrenTags;
        incidentDetailGrid.DataContext = detail;
    }

    /// <summary>
    /// 用于展示详细信息的事件
    /// </summary>
    private void IncidentDetail_OnMouseEnter(object sender, MouseEventArgs e)
    {
        incidentPopup.IsOpen = true;
    }

    /// <summary>
    /// 子标签双击事件
    /// </summary>
    private void ChildrenTagListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        TagBean tag = (TagBean) childrenTagListBox.SelectedItem;
        RefreshTagInfo(tag);
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
            RefreshTagInfo(tag);
            break;
        }
    }

    private void RefreshTagInfo(TagBean tag)
    {
        IncidentDetail detail = RecordDetailService.BuildIncidentDetail(tag, incident.Id, false);
        incidentPieChart.Series = detail.pieChart;
        childrenTagListBox.ItemsSource = detail.ChildrenTags;
        incidentDetailGrid.DataContext = detail;
    }
} }