using System;
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
            return con.State != ConnectionState.Closed && con.State != ConnectionState.Broken;
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
    /// 读取数据库数据 (指定类型)
    /// </summary>
    /// <param name="cmd">sql语句</param>
    /// <typeparam name="T">仅支持基础类型</typeparam>
    public static T[] Read<T>(SQLiteCommand cmd)
    {
        //锁，因为一个链接只支持一个cmd运行
        DataTable table = new DataTable();
        lock (con)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }
        // 判断是否有数据
        if (table.Rows.Count == 0)
            return default;
        // 判断T类型
        if (typeof(T) == typeof(string))
        {
            string[] result = new string[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = table.Rows[i][0].ToString();
            return result as T[];
        }
        if (typeof(T) == typeof(int))
        {
            int[] result = new int[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToInt32(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(uint))
        {
            uint[] result = new uint[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToUInt32(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(long))
        {
            long[] result = new long[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToInt64(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(ulong))
        {
            ulong[] result = new ulong[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToUInt64(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(float))
        {
            float[] result = new float[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToSingle(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(double))
        {
            double[] result = new double[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToDouble(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(bool))
        {
            bool[] result = new bool[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToBoolean(table.Rows[i][0]);
            return result as T[];
        }
        if (typeof(T) == typeof(DateTime))
        {
            DateTime[] result = new DateTime[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                result[i] = Convert.ToDateTime(table.Rows[i][0]);
            return result as T[];
        }
        throw new Exception("不支持的类型");
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
