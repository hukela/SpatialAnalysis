using System.Text;

namespace SpatialAnalysis.Entity
{
//节点对比信息，用于绑定页面
internal class ComparisonInfo
{
    // 基础信息
    public string Title { get; set; }
    public string Action { get; set; }
    public string CreateTime { get; set; }
    public string TagName { get; set; }
    public string OldTime { get; set; }
    public string NewTime { get; set; }
    public int OldFileCount { get; set; }
    public int NewFileCount { get; set; }
    public int OldDirCount { get; set; }
    public int NewDirCount { get; set; }
    public string OldSize { get; set; }
    public string NewSize { get; set; }
    public string SizeChanged { get; set; }
    public string OldUsage { get; set; }
    public string NewUsage { get; set; }
    public string UsageChanged { get; set; }
    public int OldExCode { get; set; }
    public int NewExCode { get; set; }
    // 文件变化量
    public string FileCountChanged
    {
        get
        {
            int result = NewFileCount - OldFileCount;
            if (result > 0)
                return string.Concat('+', result);
            else
                return result.ToString();
        }
    }
    // 文件夹变化量
    public string DirCountChanged
    {
        get
        {
            int result = NewDirCount - OldDirCount;
            return result > 0 ? string.Concat('+', result) : result.ToString();
        }
    }
    // 文件当前地址新
    public string Location
    {
        set => location = value;
        get => location == string.Empty ? "计算机硬盘" : location;
    }
    private string location = string.Empty;
    // 文件或文件夹读取异常信息
    public string ExInfo
    {
        get
        {
            if (OldExCode == 0 && NewExCode == 0)
                return string.Empty;
            else
                return "异常信息";
        }
    }
    public string OldExInfo => OldExCode == 0 ? string.Empty : buildExInfo(OldExCode);
    public string NewExInfo => NewExCode == 0 ? string.Empty : buildExInfo(NewExCode);
    private static string buildExInfo(int exCode)
    {
        string[] errInfos = RecordExCodeMap.GetValues(exCode);
        StringBuilder builder = new StringBuilder();
        foreach (string errInfo in errInfos)
            builder.Append(errInfo).Append('\n');
        builder.Remove(builder.Length - 1, 1);
        return builder.ToString();
    }
} }
