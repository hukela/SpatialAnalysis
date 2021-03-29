using SpatialAnalysis.Utils;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //用于访问非公开的类
            ForTest.Entrance();
            Console.ReadKey(true);
        }
    }
}
