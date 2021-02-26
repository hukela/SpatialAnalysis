using SpatialAnalysis.Service;
using System.Windows;
using System.Windows.Controls;

namespace SpatialAnalysis.MyPage
{
    /// <summary>
    /// MySql.xaml 的交互逻辑
    /// </summary>
    public partial class MySqlPage : Page
    {
        public MySqlPage()
        {
            InitializeComponent();
        }
        //安装MySql
        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            InstallAndUninstall install = new InstallAndUninstall();
            install.BeginInstall();
        }
        //卸载MySql
        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {
            InstallAndUninstall remove = new InstallAndUninstall();
            remove.BeginUnInstall();
        }
    }
}
