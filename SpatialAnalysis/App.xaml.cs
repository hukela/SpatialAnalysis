using System.Threading;
using System.Windows;

namespace SpatialAnalysis
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Thread.CurrentThread.Name = "Main";
        }
    }
}
