using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpatialAnalysis.IO;
using SpatialAnalysis.MyWindow;

namespace SpatialAnalysis
{
    class Test: Base
    {
        /// <summary>
        /// 测试专用
        /// </summary>
        public static void beginTest()
        {
            string server = ConfigurationManager.AppSettings["SqlServer"];
            string database = locolPath + @"\Data\Database.mdf";
            string cpn = "Data Source=" + server + ";AttachDbFilename=" + database + ";user=sa;pwd=123456;";
            SqlConnection con = new SqlConnection(cpn);
            SqlCommand cmd = new SqlCommand("select * from dda", con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            TextWindow textWindow = new TextWindow();
            //遍历表头
            foreach (DataColumn column in dt.Columns)
            {
                textWindow.Write(column.ColumnName + "\t");
            }
            textWindow.Write("\n");
            foreach (DataRow row in dt.Rows)
            {
                foreach(dynamic value in row.ItemArray)
                    textWindow.Write(value + "\t");
                textWindow.Write("\n");
            }
            textWindow.ShowDialog();
            //直接中止程序运行
            Environment.Exit(0);
        }
    }
}
//Data Source = (LocalDB)\MSSQLLocalDB;
//AttachDbFilename=C:\Users\wang\source\repos\SpatialAnalysis\SpatialAnalysis\Data\Database.mdf;
//Integrated Security = True
