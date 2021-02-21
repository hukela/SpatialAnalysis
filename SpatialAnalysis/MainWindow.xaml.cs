using SpatialAnalysis.MyPage;
using System.Windows;
using System.Windows.Controls;

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
        AddRecord addRecord = new AddRecord();
        MySqlPage mySqlPage = new MySqlPage();
        //上一个跳转按键
        Button lastButton;
        //关闭当前按键，并打开上一个按键
        private void CloseButton(Button b)
        {
            b.IsEnabled = false;
            lastButton.IsEnabled = true;
            lastButton = b;
        }
        //跳转按键事件
        private void ToMainPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = mainPage;
            CloseButton(toMainPage);
        }
        private void ToAddRecord_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = addRecord;
            CloseButton(toAddRecord);
        }
        private void ToMySql_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = mySqlPage;
            CloseButton(toMySqlPage);
        }
    }
}
