//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 进程操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DoNet.Common.Process
{
    /// <summary>
    /// 进程操作类
    /// </summary>
    public class Helper
    {
        static object _lockCreateProcess = new object();
        /// <summary>
        /// 生成进程
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static System.Diagnostics.Process CreateProcess(string filename, string dir)
        {

            System.Diagnostics.Process p = new System.Diagnostics.Process();//进程
            p.StartInfo.FileName = filename;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;

            if (!string.IsNullOrEmpty(dir))
                p.StartInfo.WorkingDirectory = dir;

            return p;

        }

        /// <summary>
        /// 执行CMD进程，带参数
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RunCMDProcess(string filename, string args, string dir)
        {
            return RunCMDProcess(dir, 0, "\"" + filename + "\" " + args);            
        }

        /// <summary>
        /// 执行CMD进程，带参数
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RunCMDProcess(string filename, string args, string dir, int timeout)
        {
            return RunCMDProcess(dir, timeout, "\"" + filename + "\" " + args);           
        }
        public static string RunCMDProcess(string dir, int timeout, List<string> inputs)
        {
            return RunCMDProcess(dir, timeout, inputs.ToArray());
        }

        public delegate void delegateErrorInfoReceived(string err);
        public delegate void delegateNormalInfoReceived(string info);
        /// <summary>
        /// 执行CMD进程，带参数
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void RunCMDProcess(string dir, int timeout, delegateNormalInfoReceived shownormal, delegateErrorInfoReceived showerr, params string[] inputs)
        {
            try
            {
                System.Diagnostics.Process p = CreateProcess(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"), dir);

                p.StartInfo.RedirectStandardError = true;
                bool IsNotLive = true;

                p.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                {
                    //ShowErrorInfo(e.Data);
                    if (showerr != null) showerr(e.Data);
                    IsNotLive = false;
                });
                p.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                {
                    // ShowNormalInfo(e.Data);
                    if (shownormal != null) shownormal(e.Data);
                    IsNotLive = false;
                });

                lock (_lockCreateProcess)
                {
                    p.Start();
                }

                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                if (inputs != null && inputs.Length > 0)//写入执行参数
                {
                    foreach (string input in inputs)
                    {
                        p.StandardInput.WriteLine(input);
                        p.StandardInput.Flush();
                        System.Threading.Thread.Sleep(100);
                    }
                }

                p.StandardInput.WriteLine(" ");
                p.StandardInput.WriteLine("exit");



                while (WaitProcessTimeOut(p, timeout))//进程超时
                {
                    if (!IsNotLive)//只处理上次响应与这次的超时时间
                    {
                        IsNotLive = true;
                        continue;
                    }

                    p.Kill();//关闭进程

                    if (showerr != null) showerr("ProcessManager.RunCMDProcess 超时 ！" + timeout);

                    return;
                }

                p.WaitForExit();

                p.Close();
                p.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行CMD进程，带参数
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RunCMDProcess(string dir, int timeout, params string[] inputs)
        {
            try
            {
                System.Diagnostics.Process p = CreateProcess(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                    "cmd.exe"), dir);
                StringBuilder result = new StringBuilder();
                p.StartInfo.RedirectStandardError = true;

                p.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                {
                    ShowErrorInfo(e.Data);
                    result.AppendLine(e.Data);
                });
                p.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                {
                    ShowNormalInfo(e.Data);
                    result.AppendLine(e.Data);
                });

                lock (_lockCreateProcess)
                {
                    p.Start();
                }

                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                if (inputs != null && inputs.Length > 0)//写入执行参数
                {
                    foreach (string input in inputs)
                    {
                        p.StandardInput.WriteLine(input);
                        System.Threading.Thread.Sleep(50);
                    }
                }

                p.StandardInput.WriteLine(" ");
                p.StandardInput.WriteLine("exit");



                if (WaitProcessTimeOut(p, timeout))//进程超进
                {
                    p.Kill();//关闭进程
                    return "ProcessManager.RunCMDProcess 超时 ！" + timeout;
                }

                p.WaitForExit();
                p.Close();
                p.Dispose();

                return result.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="filename"></param>
        public static void RunFileProcess(string filename)
        {
            System.Diagnostics.Process p = CreateProcess(filename, "");
            p.Start();
            p.Close();
            p.Dispose();
        }
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="filename"></param>
        public static string RunFileProcess(string filename, int timeout)
        {
            System.Diagnostics.Process p = CreateProcess(filename, "");
            p.Start();

            if (WaitProcessTimeOut(p, timeout))//进程超进
            {
                p.Kill();//关闭进程
                return "ProcessManager.RunFileProcess 超时 ！" + timeout;
            }

            p.WaitForExit();
            p.Close();
            p.Dispose();
            return "true";
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="filename"></param>
        public static string RunFileProcess(string filename, string args, string dir, int timeout)
        {
            System.Diagnostics.Process p = CreateProcess(filename, dir);
            p.StartInfo.Arguments = args;
            p.Start();

            if (WaitProcessTimeOut(p, timeout))//进程超时
            {
                p.Kill();//关闭进程
                return "ProcessManager.RunFileProcess 超时 ！" + timeout;
            }

            string re = p.StandardOutput.ReadToEnd();

            p.WaitForExit();
            p.Close();
            p.Dispose();
            return re;
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="filename"></param>
        public static string RunFileProcess(string filename, string args, string dir, int timeout, bool output, delegateNormalInfoReceived shownormal, delegateErrorInfoReceived showerr)
        {
            System.Diagnostics.Process p = CreateProcess(filename, dir);
            p.StartInfo.Arguments = args;

            StringBuilder result = new StringBuilder();
            p.StartInfo.RedirectStandardError = true;

            p.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
            {
                if (showerr != null)
                {
                    showerr(e.Data);
                }

                ShowErrorInfo(e.Data);

                result.AppendLine(e.Data);
            });
            p.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
            {
                if (shownormal != null)
                {
                    shownormal(e.Data);
                }

                ShowNormalInfo(e.Data);
                result.AppendLine(e.Data);
            });

            p.Start();

            if (WaitProcessTimeOut(p, timeout))//进程超时
            {
                p.Kill();//关闭进程
                return "ProcessManager.RunFileProcess 超时 ！" + timeout;
            }

            string re = p.StandardOutput.ReadToEnd();

            p.WaitForExit();
            p.Close();
            p.Dispose();
            if (output) return re + result.ToString();

            return "true";
        }
        /// <summary>
        /// 获取当前进程个数
        /// </summary>
        /// <returns></returns>
        public static int GetCurProcessCount()
        {
            string procename = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(procename);
            return ps.Length;
        }
        /// <summary>
        /// 等待进程超时
        /// </summary>
        /// <param name="p"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool WaitProcessTimeOut(System.Diagnostics.Process p, int timeout)
        {
            const int millisecond = 1000;
            if (timeout <= 0) return false;

            int timestart = 0;

            while (!p.HasExited)
            {
                System.Threading.Thread.Sleep(millisecond);
                timestart += millisecond;
                if (timestart > timeout)
                {
                    return true;
                }
            }
            return false;
        }


        static object lockshoinfo = new object();
        public static void ShowErrorInfo(string error)
        {
            lock (lockshoinfo)
            {
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ForegroundColor = cc;
            }
        }

        public static void ShowNormalInfo(string info)
        {
            lock (lockshoinfo)
            {
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(info);
                Console.ForegroundColor = cc;
            }
        }
    }
}
