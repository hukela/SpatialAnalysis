using System.Diagnostics;

namespace SpatialAnalysis.IO
{
/// <summary>
/// 用来执行命令提示符和批处理文件
/// </summary>
internal class Cmd
{
    public Cmd()
    {
        process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        ConfigCmd(process);
        process.Start();
    }
    //新建进程来打开cmd.exe
    //(这个方法好垃圾，但是没有其它更好的方法了)
    private Process process;
    private static void ConfigCmd(Process process)
    {
        //是否使用操作系统shell启动
        process.StartInfo.UseShellExecute = false;
        //截取输入流
        process.StartInfo.RedirectStandardInput = true;
        //截取输出流
        process.StartInfo.RedirectStandardOutput = true;
        //截取错误信息输出流
        process.StartInfo.RedirectStandardError = true;
        //不显示程序窗口
        process.StartInfo.CreateNoWindow = true;
    }
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="cmd">cmd命令</param>
    public void RunCmd(string cmd)
    {
        process.StandardInput.WriteLine(cmd);
    }
    /// <summary>
    /// 关闭cmd并获取运行结果
    /// </summary>
    /// <returns>返回正常信息和错误信息</returns>
    public string[] Close()
    {
        process.StandardInput.WriteLine("exit");
        process.WaitForExit();
        string[] result = new string[2];
        result[0] = process.StandardOutput.ReadToEnd();
        result[1] = process.StandardError.ReadToEnd();
        process.Close();
        return result;
    }
    /// <summary>
    /// 运行批处理文件
    /// </summary>
    /// <param name="path">绝对路径</param>
    /// <returns>返回正常信息和错误信息</returns>
    public static string[] RunCmdFile(string path)
    {
        Process process = new Process();
        process.StartInfo.FileName = path;
        ConfigCmd(process);
        process.Start();
        //单线程下只有全部运行结束后才能拿到运行结果
        process.WaitForExit();
        string[] result = new string[2];
        result[0] = process.StandardOutput.ReadToEnd();
        result[1] = process.StandardError.ReadToEnd();
        process.Close();
        return result;
    }
} }
