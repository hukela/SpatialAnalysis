using SpatialAnalysis.IO.Xml;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====");
            throw new AnException();
        }
        
        class AnException : ApplicationException
        {
            public AnException() : base("aaa")
            {
            }
        }
    }
}
