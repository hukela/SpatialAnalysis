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

    // 是否查看全部的多选框
    private void ShowAllBox_OnClick(object sender, RoutedEventArgs e) { InitPageData(); }

    // 双击进入
    private void IncidentListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (!(incidentListBox.SelectedItem is IncidentInfo incident))
            return;
        RecordDetailPage page = new RecordDetailPage(incident);
        MainWindow main = Application.Current.MainWindow as MainWindow;
        main?.ToPage(page);
    }

    // 选中事件页面
    private void IncidentListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!(incidentListBox.SelectedItem is IncidentInfo incident))
            return;
        recordCountTextBlock.Text = "选中记录条数：" + incident.RecordCount;
        if (incident.stateEnum == IncidentStateEnum.deleted)
        {
            delBtn.IsEnabled = false;
            delBtn.Content = "记录已经被删除";
        }
        else
        {
            delBtn.IsEnabled = true;
            delBtn.Content = "删除记录";
        }
    }

    // 删除按键事件
    private void DelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (!(incidentListBox.SelectedItem is IncidentInfo incident))
            return;
        SeeRecordService.deleteRecord(incident.Id);
        InitPageData();
    }
} }
