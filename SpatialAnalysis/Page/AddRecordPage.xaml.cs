using SpatialAnalysis.Entity;
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
            IncidentBean bean = (IncidentBean)DataContext;
            if (bean.Title == null)
                MessageBox.Show("标题不得为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                if (bean.Title.Length > 20)
                {
                    MessageBox.Show("标题不得超过20个字符", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            if (bean.Explain != null)
                if (bean.Explain.Length > 500)
                {
                    MessageBox.Show("备注不得超过500个字符", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            AddRecordServive.AddIncident(bean);
        }
    }
}
