using System;
using SpatialAnalysis.Entity;
using SpatialAnalysis.IO.Xml;
using SpatialAnalysis.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SpatialAnalysis.IO.Log;

namespace SpatialAnalysis.MyWindow
{
/// <summary>
/// TagWindow.xaml 的交互逻辑
/// </summary>
public partial class TagWindow : Window
{
    internal TagWindow(string title, TagBean bean = null)
    {
        InitializeComponent();
        Title = title;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        DataContext = bean ?? new TagBean() { Name = string.Empty, Color = string.Empty };
    }
    //加载页面数据
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        string[] colors = XML.Map(XML.Params.tagColor).Split(',');
        string beanColor = ((TagBean)DataContext).Color;
        int index = -1;
        bool isSelected = beanColor != string.Empty;
        List<Border> items = new List<Border>();
        //添加颜色块
        for (int i = 0; i < colors.Length; i++)
        {
            byte[] rgb = ConversionUtil.HexToRgb(colors[i]);
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
            if (!isSelected)
                continue;
            if (colors[i] == beanColor)
                index = i;
        }
        if (isSelected)
        {
            if (index == -1)
            {
                byte[] rgb = ConversionUtil.HexToRgb(beanColor);
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
        string value = ((UIElement)selectColor.SelectedItem).Uid;
        color.Text = value;
        ((TagBean)DataContext).Color = value;
    }
    private void Yes_Click(object sender, RoutedEventArgs e)
    {
        TagBean bean = (TagBean)DataContext;
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
                ConversionUtil.HexToRgb(bean.Color);
                DialogResult = true;
                Close(); return;
            }
            catch (Exception ex)
            {
                Log.Warn("颜色代码有误 " + ex.Message);
            }
        }
        MessageBox.Show("颜色代码格式错误", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
    private void No_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
} }
