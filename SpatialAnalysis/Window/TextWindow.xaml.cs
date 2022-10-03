using System.Windows;

namespace SpatialAnalysis.MyWindow
{
/// <summary>
/// TextWindow.xaml 的交互逻辑
/// </summary>
public partial class TextWindow : Window
{
    public TextWindow()
    {
        InitializeComponent();
    }
    private delegate void WriteMessage(string str);
    /// <summary>
    /// 添加信息
    /// </summary>
    /// <param name="message">信息</param>
    public void Write(string message)
    {
        DelegateMe(delegate (string str)
        {
            content.Text += str;
        }, message);
    }
    /// <summary>
    /// 添加一行信息
    /// </summary>
    /// <param name="message">信息</param>
    public void WriteLine(string message)
    {
        DelegateMe(delegate (string str)
        {
            content.Text += str + "\n";
        }, message);
    }
    /// <summary>
    /// 重写原有信息
    /// </summary>
    /// <param name="message">信息</param>
    public void WriteAll(string message)
    {
        //这里委托用作匿名函数，直接将一个函数当作一个参数传递过去。
        DelegateMe(delegate (string str)
        {
            content.Text = str;
        }, message);
    }
    /// <summary>
    /// 清空内容
    /// </summary>
    public void Clean()
    {
        if (content.Dispatcher.CheckAccess())
        {
            content.Text = "";
        }
        else
            //无参数的函数可以直接使用Invoke()方法
            content.Dispatcher.Invoke(Clean);
    }
    private void DelegateMe(WriteMessage me, string message)
    {
        //只有主线程可以操作控件
        //如果是其它线程调用了该方法，需要将其委托给主线程
        if (content.Dispatcher.CheckAccess())
            me(message);
        else
            content.Dispatcher.Invoke(me, message);
    }
} }
