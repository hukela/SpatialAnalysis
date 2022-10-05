using System.Windows;
using System.Windows.Controls;
using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;

namespace SpatialAnalysis.MyPage
{
    /// <summary>
    /// SeeRecord.xaml 的交互逻辑
    /// </summary>
    public partial class SeeRecordPage : Page
    {
        public SeeRecordPage() { InitializeComponent(); }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitPageData();
        }

        private void InitPageData()
        {
            bool showAll = showAllBox.IsChecked ?? false;
            IncidentInfo[] incidents = SeeRecordService.getIncidentInfos(showAll);
            incidentListBox.ItemsSource = incidents;
        }

        private void ShowAll_OnChecked(object sender, RoutedEventArgs e)
        {
            InitPageData();
        }
    }
}
