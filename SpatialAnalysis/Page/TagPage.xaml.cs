using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
        //通过名称获取节点层级
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
        //刷新所有的节点
        private void RefreshAll()
        {
            for (int i = 0; i < 3; i++)
            {
                if (nodeParentId[i] == -1)
                    nodeList[i].ItemsSource = null;
                else
                {
                    int indiex = -1;
                    uint parentId = Convert.ToUInt32(nodeParentId[i]);
                    TagBean[] items = TagService.GetTagItemSource(parentId);
                    nodeList[i].ItemsSource = items;
                    for (int k = 0; k < items.Length; k++)
                        if (nodeParentId[i + 1] == items[k].Id)
                        {
                            indiex = k;
                            break;
                        }
                    if (indiex != -1)
                        nodeList[i].SelectedIndex = indiex;
                }
            }
        }
        //标签的点击事件
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
                default: break;
            }
            //刷新地址栏
            tagName.Text = string.Concat('[', bean.Name, "]所标注的地址：");
            pathList.ItemsSource = TagService.GetPathItemSource(selectedTagId);
        }
        //新建标签
        public void NewTag_Click(ListBox sender)
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
                TagMapper.AddOne(bean);
                RefreshAll();
            }
        }
        //修改标签
        public void EditTag_Click(TagBean bean)
        {
            TagWindow window = new TagWindow("修改标签", bean);
            bool? result = window.ShowDialog();
            if (result == true)
            {
                TagMapper.UpdataOne(bean);
                RefreshAll();
            }
        }
        //删除标签
        public void DeleteTag_Click(uint tagId, ListBox sender)
        {
            if (MessageBox.Show("是否确定删除该标签和其所有的子标签", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
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
        }
        //这里用单机事件模拟双击
        private bool isClick = false;
        private Timer timer;
        //路径行点击事件
        public void Path_Click(object sender, RoutedEventArgs e)
        {
            if (isClick)
            {
                timer.Stop();
                isClick = false;
                TextBox textBox = null;
                Grid grid = VisualTreeHelper.GetParent(sender as UIElement) as Grid;
                //进入编辑模式
                foreach (UIElement element in grid.Children)
                {
                    if (element.Visibility == Visibility.Collapsed)
                    {
                        element.Visibility = Visibility.Visible;
                        //顺便找到输入框控件，后面对其设置聚焦
                        if (element is TextBox)
                            textBox = element as TextBox;
                    }
                    else
                        element.Visibility = Visibility.Collapsed;
                }
                editedGrid = grid;
                isInIt = true;
                //为ListViewItem添加相关事件
                DependencyObject parent = VisualTreeHelper.GetParent(grid);
                while (!(parent is ListViewItem))
                    parent = VisualTreeHelper.GetParent(parent);
                ListViewItem item = parent as ListViewItem;
                item.MouseEnter += EditedItem_MouseEnter;
                item.MouseLeave += EditedItem_MouseLeave;
                MouseLeftButtonDown += EditedItem_MouseDown;
                pathList.MouseLeftButtonDown += EditedItem_MouseDown;
                tagName.MouseLeftButtonDown += EditedItem_MouseDown;
                pathGrid.MouseLeftButtonDown += EditedItem_MouseDown;
                //设置textBox为聚焦
                //因为在设置聚焦后会瞬间被ListViewItem抢去所以这里异步设置
                DispatcherTimer setFocus = new DispatcherTimer()
                {
                    //一微秒后设置聚焦
                    Interval = new TimeSpan(10000),
                    IsEnabled = true,
                };
                setFocus.Tick += delegate { setFocus.Stop(); textBox.Focus(); };
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
        private Grid editedGrid;
        private bool isInIt = true;
        //鼠标移入事件
        private void EditedItem_MouseEnter(object sender, MouseEventArgs e) { isInIt = true; }
        //鼠标移出事件
        private void EditedItem_MouseLeave(object sender, MouseEventArgs e) { isInIt = false; }
        //添加或修改标签的地址
        public void EditedItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isInIt)
                return;
            //取消注册事件
            MouseLeftButtonDown -= EditedItem_MouseDown;
            pathList.MouseLeftButtonDown -= EditedItem_MouseDown;
            tagName.MouseLeftButtonDown -= EditedItem_MouseDown;
            pathGrid.MouseLeftButtonDown -= EditedItem_MouseDown;
            //获取相关信息
            string editedGridUid = editedGrid.Uid;
            string editedPath = null;
            foreach (UIElement element in editedGrid.Children)
            {
                if (element is TextBox)
                    editedPath = (element as TextBox).Text;
            }
            if (editedPath != string.Empty)
            {
                //添加
                if (editedGridUid == "newPath")
                {
                    DirTagMapper.AddOne(new DirTagBean()
                    {
                        TagId = selectedTagId,
                        Path = editedPath,
                    });
                }
                //修改
                else
                    DirTagMapper.EditOneById(uint.Parse(editedGridUid), editedPath);
            }
            TagSupport.SetTagSort();
            int index = pathList.SelectedIndex;
            pathList.ItemsSource = TagService.GetPathItemSource(selectedTagId);
            pathList.SelectedIndex = index;
            isInIt = true; //设置为true，关闭该方法的相应
        }
        //删除路径
        public void DeletePath_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = VisualTreeHelper.GetParent(sender as UIElement) as Grid;
            DirTagMapper.DeleteOneById(uint.Parse(grid.Uid));
            pathList.ItemsSource = TagService.GetPathItemSource(selectedTagId);
        }
    }
}
