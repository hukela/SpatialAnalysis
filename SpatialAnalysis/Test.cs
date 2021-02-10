using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpatialAnalysis.IO;

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
            Console.WriteLine("----------");
            Console.WriteLine(dt);
            //直接中止程序运行
            Environment.Exit(0);
        }
    }
}
//Data Source = (LocalDB)\MSSQLLocalDB;
//AttachDbFilename=C:\Users\wang\source\repos\SpatialAnalysis\SpatialAnalysis\Data\Database.mdf;
//Integrated Security = True
