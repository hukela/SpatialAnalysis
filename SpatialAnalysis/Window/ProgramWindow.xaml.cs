using System.Windows;
using System.Windows.Interop;

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
        //被冻结的信息
        private string freezedMessage = "";
        /// <summary>
        /// 清空内容
        /// </summary>
        public void Clean(bool cleanFreeze)
        {
            if (content.Dispatcher.CheckAccess())
            {
                if (cleanFreeze)
                {
                    content.Text = "";
                    freezedMessage = "";
                }
                else
                    content.Text = freezedMessage;
            }
            else
            {
                CleanDel me = new CleanDel(Clean);
                content.Dispatcher.Invoke(me, cleanFreeze);
            }
        }
        private delegate void CleanDel(bool cleanFreeze);
        /// <summary>
        /// 冻结原有信息，让WriteAll()无法重写这些信息
        /// </summary>
        public void Freeze()
        {
            if (content.Dispatcher.CheckAccess())
                freezedMessage = content.Text;
            else
                content.Dispatcher.Invoke(Freeze);
        }
        /// <summary>
        /// 告知线程结束，并允许关闭窗口
        /// </summary>
        public void RunOver()
        {
            if (content.Dispatcher.CheckAccess())
            {
                message.Text = "线程已停止";
                closeWindow.IsEnabled = true;
            }
            else
                content.Dispatcher.Invoke(RunOver);
        }
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
            DelegateMe(delegate (string str)
            {
                content.Text = freezedMessage + str;
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
