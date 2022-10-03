using SpatialAnalysis.Entity;
using SpatialAnalysis.Service;
using System.Windows;

namespace SpatialAnalysis.MyWindow
{
/// <summary>
/// SelectTagWindow.xaml 的交互逻辑
/// </summary>
public partial class SelectTagWindow : Window
{
    public SelectTagWindow()
    {
        InitializeComponent();
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        tagTree.ItemsSource = GetTagNodes(0);
    }
    //建立对应标签id的子标签节点
    private TagNode[] GetTagNodes(uint tagId)
    {

        TagBean[] tags = TagService.GetTagItemSource(tagId);
        TagNode[] nodes = new TagNode[tags.Length];
        for (int i = 0; i < tags.Length; i++)
        {
            nodes[i] = new TagNode()
            {
                Tag = tags[i],
                Children = GetTagNodes(tags[i].Id),
            };
        }
        return nodes;
    }
    //选中事件
    private void TagTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        yes.IsEnabled = true;
    }
    //用于传递参数
    public TagBean tagBean;
    //确定
    private void Yes_Click(object sender, RoutedEventArgs e)
    {
        tagBean = (tagTree.SelectedItem as TagNode)?.Tag;
        DialogResult = true;
        Close();
    }
    //取消
    private void No_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
    //删除标签
    private void Null_Click(object sender, RoutedEventArgs e)
    {
        tagBean = new TagBean();
        DialogResult = true;
    }
} }
