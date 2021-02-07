using SpatialAnalysis.IO.Xml;
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
            //----
            XML.Map(XML.Params.test, 15);
            System.Console.WriteLine("test = " + XML.Map(XML.Params.test));
            XML.Map(XML.Params.test, false);
            System.Console.WriteLine("test = " + XML.Map(XML.Params.test));
            XML.Map(XML.Params.test, "111");
            System.Console.WriteLine("test = " + XML.Map(XML.Params.test));
        }
        MainPage mainPage = new MainPage();
        AddRecord addRecord = new AddRecord();
        //上一个跳转按键
        Button lastButton;
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
        //关闭当前按键，并打开上一个按键
        private void CloseButton<B>(B b) where B: Button
        {
            b.IsEnabled = false;
            lastButton.IsEnabled = true;
            lastButton = b;
        }
    }
}
