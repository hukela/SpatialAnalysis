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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpatialAnalysis.MyWindow
{
    /// <summary>
    /// ProgramWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProgramWindow : Window
    {
        public ProgramWindow()
        {
            InitializeComponent();
        }
        //窗体加载完成后执行
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //禁用关闭按键
            int handle = new WindowInteropHelper(this).Handle.ToInt32();
            CloseButton.Disable(handle);
        }
        public void Clean()
        {
            if (content.Dispatcher.CheckAccess())
                content.Text = "";
            else
                content.Dispatcher.Invoke(Clean);
        }
        //允许关闭窗口
        public void RunOver()
        {
            if (content.Dispatcher.CheckAccess())
            {
                message.Text = "线程已停止";
                closeWindow.IsEnabled = true;
            }
            else
                content.Dispatcher.Invoke(Clean);
        }
        public void WriteLine(string message)
        {
            DelegateMe(delegate (string str)
            {
                content.Text += str + "\n";
            }, message);
        }
        public void Write(string message)
        {
            DelegateMe(delegate (string str)
            {
                content.Text += str;
            }, message);
        }
        private delegate void WriteMessage(string str);
        private void DelegateMe(WriteMessage me, string message)
        {
            if (content.Dispatcher.CheckAccess())
                me(message);
            else
                content.Dispatcher.Invoke(me, message);
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
