using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Xml;
using SpatialAnalysis.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpatialAnalysis.MyWindow
{
/// <summary>
/// TagWindow.xaml 的交互逻辑
/// </summary>
public partial class TagWindow : Window
{
    public TagWindow(string title, TagBean bean = null)
    {
        InitializeComponent();
        Title = title;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        if (bean == null)
            DataContext = new TagBean() { Name = string.Empty, Color = string.Empty };
        else
            DataContext = bean;
    }
    //加载页面数据
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        string[] colors = XML.Map(XML.Params.tagColor).Split(',');
        string beanColor = (DataContext as TagBean).Color;
        int index = -1;
        bool isSelected = beanColor != string.Empty;
        List<Border> items = new List<Border>();
        //添加颜色块
        for (int i = 0; i < colors.Length; i++)
        {
            byte[] rgb = ColorUtil.GetRGB(colors[i]);
            items.Add(new Border()
            {
                Width = 50,
                Height = 16,
                Uid = colors[i],
                Background = new SolidColorBrush()
                {
                    Color = Color.FromRgb(rgb[0], rgb[1], rgb[2])
                },
            });
            if (isSelected)
            {
                if (colors[i] == beanColor)
                    index = i;
            }
        }
        if (isSelected)
        {
            if (index == -1)
            {
                byte[] rgb = ColorUtil.GetRGB(beanColor);
                items.Add(new Border()
                {
                    Width = 50,
                    Height = 16,
                    Uid = beanColor,
                    Background = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(rgb[0], rgb[1], rgb[2])
                    },
                });
                selectColor.ItemsSource = items;
                selectColor.SelectedIndex = colors.Length;
            }
            else
            {
                selectColor.ItemsSource = items;
                selectColor.SelectedIndex = index;
            }
        }
        else
            selectColor.ItemsSource = items;
    }
    //选中颜色
    private void SelectColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string value = (selectColor.SelectedItem as UIElement).Uid;
        color.Text = value;
        (DataContext as TagBean).Color = value;
    }
    private void Yes_Click(object sender, RoutedEventArgs e)
    {
        TagBean bean = DataContext as TagBean;
        bean.Name = bean.Name.Trim();
        bean.Color = bean.Color.Trim();
        if (bean.Name == string.Empty)
        {
            MessageBox.Show("名称不得为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        else if (bean.Name.Length > 30)
        {
            MessageBox.Show("名称过长", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        if (bean.Color == string.Empty)
            bean.Color = "#FFFFFF";
        bool isColorTrue = bean.Color.Length == 7;
        isColorTrue = isColorTrue && bean.Color[0] == '#';
        if(isColorTrue)
        {
            try
            {
                ColorUtil.GetRGB(bean.Color);
                DialogResult = true;
                Close(); return;
            }
            catch
            { /* ignored */ }
        }
        MessageBox.Show("颜色代码格式错误", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
    private void No_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
} }
