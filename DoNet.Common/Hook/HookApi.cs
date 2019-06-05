using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace DoNet.Common.Hook
{
    /// <summary>
    /// 手标操作API
    /// </summary>
    public class HookApi
    {
        /// <summary>
        /// 模拟鼠标事件的函数api
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "mouse_event")]
        public static extern void Mouse_Event(int flags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 设置光标到指定位置的函数api
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        /// 模拟键盘事件的函数api
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "keybd_event")]
        public static extern void Keybd_Event(
            byte bVk,
            byte bScan,
            int dwFlags,
            int dwExtraInfo
        );


        public const int WH_KEYBOARD_LL = 13; //创建键盘钩子类型
        //键盘Hook结构函数
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public delegate bool HookPro(int nCode, IntPtr wParam, IntPtr lParam); //创建委托，进行回调

        //安装钩子原型
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx
            (
                int hookid,
                HookPro pfnhook,
                IntPtr hinst,
                int threadid
            );

        //卸载钩子原型
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool UnhookWindowsHookEx
            (
               IntPtr hhook
            );

        //回调钩子原型
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool CallNextHookEx
            (
                IntPtr hhook,
                int code,
                IntPtr wparam,
                IntPtr lparam
            );

        //
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory
            (
                ref KBDLLHOOKSTRUCT Source,
                IntPtr Destination, int Length
            );
    }
}
