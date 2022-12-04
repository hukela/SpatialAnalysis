using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;
using System;
using System.Windows;
using System.Windows.Controls;
using SpatialAnalysis.Mapper;

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
        DataContext = new IncidentBean(); // 先实例化对象 用于存储用户输入数据
        // 获取当前占用空间
        databaseSize.Text = "当前数据库占用空间：" + AddRecordService.GetDataSize();
        // 计算距离上一次的记录时间
        IncidentBean bean = IncidentMapper.SelectLastSuccessIncident();
        if (bean == null)
            return;
        TimeSpan time = DateTime.Now - bean.CreateTime;
        timeSpan.Text = "距离上一次记录：" + time.Days + "天";
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
        if (bean.Title.Length > 25)
        {
            MessageBox.Show("标题不得超过25个字符", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        AddRecordService.AddIncident(bean);
    }
} }
