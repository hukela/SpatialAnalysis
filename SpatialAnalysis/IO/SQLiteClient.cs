using System.Data;
using System.Data.SQLite;

namespace SpatialAnalysis.IO
{
/// <summary>
/// 针对SQLite数据库的各种操作
/// </summary>
internal static class SQLiteClient
{
    /// <summary>
    /// 建立数据库链接
    /// </summary>
    static SQLiteClient()
    {
        string conString = string.Concat("Data Source=", IoBase.localPath,
            @"\Data\main.db;Max Pool Size=10;Journal Mode=Off");
        con = new SQLiteConnection(conString);
    }
    private static readonly SQLiteConnection con;
    private static readonly SQLiteDataAdapter adapter = new SQLiteDataAdapter();
    /// <summary>
    /// 查看是否连接
    /// </summary>
    public static bool IsConnected
    {
        get
        {
            if (con == null)
                return false;
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                return false;
            else
                return true;
        }
    }
    /// <summary>
    /// 打开连接
    /// </summary>
    public static void OpenConnect()
    {
        if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
            con.Open();
    }
    /// <summary>
    /// 关闭连接
    /// </summary>
    public static void CloseConnect()
    {
        if (con.State != ConnectionState.Closed && con.State != ConnectionState.Broken)
            con.Close();
    }
    /// <summary>
    /// 读取数据库数据
    /// </summary>
    /// <param name="cmd">sql语句</param>
    /// <returns>表格数据</returns>
    public static DataTable Read(SQLiteCommand cmd)
    {
        //锁，因为一个链接只支持一个cmd运行
        lock(con)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            adapter.SelectCommand = cmd;
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
    /// <summary>
    /// 更改数据库数据
    /// </summary>
    /// <param name="cmd">sql语句</param>
    /// <returns>被改变的行数</returns>
    public static int Write(SQLiteCommand cmd)
    {
        lock(con)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            return cmd.ExecuteNonQuery();
        }
    }
    /// <summary>
    /// 执行sql语句
    /// </summary>
    public static void ExecuteSql(string sql)
    {
        using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
        {
            cmd.ExecuteNonQuery();
        }
    }
} }
