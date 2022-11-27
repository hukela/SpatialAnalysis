using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace SpatialAnalysis.IO
{
/// <summary>
/// 针对SQLite数据库的各种操作
/// </summary>
internal static class SQLiteClient
{
    // 数据库位置
    public static string DATA_PATH = IoBase.localPath + @"\Data\main.db";

    /// <summary>
    /// 建立数据库链接
    /// </summary>
    static SQLiteClient()
    {
        if (!File.Exists(DATA_PATH))
            return;
        string conString = string.Concat("Data Source=", DATA_PATH, ";Max Pool Size=10;Journal Mode=Off");
        con = new SQLiteConnection(conString, false);
    }

    private static readonly SQLiteConnection con;
    private static readonly SQLiteDataAdapter adapter = new SQLiteDataAdapter();

    /// <summary>
    /// 检查数据库是否存在
    /// </summary>
    public static bool check() { return con != null; }

    /// <summary>
    /// 打开连接
    /// </summary>
    public static void OpenConnect()
    {
        if (con == null)
            return;
        lock (con)
        {
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();
        }
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public static void CloseConnect()
    {
        if (con == null)
            return;
        lock (con)
        {
            if (con.State != ConnectionState.Closed && con.State != ConnectionState.Broken)
                con.Close();
        }
    }

    // 生成sql语句以方便查找错误
    private static string buildSql(SQLiteCommand cmd)
    {
        return cmd.Parameters
            .Cast<SQLiteParameter>()
            .Aggregate(
                cmd.CommandText,
                (current, p) => p.DbType == DbType.String ?
                        current.Replace('@' + p.ParameterName, "'" + p.Value + "'") :
                        current.Replace('@' + p.ParameterName, p.Value.ToString()));
    }

    /// <summary>
    /// 读取数据库数据
    /// </summary>
    /// <param name="cmd">sql语句</param>
    /// <returns>表格数据</returns>
    public static DataTable Read(SQLiteCommand cmd)
    {
        lock(con)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            adapter.SelectCommand = cmd;
            DataTable table = new DataTable();
            try
            {
                adapter.Fill(table);
            }
            catch (SQLiteException e)
            {
                throw new SQLiteException("error in SQLiteClient.Read\n" + buildSql(cmd), e);
            }
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
        DataTable table = Read(cmd);
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
    public static void Write(SQLiteCommand cmd)
    {
        lock(con)
        {
            if (con.State == ConnectionState.Broken)
                OpenConnect();
            cmd.Connection = con;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                throw new SQLiteException("error in SQLiteClient.Write\n" + buildSql(cmd), e);
            }
        }
    }

    /// <summary>
    /// 执行sql语句
    /// </summary>
    public static void ExecuteSql(string sql)
    {
        lock (con)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    throw new SQLiteException("error in SQLiteClient.ExecuteSql\n" + buildSql(cmd), e);
                }
        }
    }
} }
