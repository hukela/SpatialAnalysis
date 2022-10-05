using System.Windows.Controls;

namespace SpatialAnalysis.MyPage
{
/// <summary>
/// RecordDetailPage.xaml 的交互逻辑
/// </summary>
public partial class RecordDetailPage : Page
{
    public RecordDetailPage(uint incidentId)
    {
        this.incidentId = incidentId;
        InitializeComponent();
        testBlock.Text = "事件id：" + incidentId;
    }
    private readonly uint incidentId;
} }