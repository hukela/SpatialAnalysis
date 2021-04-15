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
            //更新标签标注缓存
            TagSupport.CheckTagSort();
            oldIncident.ItemsSource = ComparisonService.GetComboBoxResource();
            newIncident.ItemsSource = ComparisonService.GetComboBoxResource();
        }
        //进行分析
        private void RunAnalyse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Grid grid = oldIncident.SelectedItem as Grid;
            uint original = uint.Parse(grid.Uid);
            grid = newIncident.SelectedItem as Grid;
            uint subsequent = uint.Parse(grid.Uid);
        }
    }
}
