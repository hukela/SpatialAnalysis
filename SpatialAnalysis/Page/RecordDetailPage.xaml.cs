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

    // 加载页面数据
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        incidentTitleTextBlock.Text = incident.Title;
        incidentDescriptionTextBlock.Text = incident.Description;
        IncidentDetail detail = RecordDetailService.BuildIncidentDetail(incidentInfo, false);
        incidentPieChart.Series = detail.pieChart;
    }

    private void IncidentDetail_OnMouseEnter(object sender, MouseEventArgs e)
    {
        incidentPopup.IsOpen = true;
    }

    private void IncidentPieChart_OnChartPointPointerDown(IChartView chart, ChartPoint point)
    {
        throw new System.NotImplementedException();
    }
} }