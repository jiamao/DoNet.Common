//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DoNet.Common.Reflection
{
    /// <summary>
    /// 调用其它DLL的API
    /// </summary>
    public class ClassApi
    {
        [DllImport("kernel32.dll")]
        public extern static IntPtr LoadLibrary(string path);
        [DllImport("kernel32.dll")]
        public extern static IntPtr GetProcAddress(IntPtr lib, string funcName);

        [DllImport("kernel32.dll")]
        public extern static bool FreeLibrary(IntPtr lib);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("user32", EntryPoint = "CallWindowProc")]
        public static extern int CallWindowProc(IntPtr lpPrevWndFunc, int hwnd, int MSG, int wParam, int lParam);

    }
}
