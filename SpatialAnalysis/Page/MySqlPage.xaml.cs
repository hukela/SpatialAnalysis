using SpatialAnalysis.Entity;
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
        //是否有本地数据库
        bool haveLocalMySql;
        //加载页面数据
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlBean bean = MySqlService.GetBean();
            DataContext = bean;
            haveLocalMySql = bean.haveLocalMySql;
        }
        //还原配置
        private void Rollback_Click(object sender, RoutedEventArgs e)
        {
            DataContext = MySqlService.GetBean();
        }
        //应用配置
        private void UseConfig_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确定修改配置", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                MySqlBean bean = (MySqlBean)DataContext;
                MySqlService.SaveConfig(bean);
                MessageBox.Show("修改成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
        //打开连接
        private void OpenConnect_Click(object sender, RoutedEventArgs e)
        {
            MySqlService.OpenConnect();
            Page_Loaded(null, null);
        }
        //关闭连接
        private void CloseConnect_Click(object sender, RoutedEventArgs e)
        {
            MySqlService.CloseConnect();
            Page_Loaded(null, null);
        }
        //打开服务
        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            MySqlService.StartServer();
            Page_Loaded(null, null);
        }
        //关闭服务
        private void StopServer_Click(object sender, RoutedEventArgs e)
        {
            MySqlService.StopServer();
            Page_Loaded(null, null);
        }
        //安装MySql
        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (haveLocalMySql)
            {
                MessageBox.Show("请不要重复安装", "错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (MessageBox.Show("是否确定安装", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                InstallAndUninstall install = new InstallAndUninstall();
                install.BeginInstall();
                Page_Loaded(null, null);
            }
        }
        //卸载MySql
        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确定删除", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                InstallAndUninstall uninstall = new InstallAndUninstall();
                uninstall.BeginUnInstall();
                Page_Loaded(null, null);
            }
        }
    }
}
