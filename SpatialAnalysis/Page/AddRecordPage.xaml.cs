using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SpatialAnalysis.MyPage
{
/// <summary>
/// AddRecord.xaml 的交互逻辑
/// </summary>
public partial class AddRecordPage : Page
{
    public AddRecordPage()
    {
        InitializeComponent();
    }
    //加载页面
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        IncidentBean bean = AddRecordService.GetBean();
        DataContext = bean;
        if (bean.CreateTime == DateTime.MinValue)
            return;
        TimeSpan time = DateTime.Now - bean.CreateTime;
        timeSpan.Text = "距离上一次记录：- 天".Replace(" - ", time.Days.ToString());
    }
    //添加事件
    private void Submit_Click(object sender, RoutedEventArgs e)
    {
        IncidentBean bean = (IncidentBean)DataContext;
        bean.Title = bean.Title.Trim();
        if (bean.Title == string.Empty)
        {
            MessageBox.Show("标题不得为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        else
        {
            if (bean.Title.Length > 20)
            {
                MessageBox.Show("标题不得超过25个字符", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }
        AddRecordService.AddIncident(bean);
    }
} }
