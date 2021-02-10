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
using System.Windows.Shapes;

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
        public void WriteAll(string message)
        {
            //这里委托用作匿名函数，直接将一个函数当作一个参数传递过去。
            DelegateMe(delegate (string str)
            {
                content.Text = str;
            }, message);
        }
        public void WriteLine(string message)
        {
            DelegateMe(delegate (string str)
            {
                content.Text += str + "\ns";
            }, message);
        }
        public void Write(string message)
        {
            DelegateMe(delegate (string str)
            {
                content.Text += str;
            }, message);
        }
        public void Clean()
        {
            if (content.Dispatcher.CheckAccess())
                content.Text = "";
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
    }
}
