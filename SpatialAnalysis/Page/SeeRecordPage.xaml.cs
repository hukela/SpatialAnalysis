using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;

namespace SpatialAnalysis.MyPage
{
/// <summary>
/// SeeRecordPage.xaml 的交互逻辑
/// </summary>
public partial class SeeRecordPage : Page
{
    public SeeRecordPage() { InitializeComponent(); }

    private void Page_Loaded(object sender, RoutedEventArgs e) { InitPageData(); }

    // 加载页面数据
    private void InitPageData()
    {
        bool showAll = showAllBox.IsChecked ?? false;
        IncidentInfo[] incidents = SeeRecordService.getIncidentInfos(showAll);
        incidentListBox.ItemsSource = incidents;
    }

    private void ShowAllBox_OnClick(object sender, RoutedEventArgs e) { InitPageData(); }

    private void IncidentListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (!(incidentListBox.SelectedItem is IncidentInfo incident)) return;
        RecordDetailPage page = new RecordDetailPage(incident);
        MainWindow main = Application.Current.MainWindow as MainWindow;
        main?.ToPage(page);
    }

    private void IncidentListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!(incidentListBox.SelectedItem is IncidentInfo incident))
            return;
        recordCountTextBlock.Text = "事件记录条数：" + incident.RecordCount;
    }
} }
