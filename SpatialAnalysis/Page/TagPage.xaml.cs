using SpatialAnalysis.Entity;
using SpatialAnalysis.Mapper;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SpatialAnalysis.MyPage
{
    /// <summary>
    /// TagPage.xaml 的交互逻辑
    /// </summary>
    public partial class TagPage : Page
    {
        //为方便刷新页面
        private ListView[] nodeList;
        long[] nodeParentId;
        public TagPage()
        {
            nodeList = new ListView[3];
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
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            firstNode.ItemsSource = TagService.GetItemDataSource(0);
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
                    Grid[] items = TagService.GetItemDataSource(parentId);
                    nodeList[i].ItemsSource = items;
                    for (int k = 0; k < items.Length; k++)
                        if (nodeParentId[i + 1] == long.Parse(items[k].Uid))
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
            ListView node = sender as ListView;
            if (node.SelectedItem == null)
                return;
            Grid grid = node.SelectedItem as Grid;
            uint tagId = uint.Parse(grid.Uid);
            int plies = GetPliesByName(node.Name);
            switch (plies)
            {
                case 0:
                    if (nodeParentId[1] == tagId)
                        return;
                    nodeParentId[1] = tagId;
                    nodeParentId[2] = -1;
                    RefreshAll();
                    break;
                case 1:
                    if (nodeParentId[2] == tagId)
                        return;
                    nodeParentId[2] = tagId;
                    RefreshAll();
                    break;
                case 2:
                    nodeParentId[3] = tagId;
                    break;
            }
        }
        //新建标签
        public void NewTag_Click(ListView sender)
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
        public void DeleteTag_Click(uint tagId, ListView sender)
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
            }
        }
    }
}
