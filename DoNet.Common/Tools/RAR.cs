//////////////////////////////////////////////////
// Author   : 丁峰峰
// Date     : 2010/09/15
// Usage    : RAR操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;

namespace DoNet.Common.Tool
{
    public class RAR
    {
        static string _sRARPath = "";

        /// <summary>
        /// 获取RAR工具地址
        /// </summary>
        /// <returns></returns>
        public static string RARPath
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_sRARPath))
                    {
                        RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\Applications\\WinRAR.exe\\shell\\open\\command");
                        if (reg != null)
                        {
                            string p = reg.GetValue("").ToString();
                            if (!string.IsNullOrEmpty(p)) _sRARPath = p.Replace("%1", "").Replace("\"", "").Trim();
                            else _sRARPath = "rar";
                        }
                        else _sRARPath = "rar";
                    }
                }
                catch { }
                return _sRARPath;
            }
            set { _sRARPath = value; }
        }

        /// <summary>
        /// 获取RAR解压工具进程
        /// </summary>
        /// <returns></returns>
        public static System.Diagnostics.Process GetWinRARProcess()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = RARPath;//获取路径
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.ErrorDialog = false;
            p.StartInfo.UseShellExecute = false;
            return p;

        }
        /// <summary>
        /// 解压RAR文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <param name="toDir"></param>
        public static void ExcRAR(string source, string filter, string toDir, bool curdir)
        {
            IO.DirectoryHelper.CreateDirectory(toDir);

            System.Diagnostics.Process p = GetWinRARProcess();//生成解压进程
            p.StartInfo.Arguments = (curdir ? "e" : "x") + " -ibck -o+ -c \"" + source + (string.IsNullOrEmpty(filter) ? "\" \"" : "\" \"" + filter + "\" \"") + toDir + "\"";
            p.Start();
            p.WaitForExit();
        }

        /// <summary>
        /// 生成自动运行的包
        /// </summary>
        /// <param name="pakPath">需要打包的路径</param>
        /// <param name="icon">打包后的图标</param>
        /// <param name="autoFile">自动执行的文件名</param>
        /// <param name="pakFileName">打包后的包名</param>
        public static void CreateAutoPak(string pakPath, string autoFile, string pakFileName, string icon)
        {
            var arg1 = string.Format("\"{0}\" a -r -sfx -iicon\"{1}\" \"{2}\"", RARPath,icon, pakFileName);

            string pak = System.IO.Path.Combine(pakPath, pakFileName);
            if (System.IO.File.Exists(pak)) System.IO.File.Delete(pak);

            string tmptxt = System.IO.Path.Combine(pakPath, "sfx.txt");
            System.IO.File.WriteAllText(tmptxt, "Setup=\"" + autoFile + "\"\r\nTempMode\r\nSilent=1\r\nOverwrite=1");

            var arg2 = string.Format("\"{0}\" c -zsfx.txt \"{1}\"", RARPath, pakFileName);

            //执行命令
            var re = DoNet.Common.Process.Helper.RunCMDProcess(pakPath, 0, arg1, arg2);

            Console.WriteLine(re);

            System.IO.File.Delete(tmptxt);
        }
    }
}
