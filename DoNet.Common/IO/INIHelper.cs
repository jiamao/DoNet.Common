//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : ini操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DoNet.Common.IO
{
    /// <summary>
    ///  INI文件操作类
    /// </summary>
    public class INIHelper
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);

        /// <summary>
        /// 写ini文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static long IniWriteValue(string Section, string Key, string Value, string filepath)//对ini文件进行写操作的函数
        {
            return WritePrivateProfileString(Section, Key, Value, filepath);
        }

        /// <summary>
        /// 读取INI文件中的节点
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string IniReadValue(string Section, string Key, string filepath)//对ini文件进行读操作的函数
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
            255, filepath);
            return temp.ToString();
        }
    }
}
