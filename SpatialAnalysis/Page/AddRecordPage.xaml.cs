using SpatialAnalysis.Service;
using System.Windows;
using System.Windows.Controls;

namespace SpatialAnalysis.MyPage
{
    /// <summary>
    /// AddRecord.xaml 的交互逻辑
    /// </summary>
    public partial class AddRecordPage : Page
    {
        public AddRecordPage()
        {
            InitializeComponent();
        }
        //加载页面
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = AddRecordServive.GetBean();
        }
        //添加事件
        private void Submit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
