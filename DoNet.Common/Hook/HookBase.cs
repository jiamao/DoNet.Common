//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace DoNet.Common.Hook
{
    /// <summary>
    /// 系统钩子
    /// </summary>
    public class HookBase
    {
        static IntPtr hHook = IntPtr.Zero; //创建钩子编号

        GCHandle _hookProcHandle;

        /// <summary>
        /// //创建按健委托．
        /// </summary>
        /// <param name="keycode"></param>
        /// <param name="modifierkeys"></param>
        public delegate void KeyPressCode(int keycode, System.Windows.Forms.Keys modifierkeys);

        /// <summary>
        /// 触发按健事件
        /// </summary>
        public event KeyPressCode OnKeyPressCode;

        /// <summary>
        /// 钩子回调
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public bool KEYBOARD_HOOKPRO(int nCode, IntPtr wParam, IntPtr lParam)
        {
            HookApi.KBDLLHOOKSTRUCT kb = new HookApi.KBDLLHOOKSTRUCT();
            HookApi.CopyMemory(ref kb, lParam, 20);      //结果就在这里了
            //if (kb.vkCode == 91) { return true; }//屏蔽左WIN健,,右边的是92

            UnLoadWindowsKeyBordHook();
            LoadWindowsKeyBordHook();

            if(OnKeyPressCode!=null)OnKeyPressCode(kb.vkCode, System.Windows.Forms.Control.ModifierKeys);//响应外界的事件

            return HookApi.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 设置钩子....调用此方法即可装载钩子
        /// </summary>
        public void LoadWindowsKeyBordHook()
        {
            if (hHook == IntPtr.Zero)
            {
                HookApi.HookPro hk = new HookApi.HookPro(this.KEYBOARD_HOOKPRO);
                _hookProcHandle = GCHandle.Alloc(hk);
                //挂钩子
                hHook = HookApi.SetWindowsHookEx(
                    HookApi.WH_KEYBOARD_LL,
                    hk,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule),
                    0);
                if (hHook == IntPtr.Zero)
                {
                    throw new Exception("安装钩子不成功！"); // 挂钩子不成功返回值 0
                }
            }
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        public void UnLoadWindowsKeyBordHook()
        {
            if (hHook != IntPtr.Zero)
            {
                //如果钩子已经挂上则取消钩子，否则不用取消
                HookApi.UnhookWindowsHookEx(hHook);
                _hookProcHandle.Free();
                hHook = IntPtr.Zero;
            }
        }

    }
}
