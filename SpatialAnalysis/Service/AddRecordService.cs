using System.IO;
using SpatialAnalysis.Entity;
using SpatialAnalysis.MyWindow;
using SpatialAnalysis.Service.AddRecordExtend;
using System.Threading;
using SpatialAnalysis.IO;
using SpatialAnalysis.IO.Log;
using SpatialAnalysis.Utils;

namespace SpatialAnalysis.Service
{
internal static class AddRecordService
{
    private static FileInfo dataInfo;

    /// <summary>
    /// 获取数据库文件大小
    /// </summary>
    public static string GetDataSize()
    {
        if (dataInfo == null)
            dataInfo = new FileInfo(SQLiteClient.DATA_PATH);
        long size = dataInfo.Length;
        return ConversionUtil.StorageFormat(size, false);
    }

    /// <summary>
    /// 添加记录
    /// </summary>
    /// <param name="bean">事件bean</param>
    public static void AddIncident(IncidentBean bean)
    {
        Log.Info("开始添加记录");
        ProgramWindow window = new ProgramWindow();
        AddRecord addRecord = new AddRecord(bean, window);
        //建立异步线程来记录全盘文件
        addRecord.thread = new Thread(addRecord.StartAdd) { Name = "addRecord" };
        addRecord.thread.Start();
        window.ShowDialog();
    }
} }
