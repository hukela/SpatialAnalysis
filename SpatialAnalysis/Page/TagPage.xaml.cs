using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SpatialAnalysis.IO.Log;

namespace SpatialAnalysis.MyPage
{
/// <summary>
/// TagPage.xaml 的交互逻辑
/// </summary>
public partial class TagPage : Page
{
    //为方便刷新页面
    private readonly ListBox[] nodeList;
    private readonly long[] nodeParentId;
    //当前所展示的标签
    private uint selectedTagId;
    public TagPage()
    {
        nodeList = new ListBox[3];
        nodeParentId = new long[4];
        nodeParentId[0] = 0;
        nodeParentId[1] = -1;
        nodeParentId[2] = -1;
        nodeParentId[3] = -1;
        InitializeComponent();
        //只有在执行InitializeComponent方法后，各个控件变量才不为null
        nodeList[0] = firstNode;
        nodeList[1] = secondNode;
        nodeList[2] = thirdNode;
    }

    //加载页面数据
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        firstNode.ItemsSource = TagService.GetTagItemSource(0);
    }

    /// <summary>
    /// 通过名称获取节点层级
    /// </summary>
    /// <param name="name">对应节点对象的名称</param>
    private int GetPliesByName(string name)
    {
        switch (name)
        {
            case "firstNode":
                return 0;
            case "secondNode":
                return 1;
            case "thirdNode":
                return 2;
            default: throw new ApplicationException("无法识别新加标签的层数");
        }
    }

    /// <summary>
    /// 刷新所有的节点
    /// </summary>
    private void RefreshAll()
    {
        for (int i = 0; i < 3; i++)
        {
            if (nodeParentId[i] == -1)
                nodeList[i].ItemsSource = null;
            else
            {
                int index = -1;
                uint parentId = Convert.ToUInt32(nodeParentId[i]);
                TagBean[] items = TagService.GetTagItemSource(parentId);
                nodeList[i].ItemsSource = items;
                for (int k = 0; k < items.Length; k++)
                    if (nodeParentId[i + 1] == items[k].Id)
                    {
                        index = k;
                        break;
                    }
                if (index != -1)
                    nodeList[i].SelectedIndex = index;
            }
        }
    }

    /// <summary>
    /// 标签的点击事件
    /// </summary>
    private void Tag_Selected(object sender, SelectionChangedEventArgs e)
    {
        ListBox node = sender as ListBox;
        if (node.SelectedItem == null)
            return;
        //获取bean
        TagBean bean = node.SelectedItem as TagBean;
        //刷新标签栏
        selectedTagId = bean.Id;
        int plies = GetPliesByName(node.Name);
        switch (plies)
        {
            case 0:
                if (nodeParentId[1] == selectedTagId)
                    return;
                nodeParentId[1] = selectedTagId;
                nodeParentId[2] = -1;
                RefreshAll();
                break;
            case 1:
                if (nodeParentId[2] == selectedTagId)
                    return;
                nodeParentId[2] = selectedTagId;
                RefreshAll();
                break;
            case 2:
                nodeParentId[3] = selectedTagId;
                break;
        }
        //刷新地址栏
        tagName.Text = string.Concat('[', bean.Name, "]所标注的地址：");
        pathList.ItemsSource = TagService.GetPathItemSource(selectedTagId);
    }

    /// <summary>
    /// 新建标签
    /// </summary>
    /// <param name="sender">要添加标签的列表对象</param>
    internal void NewTag_Click(ListBox sender)
    {
        int plies = GetPliesByName(sender.Name);
        if (nodeParentId[plies] == -1)
        {
            MessageBox.Show("未选择父级标签", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        uint patentId = Convert.ToUInt32(nodeParentId[plies]);
        TagWindow window = new TagWindow("新建标签");
        bool? result = window.ShowDialog();
        if (result == true)
        {
            TagBean bean = window.DataContext as TagBean;
            bean.ParentId = patentId;
            TagMapper.InsertOne(bean);
            RefreshAll();
        }
    }

    /// <summary>
    /// 修改标签
    /// </summary>
    /// <param name="bean">要修改的标签对象</param>
    internal void EditTag_Click(TagBean bean)
    {
        TagWindow window = new TagWindow("修改标签", bean);
        bool? result = window.ShowDialog();
        if (result == true)
        {
            TagMapper.UpdateOne(bean);
            RefreshAll();
        }
    }

    /// <summary>
    /// 删除标签
    /// </summary>
    /// <param name="tagId">标签id</param>
    /// <param name="sender">当前标签list对象</param>
    internal void DeleteTag_Click(uint tagId, ListBox sender)
    {
        MessageBoxResult result = MessageBox.Show("是否确定删除该标签和其所有的子标签", "提示", MessageBoxButton.OKCancel);
        if (result != MessageBoxResult.OK) return;
        long id = Convert.ToInt64(tagId);
        int plies = GetPliesByName(sender.Name);
        TagService.DeleteTag(tagId);
        if (nodeParentId[plies + 1] == id)
        {
            for (int i = plies + 1; i < nodeParentId.Length; i++)
                nodeParentId[i] = -1;
        }
        RefreshAll();
        pathList.ItemsSource = null;
    }

    //这里用单击事件模拟双击
    private bool isClick;
    private Timer timer;
    /// <summary>
    /// 路径行点击事件
    /// </summary>
    /// <param name="sender">展示地址的对象(TextBlock)</param>
    private void Path_Edit(object sender, RoutedEventArgs e)
    {
        if (isClick)
        {
            timer.Stop();
            isClick = false;
            TextBox textBox = null;
            Grid grid = VisualTreeHelper.GetParent((UIElement)sender) as Grid;
            //进入编辑模式
            foreach (UIElement element in grid.Children)
            {
                if (element.Visibility == Visibility.Collapsed)
                {
                    element.Visibility = Visibility.Visible;
                    //顺便找到输入框控件，后面对其设置聚焦
                    if (element is TextBox box)
                        textBox = box;
                }
                else
                    element.Visibility = Visibility.Collapsed;
            }
            Debug.Assert(textBox != null, nameof(textBox) + " != null");
            // 记录编辑状态
            editingDirTagId = ((DirTagBean)pathList.SelectedItem).Id;
            mouseInItem = true;
            editingPathIsLock = false; // 放重复执行参数
            editingDirTextBox = textBox;
            if (editingDirTagId == 0)
                textBox.Text = string.Empty;
            //单独为当前ListBoxItem添加相关事件 防止其他item得到干扰
            DependencyObject parent = VisualTreeHelper.GetParent(grid);
            while (!(parent is ListBoxItem))
                parent = VisualTreeHelper.GetParent(parent);
            editingItem = parent as ListBoxItem;
            editingItem.MouseEnter += EditedItem_MouseEnter;
            editingItem.MouseLeave += EditedItem_MouseLeave;
            //添加保存事件
            Application.Current.MainWindow.MouseLeftButtonDown += Path_Save;
            //设置textBox为聚焦 因为在设置聚焦后会瞬间被ListBoxItem抢去所以这里延迟设置
            SetFocus(textBox, 1);
        }
        else
        {
            //用于模拟双击事件的
            isClick = true;
            timer = new Timer()
            {
                Interval = 300,
                AutoReset = false,
                Enabled = true,
            };
            timer.Elapsed += delegate { isClick = false; };
            timer.Start();
        }
    }

    //记录该编辑项中所有的相关数据
    private bool mouseInItem;
    private uint editingDirTagId;
    private TextBox editingDirTextBox;
    private ListBoxItem editingItem;
    private bool editingPathIsLock;
    //鼠标移入事件
    private void EditedItem_MouseEnter(object sender, MouseEventArgs e) { mouseInItem = true; }
    //鼠标移出事件
    private void EditedItem_MouseLeave(object sender, MouseEventArgs e) { mouseInItem = false; }

    /// <summary>
    /// 加锁 防止多线程重复执行
    /// </summary>
    private bool lockPathEdit()
    {
        lock (this)
        {
            if (editingPathIsLock)
                return false;
            editingPathIsLock = true;
            return true;
        }
    }

    /// <summary>
    /// 添加或修改标签的地址
    /// </summary>
    private void Path_Save(object sender, RoutedEventArgs e)
    {
        Log.Info("Path_Save");
        if (!lockPathEdit()) // 由于有可能有多个事件同时触发 所以这里要加锁防止重复执行
        {
            Log.Info("Path_Save return");
            return;
        }
        if (mouseInItem)
        {
            // 保持当前焦点状态 直接设置会报异常 这里延迟设置
            SetFocus(editingDirTextBox, 100);
            return;
        }
        //移除启动编辑时事件
        Application.Current.MainWindow.MouseLeftButtonDown -= Path_Save;
        editingItem.MouseEnter -= EditedItem_MouseEnter;
        editingItem.MouseLeave -= EditedItem_MouseLeave;
        //获取相关信息
        if (editingDirTextBox != null && editingDirTextBox.Text != string.Empty)
        {
            //添加
            if ( editingDirTagId == 0)
            {
                DirTagMapper.InsertOne(new DirTagBean()
                {
                    TagId = selectedTagId,
                    Path = editingDirTextBox.Text,
                });
            }
            //修改
            else
                DirTagMapper.UpdateById(editingDirTagId, editingDirTextBox.Text);
        }
        TagCache.DeleteTagCache();
        // 刷新时保持当前选中对象
        int index = pathList.SelectedIndex;
        pathList.ItemsSource = TagService.GetPathItemSource(selectedTagId);
        pathList.SelectedIndex = index;
        editingPathIsLock = false; // 释放锁
        Log.Info("修改地址完成");
    }

    /// <summary>
    /// 延迟异步设置输入框焦点
    /// </summary>
    /// <param name="element">设置对象</param>
    /// <param name="time">延迟时间 单位微秒</param>
    private void SetFocus(IInputElement element, long time)
    {
        DispatcherTimer setFocus = new DispatcherTimer()
        {
            Interval = new TimeSpan(time * 10000),
            IsEnabled = true,
        };
        setFocus.Tick += delegate { setFocus.Stop(); element.Focus(); };
    }

    /// <summary>
    /// 删除路径
    /// </summary>
    private void DeletePath_Click(object sender, RoutedEventArgs e)
    {
        //移除启动编辑时事件
        Application.Current.MainWindow.MouseLeftButtonDown -= Path_Save;
        editingItem.MouseEnter -= EditedItem_MouseEnter;
        editingItem.MouseLeave -= EditedItem_MouseLeave;
        //删除
        DirTagMapper.DeleteOneById(editingDirTagId);
        pathList.ItemsSource = TagService.GetPathItemSource(selectedTagId);
    }
} }
