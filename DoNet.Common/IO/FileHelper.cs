//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace DoNet.Common.IO
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 强制删除一个文件
        /// </summary>
        /// <param name="filename"></param>
        public static void DeleteFile(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            SetFileNormal(filename);//去除只读属性
            System.IO.File.Delete(filename);
        }

        /// <summary>
        /// 去除文件的只读与隐藏属性
        /// </summary>
        /// <param name="filename"></param>
        public static void SetFileNormal(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            System.IO.FileAttributes fabs = System.IO.File.GetAttributes(filename);
            if ((fabs & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
            {
                fabs = fabs ^ System.IO.FileAttributes.ReadOnly;                
            }
            if ((fabs & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
            {
                fabs = fabs ^ System.IO.FileAttributes.Hidden;               
            }
            if ((fabs & System.IO.FileAttributes.System) == System.IO.FileAttributes.System)
            {
                fabs = fabs ^ System.IO.FileAttributes.System;               
            }

            System.IO.File.SetAttributes(filename, fabs);//读置文件属性
        }

        /// <summary>
        /// 设置文件属性
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fa"></param>
        public static void SetFileAttribute(string filename, System.IO.FileAttributes fa)
        {
            if (!System.IO.File.Exists(filename) && !System.IO.Directory.Exists(filename)) return;
           
            System.IO.File.SetAttributes(filename, fa);//读置文件属性
        }
        
        /// <summary>
        /// 强制拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyFile(string source, string target)
        {
            DeleteFile(target);
            string parentDir = System.IO.Path.GetDirectoryName(target);
           DirectoryHelper. CreateDirectory(parentDir);
            System.IO.File.Copy(source, target, true);
        }

        /// <summary>
        /// 通过文件名获取图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static System.Drawing.Icon GetIconByFileName(string fileName)
        {
            if (fileName == null || fileName.Equals(string.Empty)) return null;
            if (!System.IO.File.Exists(fileName)) return null;

            FileApi.SHFILEINFO shinfo = new FileApi.SHFILEINFO();           
            FileApi.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), FileApi.SHGFI_ICON | FileApi.SHGFI_SMALLICON);
            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            return myIcon;
        }

        /// <summary>
        /// 通过文件类型获取图标
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="isLarge"></param>
        /// <returns></returns>
        public static System.Drawing.Icon GetIconByFileType(string fileType, bool isLarge)
        {
            if (fileType == null || fileType.Equals(string.Empty)) return null;

            RegistryKey regVersion = null;
            string regFileType = null;
            string regIconString = null;
            string systemDirectory = Environment.SystemDirectory;

            if (fileType[0] == '.')
            {
                //读系统注册表中文件类型信息
                regVersion = Registry.ClassesRoot.OpenSubKey(fileType, true);
                if (regVersion != null)
                {
                    regFileType = regVersion.GetValue("") as string;
                    regVersion.Close();
                    regVersion = Registry.ClassesRoot.OpenSubKey(regFileType + @"\DefaultIcon", true);
                    if (regVersion != null)
                    {
                        regIconString = regVersion.GetValue("") as string;
                        regVersion.Close();
                    }
                }
                if (regIconString == null)
                {
                    //没有读取到文件类型注册信息，指定为未知文件类型的图标
                    regIconString = System.IO.Path.Combine(systemDirectory, "shell32.dll,0");
                }
            }
            else
            {
                //直接指定为文件夹图标
                regIconString = System.IO.Path.Combine(systemDirectory, "shell32.dll,3");
            }
            string[] fileIcon = regIconString.Split(new char[] { ',' });
            if (fileIcon.Length != 2)
            {
                //系统注册表中注册的标图不能直接提取，则返回可执行文件的通用图标
                fileIcon = new string[] { System.IO.Path.Combine(systemDirectory, "shell32.dll"), "2" };
            }
            System.Drawing.Icon resultIcon = null;
            try
            {
                //调用API方法读取图标
                int[] phiconLarge = new int[1];
                int[] phiconSmall = new int[1];
                uint count = FileApi.ExtractIconEx(fileIcon[0], Int32.Parse(fileIcon[1]), phiconLarge, phiconSmall, 1);
                IntPtr IconHnd = new IntPtr(isLarge ? phiconLarge[0] : phiconSmall[0]);
                resultIcon = System.Drawing.Icon.FromHandle(IconHnd);
            }
            catch { }
            return resultIcon;
        }

        /// <summary>
        /// 读取文件所有字符
        /// 非独占
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadAllText(string path, Encoding encoding)
        {
            var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            var strreader = new System.IO.StreamReader(fs, encoding);
            var content = strreader.ReadToEnd();
            fs.Close();
            return content;
        }
    }

    /// <summary>
    /// 文件操作API
    /// </summary>
    public class FileApi
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
    }
}
