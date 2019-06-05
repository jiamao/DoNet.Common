using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DoNet.Common.IO
{
    public class CPrinter:IDisposable
    {
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const int OPEN_EXISTING = 3;
        private const int INVALID_HANDLE_VALUE = -1;
        public static bool print = true;//是否接受打印

        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }

        [DllImport("kernel32.dll")]
        private static extern int CreateFile
        (
            string lpFileName,
            uint dwDesiredAccess,
            int dwShareMode,
            int lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            int hTemplateFile
        );

        [DllImport("kernel32.dll")]
        private static extern bool WriteFile
        (
            int hFile,
            byte[] lpBuffer,
            int nNumberOfBytesToWrite,
            ref int lpNumberOfBytesWritten,
            ref     OVERLAPPED lpOverlapped
        );

        [DllImport("kernel32.dll")]
        private static extern bool FlushFileBuffers(int hFile);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hFile);

        private static int iHandle = 0;    //打印机句柄
        private delegate bool writefile(int hf, byte[] lb, int nbw, ref int lbw, ref OVERLAPPED lpd);
        public static bool Open()
        {
            try
            {
                //if (!CommBase.ifUseOutside) { print = false; return false; }
                writefile wr = new writefile(WriteFile);
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("  ");
                mybyte[0] = 27;
                mybyte[1] = 64;
                if (iHandle == -1 || iHandle == 0) iHandle = CreateFile("lpt1", GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);
                IAsyncResult ir = wr.BeginInvoke(iHandle, mybyte, 2, ref i, ref x, null, null);
                long dtstart = DateTime.Now.Ticks;
                long dtend = DateTime.Now.Ticks;
                while (!ir.IsCompleted)
                {
                    //Application.DoEvents();
                    dtend = DateTime.Now.Ticks;
                    if (dtend - dtstart >= 1000000) { throw new Exception(" "); }
                }
                print = wr.EndInvoke(ref i, ref x, ir);
                if (iHandle != INVALID_HANDLE_VALUE)
                {
                    print = true;
                    return true;
                }
                else
                {
                    throw new Exception(" ");
                    //return false;
                }
            }
            catch
            {
                //if (CommBase.ShowMessage(CommBase.MessageMode.typeOKCancel, "无法连接打印机，是否继续操作不打印。") != CommBase.MessageMode.typeOK)
                //{
                //    Open();//继续检测打印机
                //}


                //Open();
                print = false;
                return false;

                throw new Exception("无法连接打印机!");

            }
        }


        public static bool Write(String Mystring)
        {
            if (!print) return true;
            if (iHandle != INVALID_HANDLE_VALUE && print)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                //byte[] mybyte = System.Text.Encoding.Default.GetBytes(Mystring + " ");
                //mybyte[mybyte.Length - 1] = 10;
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("  " + Mystring + " ");
                mybyte[0] = 29;
                mybyte[1] = 87;
                mybyte[mybyte.Length - 1] = 10;
                return WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }
        }
        public static bool Write(String Mystring, byte NL, byte NH)
        {
            if (!print) return true;
            if (iHandle != INVALID_HANDLE_VALUE && print)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("  " + Mystring + " ");
                mybyte[0] = 29;
                mybyte[1] = 87;
                mybyte[mybyte.Length - 2] = NL;
                mybyte[mybyte.Length - 1] = NH;

                return WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }
        }
        public static void SetBarHigh()
        {
            if (!print) return;
            if (iHandle != INVALID_HANDLE_VALUE)
            {

                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("   ");

                mybyte[0] = 29;
                mybyte[1] = 104;
                mybyte[2] = 81;
                WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }

        }
        //设置条码的宽度为最小
        public static void SetBarWith()
        {
            if (!print) return;
            //打
            if (iHandle != INVALID_HANDLE_VALUE)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("   ");

                mybyte[0] = 29;
                mybyte[1] = 119;
                mybyte[2] = 2;
                WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);

            }
            else
            {
                throw new Exception("打印端口未打开!");
            }

        }


        public static void SetWordTimes()
        {
            if (!print) return;
            //打
            if (iHandle != INVALID_HANDLE_VALUE)
            {

                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("  ");

                mybyte[0] = 27;
                mybyte[1] = 14;
                WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }

        }



        public static void SetWordCommon()
        {
            if (!print) return;
            //打
            if (iHandle != INVALID_HANDLE_VALUE)
            {

                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("  ");

                mybyte[0] = 27;
                mybyte[1] = 20;
                WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }

        }





        //打印条码
        public static bool WriteCode(string strCode, char c)
        {
            if (!print) return true;
            //打
            if (iHandle != INVALID_HANDLE_VALUE)
            {
                //GoNLines();
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("          " + strCode + " ");
                mybyte[0] = 29;
                mybyte[1] = 119;
                mybyte[2] = 2;
                mybyte[3] = 29;
                mybyte[4] = 72;
                mybyte[5] = 2;
                mybyte[6] = 29;
                mybyte[7] = 107;
                mybyte[8] = 4;
                mybyte[9] = Convert.ToByte(c);//条号的首个字母，此处默认为Ｓ                
                mybyte[mybyte.Length - 1] = 0;
                return WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }

        }
        //打印条码
        public static bool WriteCode(string strCode)
        {
            if (!print) return true;
            //打
            if (iHandle != INVALID_HANDLE_VALUE)
            {

                int i = 0;
                OVERLAPPED x = new OVERLAPPED();
                byte[] mybyte = System.Text.Encoding.Default.GetBytes("   " + strCode + " ");
                mybyte[0] = 29;
                mybyte[1] = 107;
                mybyte[2] = 0;
                //mybyte[3] = 83;//条号的首个字母，此处默认为Ｓ
                mybyte[mybyte.Length - 1] = 0;


                return WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("打印端口未打开!");
            }

        }


        /// <summary>
        /// 打开钱箱
        /// </summary>

        public static bool OpenCashBox()
        {

            if (!print) return true;
            if (iHandle != INVALID_HANDLE_VALUE)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();

                byte[] buf = new byte[5];
                buf[0] = 27;
                buf[1] = 112;
                buf[2] = 0;
                buf[3] = 60;
                buf[4] = 255;

                return WriteFile(iHandle, buf, 5, ref i, ref x);

            }
            else
            {
                throw new Exception("钱箱端口未打开!");
            }
        }
        /// <summary>
        /// 进纸Ｎ行
        /// </summary>
        /// <param name="n">进纸行数（N为0到255）</param>
        /// <returns></returns>
        public static bool GoNLines(int n)
        {
            if (!print) return true;
            if (iHandle != INVALID_HANDLE_VALUE)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();

                byte[] buf = new byte[3];
                buf[0] = 27;
                buf[1] = 100;
                buf[2] = (byte)n;

                return WriteFile(iHandle, buf, buf.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("端口未打开!");
            }
        }
        /// <summary>
        /// 加粗打印
        /// </summary>

        public static bool SetB(string str)
        {
            if (!print) return true;
            if (iHandle != INVALID_HANDLE_VALUE)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();

                byte[] buf = System.Text.Encoding.Default.GetBytes("    " + str + " ");
                buf[0] = 27;
                buf[1] = 14;
                buf[2] = 27;
                buf[3] = 69;
                buf[buf.Length - 1] = 1;

                return WriteFile(iHandle, buf, buf.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("端口未打开!");
            }
        }
        /// <summary>
        /// 取消加粗打印
        /// </summary>

        public static bool CannelB()
        {
            if (!print) return true;
            if (iHandle != INVALID_HANDLE_VALUE)
            {
                int i = 0;
                OVERLAPPED x = new OVERLAPPED();

                byte[] buf = new byte[3];
                buf[0] = 27;
                buf[1] = 69;
                buf[2] = 0;

                return WriteFile(iHandle, buf, buf.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("端口未打开!");
            }
        }
        public static bool AllWrite()
        {
            if (!print) return true;
            FlushFileBuffers(iHandle);
            return true;
            //return CloseHandle(iHandle);
        }

        public static bool Close()
        {
            if (!print) return true;            
            return CloseHandle(iHandle);
        }

        public void Dispose()
        {
            Close();
        }
    }
}
