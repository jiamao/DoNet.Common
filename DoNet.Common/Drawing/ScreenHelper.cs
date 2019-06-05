//////////////////////////////////////////////////
// Author   : 家猫
// Date     : 2010/09/15
// Usage    : 屏幕操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DoNet.Common.Drawing
{
    /// <summary>
    /// 屏幕操作实例
    /// 用来全屏截图
    /// </summary>
    public class ScreenHelper
    {
        /// <summary>
        /// 获取全屏图像
        /// </summary>
        /// <returns></returns>
        public Bitmap GetFullScreen()
        {
            //方法一
            //IntPtr window = GDIAPI.GetDesktopWindow();
            //IntPtr windc = GDIAPI.GetDC(window);
            //IntPtr winbitmap = GDIAPI.GetCurrentObject(windc, 7);
            //Bitmap mimage = Image.FromHbitmap(winbitmap);
            //try
            //{
            //    GDIAPI.ReleaseDC(windc);
            //}
            //catch { } 
            //方法二
            Bitmap mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            Graphics gp = Graphics.FromImage(mimage);
            gp.CopyFromScreen(new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y), new Point(0, 0), mimage.Size, CopyPixelOperation.SourceCopy);

            ////方法三
            //IntPtr windc = GDIAPI.CreateDC("DISPLAY", null, null, (IntPtr)null);
            //Graphics g1 = Graphics.FromHdc(windc);
            //Bitmap mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, g1);
            //Graphics g2 = Graphics.FromImage(mimage);
            //IntPtr dc1 = g1.GetHdc();
            //IntPtr dc2 = g2.GetHdc();
            //GDIAPI.BitBlt(dc2, 0, 0, mimage.Width, mimage.Height, dc1, 0, 0, 13369376);
            //g1.ReleaseHdc(dc1);
            //g2.ReleaseHdc(dc2);
            ////GDIAPI.ReleaseDC(windc);
            //g1.Dispose();
            //g2.Dispose();
            //GC.Collect();
            return mimage;
        }

        /// <summary>
        /// 通过系统API截取全屏图像
        /// </summary>
        /// <returns></returns>
        public Bitmap GetFullScreenByApi()
        {
            //方法三
            IntPtr windc = GDIAPI.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            //Graphics g1 = Graphics.FromHdc(windc);
            var mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            Graphics g2 = Graphics.FromImage(mimage);
            //IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();
            GDIAPI.BitBlt(dc2, 0, 0, mimage.Width, mimage.Height, windc, 0, 0, 13369376);
            //g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);
            //try
            //{
            //    GDIAPI.ReleaseDC(windc);
            //}
            //catch
            //{ }
            //g1.Dispose();
            g2.Dispose();

            return mimage;
        }

        /// <summary>
        /// 截取窗体图
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Bitmap GetFormFace(System.Windows.Forms.Form f)
        {
            IntPtr windc = f.Handle;
            Graphics g1 = Graphics.FromHwnd(windc);
            Bitmap mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, g1);
            Graphics g2 = Graphics.FromImage(mimage);
            IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();
            GDIAPI.BitBlt(dc2, 0, 0, mimage.Width, mimage.Height, dc1, 0, 0, 13369376);
            g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);

            g1.Dispose();
            g2.Dispose();
            GC.Collect();
            return mimage;
        }
    }

    /// <summary>
    /// GDI API
    /// </summary>
    public class GDIAPI
    {
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_PAINT = 0x000F;
        public const int WM_SIZE = 0x0005;
        public const int WM_CHILDACTIVATE = 0x0022;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCMOUSELEVEL = 0x02A2;
        public const int WM_NCMOUSEHOVER = 0x02A0;

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr windowhander);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr windowhander);
        [DllImport("gdi32.dll")]
        public extern static IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);
        [DllImport("user32.dll")]
        public extern static void ReleaseDC(IntPtr hdc);
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern bool BitBlt(
        IntPtr hdcDest,     //    目标设备的句柄   
        int nXDest,         //    目标对象的左上角的X坐标   
        int nYDest,         //    目标对象的左上角的X坐标   
        int nWidth,         //    目标对象的矩形的宽度   
        int nHeight,        //    目标对象的矩形的长度   
        IntPtr hdcSrc,      //    源设备的句柄   
        int nXSrc,          //    源对象的左上角的X坐标   
        int nYSrc,          //    源对象的左上角的X坐标   
        System.Int32 dwRop  //    光栅的操作值   
        );
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateDC(
        string lpszDriver,    //    驱动名称   
        string lpszDevice,    //    设备名称   
        string lpszOutput,    //    无用，可以设定位"NULL"   
        IntPtr lpInitData    //    任意的打印机数据   
        );
    }

    public class NetConnectMap
    {
        public const int RESOURCETYPE_DISK = 0x0001;
        public const int RESOURCETYPE_PRINT = 0x0002;
        public const int RESOURCETYPE_ANY = 0x0000;
        public const int RESOURCE_CONNECTED = 0x0001;
        public const int RESOURCE_REMEMBERED = 0x0003;
        public const int RESOURCE_GLOBALNET = 0x0002;
        public const int RESOURCEDISPLAYTYPE_DOMAIN = 0x0001;
        public const int RESOURCEDISPLAYTYPE_GENERIC = 0x0000;
        public const int RESOURCEDISPLAYTYPE_SERVER = 0x0002;
        public const int RESOURCEDISPLAYTYPE_SHARE = 0x0003;
        public const int RESOURCEUSAGE_CONNECTABLE = 0x0001;
        public const int RESOURCEUSAGE_CONTAINER = 0x0002;
        // Error Constants:
        //public const intt ERROR_ACCESS_DENIED = 5&;
        //public const int ERROR_ALREADY_ASSIGNED = 85&;
        //public const int ERROR_BAD_DEV_TYPE = 66&;
        //public const int ERROR_BAD_DEVICE = 1200&;
        //public const int ERROR_BAD_NET_NAME = 67&;
        //public const int ERROR_BAD_PROFILE = 1206&;
        //public const int ERROR_BAD_PROVIDER = 1204&;
        //public const intt ERROR_BUSY = 170&;
        //public const int ERROR_CANCELLED = 1223&;
        //public const int ERROR_CANNOT_OPEN_PROFILE = 1205&;
        //public const int ERROR_DEVICE_ALREADY_REMEMBERED = 1202&;
        //public const int ERROR_EXTENDED_ERROR = 1208&;
        //public const intt ERROR_INVALID_PASSWORD = 86&;
        //public const int ERROR_NO_NET_OR_BAD_PATH = 1203&;

        public struct NETRESOURCE
        {     //   nr    
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        };

        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2A(NETRESOURCE[] lpNetResource, string lpPassword, string lpUserName, int dwFlags);

        [DllImport("mpr.dll")]
        public static extern int WNetCancelConnection2A(string sharename, int dwFlags, int fForce);
        public static int Connect(string remotePath, string localPath, string username, string password)
        {
            NETRESOURCE[] share_driver = new NETRESOURCE[1];
            share_driver[0].dwType = RESOURCETYPE_DISK;
            share_driver[0].lpLocalName = localPath;
            share_driver[0].lpRemoteName = remotePath;

            Disconnect(localPath);
            int ret = WNetAddConnection2A(share_driver, password, username, 1);

            return ret;
        }

        public static void Disconnect(string localpath)
        {
            WNetCancelConnection2A(localpath, 1, 1);
        }

    }

    public class FormAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr windowhander);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr windowhander);
        [DllImport("gdi32.dll")]
        public extern static IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);
        [DllImport("user32.dll")]
        public extern static void ReleaseDC(IntPtr hdc);
        [DllImport("user32.dll")]
        public extern static int GetAsyncKeyState(int vkey);
        /// <summary>
        /// 将窗体移至最顶端
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int BringWindowToTop(IntPtr hwnd);

        const int HWND_TOP = 0;
        const int SWP_SHOWWINDOW = 32;
        [DllImport("user32.dll")]
        public extern static IntPtr BeginDeferWindowPos(int nNumWindows);
        [DllImport("user32.dll")]
        public extern static IntPtr DeferWindowPos(IntPtr hWindowPosInfo, IntPtr hwnd, int hwndinsertafter, int x, int y, int cx, int cy, int uflags);
        [DllImport("user32.dll")]
        public extern static IntPtr EndDeferWindowPos(IntPtr hWindowPosInfo);
        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="windhwnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cw"></param>
        /// <param name="ch"></param>
        public static void MoveOrResizeForm(IntPtr windhwnd, int x, int y, int cw, int ch)
        {
            IntPtr hdwp;
            hdwp = BeginDeferWindowPos(1);
            DeferWindowPos(hdwp, windhwnd, HWND_TOP, x, y, cw, ch, SWP_SHOWWINDOW);
            EndDeferWindowPos(hdwp);
        }
    }

}
