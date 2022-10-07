using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;

namespace SpatialAnalysis.MyPage
{
/// <summary>
/// RecordDetailPage.xaml 的交互逻辑
/// </summary>
public partial class RecordDetailPage : Page
{
    public RecordDetailPage(uint incidentId)
    {
        InitializeComponent();
        incident = IncidentMapper.SelectById(incidentId);
    }

    private readonly IncidentBean incident;

    // 加载页面数据
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        incidentTitleTextBlock.Text = incident.Title;
        incidentDescriptionTextBlock.Text = incident.Description;
    }

    private void IncidentDetail_OnMouseEnter(object sender, MouseEventArgs e)
    {
        incidentPopup.IsOpen = true;
    }
} }