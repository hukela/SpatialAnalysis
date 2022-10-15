using System;

namespace SpatialAnalysis.Utils
{
/// <summary>
/// 测试类，用于外部去访问非公开的类
/// </summary>
public class ForTest
{
    public static void Entrance()
    {
        LocalCache<int, int> localCache = new LocalCache<int, int>(4, test);
        Console.WriteLine(localCache.Get(1));
        Console.WriteLine(localCache.Get(2));
        Console.WriteLine(localCache.Get(1));
        localCache.Clear();
        Console.WriteLine(localCache.Get(1));
    }

    private static int test(int i)
    {
        Console.WriteLine("执行：" + i);
        return i + 1;
    }
} }
