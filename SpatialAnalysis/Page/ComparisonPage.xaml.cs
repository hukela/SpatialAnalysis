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
            incident1.ItemsSource = ComparisonService.GetComboBoxResource();
            incident2.ItemsSource = ComparisonService.GetComboBoxResource();
        }
    }
}
