using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SpatialAnalysis.page;

namespace SpatialAnalysis
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //设置初始值
            pageFrame.Content = mainPage;
            lastButton = toMainPage;
            toMainPage.IsEnabled = false;
        }
        MainPage mainPage = new MainPage();
        //上一个跳转按键
        Button lastButton;
        //跳转按键事件
        private void ToMainPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = mainPage;
            toMainPage.IsEnabled = false;
            lastButton.IsEnabled = true;
            lastButton = toMainPage;
        }
    }
}
