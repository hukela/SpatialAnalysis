using SpatialAnalysis.IO;
using SpatialAnalysis.MyPage;
using System.Windows;
using System.Windows.Controls;
using SpatialAnalysis.IO.Log;

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
        lastButton = toAddRecord;
        toAddRecord.IsEnabled = false;
    }

    //当窗口加载完成后执行
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //若数据库不可用，则关闭应用
        if (SQLiteClient.check())
            pageFrame.Content = addRecord;
        else
        {
            MessageBox.Show("sqlite数据库异常", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error("sqlite数据库异常");
            Application.Current.Shutdown();
        }
    }

    //初始化页面
    private readonly AddRecordPage addRecord = new AddRecordPage();
    private readonly ComparisonPage comparisonPage = new ComparisonPage();
    public readonly TagPage tagPage = new TagPage();
    public readonly SeeRecordPage seeRecordPage = new SeeRecordPage();

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
    private void ToRecordPage_Click(object sender, RoutedEventArgs e)
    {
        pageFrame.Content = seeRecordPage;
        CloseButton(toRecordPage);
    }

    /// <summary>
    /// 跳转到指定页面
    /// </summary>
    /// <param name="page">页面</param>
    public void ToPage(Page page)
    {
        pageFrame.Content = page;
    }
} }
