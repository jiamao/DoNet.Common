//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : path操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace DoNet.Common.IO
{
    public class PathMg
    {
        /// <summary>
        /// 是否是WEB调用
        /// </summary>
        public static bool IsWeb
        {
            get
            {
                return !(System.Web.HttpContext.Current == null);
            }
        }

        /// <summary>
        /// 获取根路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            return IsWeb ? System.Web.HttpContext.Current.Server.MapPath("~") : System.Windows.Forms.Application.StartupPath;
        }

        /// <summary>
        /// 转换网站路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        public static string CheckWebPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// 检 查路径是正确性
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string CheckPath(string strPath)
        {
            if (IsWeb)
            {
                return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetRootPath(), strPath));
            }
            else
            {                
                if (string.IsNullOrWhiteSpace(strPath))
                {
                    return System.Windows.Forms.Application.StartupPath;
                }

                if (System.IO.Path.IsPathRooted(strPath))
                {
                    return System.IO.Path.GetFullPath(strPath);
                }
                else
                {
                    return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, strPath));
                }
            }
        }

        /// <summary>
        /// 检 查路径是正确性
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string CheckPath(string RootPath, string TarPath)
        {
            RootPath = RootPath.Trim().TrimEnd('\\', '/');
            if (RootPath.Trim() == ".") RootPath = System.IO.Directory.GetCurrentDirectory();

            if (string.IsNullOrEmpty(TarPath) || TarPath.Trim() == "") return RootPath;
            TarPath = TarPath.TrimStart('\\', '/');
            if (System.IO.Path.IsPathRooted(TarPath))
            {
                return System.IO.Path.GetFullPath(TarPath);
            }
            else
            {
                return System.IO.Path.GetFullPath(System.IO.Path.Combine(RootPath, TarPath));
            }
        }

        /// <summary>
        /// 是否是网络路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool IsNetPath(string strPath)
        {
            if (!string.IsNullOrEmpty(strPath) && strPath.Trim().StartsWith("\\\\"))
            {
                return true;
            }
            return false;
        }

        static Dictionary<string, string> m_sFrameworkPath = new Dictionary<string, string>();
        /// <summary>
        /// 获取环NET路径
        /// </summary>
        /// <param name="ver">2.0|3.5</param>
        /// <returns></returns>
        public static string GetFrameworkPath(string ver)
        {
            if (m_sFrameworkPath.ContainsKey(ver)) return m_sFrameworkPath[ver];

            RegistryKey reg = Registry.LocalMachine;
            try
            {
                string p = "";
                //{1A5AC6AE-7B95-478C-B422-0E994FD727D6} 9.0 {1B2EEDD6-C203-4d04-BD59-78906E3E8AAB} 8.0
                RegistryKey regmid = reg.OpenSubKey("SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\" + ver);
                if (regmid == null)
                {
                    if (ver == "2.0")
                    {
                        regmid = reg.OpenSubKey("SOFTWARE\\Microsoft\\ASP.NET\\2.0.50727.0");
                        p = regmid.GetValue("Path").ToString();
                    }
                    else if (ver == "3.5")
                    {
                        regmid = reg.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\v3.5");
                        p = regmid.GetValue("InstallPath").ToString();
                    }

                }
                else
                {
                    p = regmid.GetValue("MSBuildToolsPath").ToString();
                }
                regmid.Close();

                m_sFrameworkPath.Add(ver, p);

                return p;

            }
            catch
            {
                return "";
            }
            finally
            {
                reg.Close();
            }
        }

        static Dictionary<string, string> m_sVSPath = new Dictionary<string, string>();
        /// <summary>
        /// 获取visual studio路径
        /// </summary>
        /// <param name="ver">2005|2008</param>
        /// <returns></returns>
        public static string GetVisualStudioPath(string ver)
        {
            if (m_sVSPath.ContainsKey(ver)) return m_sVSPath[ver];
            RegistryKey reg = Registry.LocalMachine;
            try
            {
                string m_sIDEPath = "";
                //{1A5AC6AE-7B95-478C-B422-0E994FD727D6} 9.0 {1B2EEDD6-C203-4d04-BD59-78906E3E8AAB} 8.0
                string vsguid = ver == "2005" ? "{1B2EEDD6-C203-4d04-BD59-78906E3E8AAB}" : "{1A5AC6AE-7B95-478C-B422-0E994FD727D6}";
                RegistryKey regdevenv = reg.OpenSubKey("software\\Classes\\CLSID\\" + vsguid + "\\LocalServer32");
                m_sIDEPath = regdevenv.GetValue("").ToString();
                regdevenv.Close();
                if (string.IsNullOrEmpty(m_sIDEPath)) m_sIDEPath = @"C:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv";
                m_sIDEPath = m_sIDEPath.Replace(".exe", "").Replace("\"%1\"", "");

                m_sVSPath.Add(ver, m_sIDEPath);

                return m_sIDEPath;
            }
            catch
            {
                return "";
            }
            finally
            {
                reg.Close();
            }
        }
    }
}
