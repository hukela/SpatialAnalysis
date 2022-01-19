using SpatialAnalysis.IO;
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
            pageFrame.Content = main;
            lastButton = toMainPage;
            toMainPage.IsEnabled = false;
        }
        //当窗口加载完成后执行
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsCanUse();
        }
        //显示主页页面
        private Page main;
        //初始化页面
        public MainPage mainPage = new MainPage();
        private UnavailablePage unavailablePage;
        public AddRecordPage addRecord = new AddRecordPage();
        public ComparisonPage comparisonPage = new ComparisonPage();
        public TagPage tagPage = new TagPage();
        public SeeRecordPage seeRecordPage = new SeeRecordPage();
        //若数据库不可用，则关闭相关功能
        public void IsCanUse()
        {
            bool isEnabled = SQLiteClient.IsConnected;
            toAddRecord.IsEnabled = isEnabled;
            toTagPage.IsEnabled = isEnabled;
            toIncidentPage.IsEnabled = isEnabled;
            toComparisonPage.IsEnabled = isEnabled;
            if(isEnabled)
                main = mainPage;
            else
            {
                if (unavailablePage == null)
                    unavailablePage = new UnavailablePage();
                main = unavailablePage;
            }
        }
        //上一个跳转按键
        private Button lastButton;
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
            pageFrame.Content = main;
            CloseButton(toMainPage);
        }
        private void ToAddRecord_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = addRecord;
            CloseButton(toAddRecord);
        }
        private void ToComparisonPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = comparisonPage;
            CloseButton(toComparisonPage);
        }
        private void ToTagPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = tagPage;
            CloseButton(toTagPage);
        }
        private void ToIncidentPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Content = seeRecordPage;
            CloseButton(toIncidentPage);
        }
    }
}
