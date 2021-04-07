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
        public TagPage()
        {
            InitializeComponent();
        }
        //加载页面数据
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            firstNode.ItemsSource = TagService.GetItemDataSource(0);
        }
        //当前选中的标签
        uint node1 = 0;
        uint node2 = 0;
        //标签的点击事件
        private void Tag_Selected(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            if (list.SelectedItem == null)
                return;
            Grid grid = list.SelectedItem as Grid;
            uint tagId = uint.Parse(grid.Uid);
            switch (list.Name)
            {
                case "firstNode":
                    node1 = tagId;
                    secondNode.ItemsSource = TagService.GetItemDataSource(tagId);
                    break;
                case "secondNode":
                    node2 = tagId;
                    thirdNode.ItemsSource = TagService.GetItemDataSource(tagId);
                    break;
                case "thirdNode": break;
                default: throw new ApplicationException("无法识别新加标签的层数");
            }
        }
        //新建标签
        public void NewTag_Click(byte plies)
        {
            TagWindow window = new TagWindow("新建标签");
            bool? result = window.ShowDialog();
            if (result == true)
            {
                TagBean bean = window.DataContext as TagBean;
                switch (plies)
                {
                    case 1:
                        TagMapper.AddOne(bean);
                        firstNode.ItemsSource = TagService.GetItemDataSource(0);
                        break;
                    case 2:
                        if (node1 == 0)
                        {
                            MessageBox.Show("未选中第一级标签", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                        bean.ParentId = node1;
                        TagMapper.AddOne(bean);
                        secondNode.ItemsSource = TagService.GetItemDataSource(node1);
                        break;
                    case 3:
                        if (node2 == 0)
                        {
                            MessageBox.Show("未选中第二级标签", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                        bean.ParentId = node2;
                        TagMapper.AddOne(bean);
                        thirdNode.ItemsSource = TagService.GetItemDataSource(node2);
                        break;
                    default:
                        throw new ApplicationException("未知的标签等级");
                }
            }
        }
        //修改标签
        public void EditTag_Click(uint id)
        {
        }
        //删除标签
        public void DeleteTag_Click(uint id)
        {
        }
    }
}
