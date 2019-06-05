//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : FTP操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace DoNet.Common.Net
{
    /// <summary>
    /// FTP操作类
    /// </summary>
    public class FTPClient
    {
        //开始定义各种WININET类库的常量********************************************
        const uint INTERNET_OPEN_TYPE_DIRECT = 1;
        const ulong INTERNET_OPEN_TYPE_PRECONFIG = 0;
        const uint INTERNET_DEFAULT_FTP_PORT = 21;
        const uint INTERNET_INVALID_PORT_NUMBER = 0;
        const uint INTERNET_SERVICE_FTP = 1;
        const uint INTERNET_FLAG_PASSIVE = 0x8000000;//初动模式
        const uint INTERNET_FLAG_PORT = 0x0; // 主动模式
        const uint INTERNET_FLAG_RELOAD = 0x80000000;
        const uint GENERIC_READ = 0x80000000;
        const uint FTP_TRANSFER_TYPE_BINARY = 2;
        const uint MAX_PATH = 260;
        const uint ERROR_INTERNET_EXTENDED_ERROR = 12003;
        const uint ERROR_NO_MORE_FILES = 18;
        //结束定义各种WININET类库的常量********************************************

        public string m_sErrMsg;        //存放出错信息

        protected string m_sUser;       //用户
        protected string m_sPwd;        //密码
        protected string m_sServer;     //FTP服务器地址

        bool m_bConnectReturn = false;    //FTP连接调用结束标志

        uint m_iFtpPort = 21;             //ftp port

        //存放查找文件信息的结构
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        class WIN32_FIND_DATA
        {
            public UInt32 dwFileAttributes = 0;
            public FILETIME ftCreationTme;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public UInt32 nFileSizeHigh = 0;
            public UInt32 nFileSizeLow = 0;
            public UInt32 dwReserved0 = 0;
            public UInt32 dwReserved1 = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string cFileName = null;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName = null;
        };


        //存放文件时间的结构
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        class FILETIME
        {
            public int dwLowDateTime = 0;
            public int dwHighDateTime = 0;
        };

        //以下开始引入WININET API **************************************************

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool InternetGetLastResponseInfo(ref uint ulError,
        [MarshalAs(UnmanagedType.LPTStr)] string strBuffer, ref uint ulBufferLength);

        [DllImport("wininet.dll", EntryPoint = "InternetOpen", CharSet = CharSet.Auto)]
        public static extern IntPtr InternetOpen(string strAppName, UInt32 nAccessType, string sProxy,
              string sProxyBypass, UInt32 nFlags);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetFindNextFile(IntPtr hFind, [In, Out] WIN32_FIND_DATA
         dirData);

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr InternetConnect(IntPtr ulSession, string
        strServer, uint ulPort, string strUser, string strPassword, uint ulService, uint ulFlags,
        uint ulContext);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool InternetGetConnectedState(ref uint ulFlags, uint
        ulReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool FtpSetCurrentDirectory(IntPtr ulSession, string
        strPath);

        [DllImport("wininet.dll", EntryPoint = "FtpGetCurrentDirectoryA", CharSet = CharSet.Auto)]
        static extern bool FtpGetCurrentDirectory(IntPtr ulSession, byte[] CurDir, ref int flag);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool FtpCreateDirectory(IntPtr ulSession, string
        directory);

        [DllImport("wininet.dll", EntryPoint = "FtpFindFirstFile", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FtpFindFirstFile(IntPtr ulSession, string strPath
        , [In, Out] WIN32_FIND_DATA dirData, int ulFlags, int ulContext);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool FtpGetFile(IntPtr ulSession, string strRemoteFile,
        string strLocalFile, bool bolFailIfExist, uint ulFlags, int ulInetFals, int
        ulContext);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FtpPutFile(IntPtr ulSession, string strLocalFile,
        string strRemoteFile, int ulFlags, int ulContext);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FtpDeleteFile(IntPtr ulSession, string strFileName);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FtpRenameFile(IntPtr ulSession, string strOldFileName, string strNewFileName);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool InternetCloseHandle(IntPtr ulSession);

        [DllImport("WinInet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FtpOpenFile(IntPtr hConnect, string lpszFileName, uint dwAccess, uint dwFlags, IntPtr dwContext);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint FtpGetFileSize(IntPtr hFile, ref uint dwSize);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool InternetGetLastResponseInfo(ref uint ulError, ref string sBuffer, ref uint ulBufferLength);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetLastError();

        //结束引入WININET API ******************************************************

        private IntPtr m_ulSession;             //FTP连接句柄
        private IntPtr m_ulOpen;                //internet会话句柄


        /// </summary>
        public FTPClient(string strFtpServer, string strUser, string strPwd, int iPort)
        {
            m_sServer = strFtpServer;
            m_sUser = strUser;
            m_sPwd = strPwd;
            m_iFtpPort = (uint)iPort;
        }

        public FTPClient()
        {
        }

        /// <summary>
        /// 设置FTP连接信息
        /// </summary>
        /// <param name="strFtpServer"></param>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <param name="iPort"></param>
        public void SetFtp(string strFtpServer, string strUser, string strPwd, int iPort)
        {
            m_sServer = strFtpServer;
            m_sUser = strUser;
            m_sPwd = strPwd;
            m_iFtpPort = (uint)iPort;
        }

        /// <summary>
        ///*************************************************************************
        /// </summary>
        public void ConnectFtp()
        {
            uint nFlag = INTERNET_FLAG_PASSIVE;
            m_ulSession = InternetConnect(m_ulOpen, m_sServer, m_iFtpPort,
            m_sUser, m_sPwd, INTERNET_SERVICE_FTP, nFlag, 0);
            m_bConnectReturn = true;            //设置调用已结束的标志
        }

        public int Open()
        {
            uint nError = 0;      //存放WININET调用错误号
            int iError = 0;       //存放WIN32错误号

            //重置FTP连接调用结束标志
            m_bConnectReturn = false;

            //打开WININET的INTERNET连接
            m_ulOpen = InternetOpen("C#Wininet", INTERNET_OPEN_TYPE_DIRECT, null, null, 0);
            if (m_ulOpen.ToInt32() == 0)
            {
                m_sErrMsg = "Error on InternetOpen!";
                return -1;
            }

            //新启用一个线程来连接FTP服务器
            Thread tFtp = new Thread(new ThreadStart(ConnectFtp));
            tFtp.Start();

            //如等待10秒后连接线程仍未成功返回，则终止它，并直接返回失败
            int i = 0;
            while (!m_bConnectReturn)
            {
                Thread.Sleep(500);
                i++;
                if (i > 20)
                {
                    tFtp.Abort();
                    m_sErrMsg = "on InternetConnect! time out!";
                    return -1;
                }
            }

            //如有错误，获取错误消息并返回失败,否则返回成功
            iError = Marshal.GetLastWin32Error();
            if (m_ulSession.ToInt32() == 0)
            {
                uint nLength = 1;
                InternetGetLastResponseInfo(ref nError, null, ref nLength);
                string sError = new string(' ', (int)nLength);
                InternetGetLastResponseInfo(ref nError, sError, ref nLength);
                m_sErrMsg = "on InternetConnect! " + nError.ToString() + "@" + iError.ToString() + "@" + sError;
                return -1;
            }
            else return 0;
        }


        /// </summary>
        public string GetFirstFile(string sSourceDir, string sTargetDir, string sFileExtension)
        {
            IntPtr hFind;                                   //文件查找句柄
            int dwType = 0;
            int iRet;                                       //错误号

            WIN32_FIND_DATA pData = new WIN32_FIND_DATA(); //文件查找结构

            //查找第一个符合指定扩展名的文件
            hFind = FtpFindFirstFile(m_ulSession, sSourceDir + "/*" + sFileExtension, pData, 0, 0);
            if (hFind.ToInt32() == 0)
            {
                iRet = Marshal.GetLastWin32Error();
                if (iRet == ERROR_INTERNET_EXTENDED_ERROR || iRet == ERROR_NO_MORE_FILES) //没有匹配的文件
                    return "";
                else //error occur
                {
                    m_sErrMsg = "Error on FtpFindFirstFile! errorcode=" + iRet.ToString();
                    return "#";
                }
            }

            while (pData.dwFileAttributes == 16)              //如是文件夹就继续查找下一个;
            {
                if (!InternetFindNextFile(hFind, pData))    //没有匹配的文件
                {
                    InternetCloseHandle(hFind);
                    return "";
                }
            }

            //下载查到的文件
            if (!FtpGetFile(m_ulSession, sSourceDir + "/" + pData.cFileName, sTargetDir + "\\" + pData.cFileName,
                            false, INTERNET_FLAG_RELOAD, dwType, 0))      //如下载出错,就返回失败
            {
                iRet = Marshal.GetLastWin32Error();
                m_sErrMsg = "Error on GetFile!" + iRet.ToString();
                InternetCloseHandle(hFind);
                return "#";
            }
            InternetCloseHandle(hFind);                     //关闭文件查询句柄

            return pData.cFileName;                         //返回成功下载的文件名
        }
        /// <summary>
        /// 检 查是否存在目录
        /// </summary>
        /// <param name="sDir"></param>
        /// <returns></returns>
        public bool CheckDirExsts(string sDir)
        {
            //IntPtr hFind;                                   //文件查找句柄

            //int iRet;                                       //错误号

            //WIN32_FIND_DATA pData = new WIN32_FIND_DATA(); //文件查找结构

            //查找第一个文件夹
            //hFind = FtpFindFirstFile(m_ulSession, sDir, pData, 0, 0);
            //FtpSetCurrentDirectory(m_ulSession, "~");//返回根目录
            if (FtpSetCurrentDirectory(m_ulSession, sDir))
            {
                //FtpSetCurrentDirectory(m_ulSession, "~");//返回根目录
                return true;
            }
            return false;
            //if (hFind.ToInt32() == 0)
            //{
            //  iRet = Marshal.GetLastWin32Error();
            // this.m_sErrMsg = "错误号：" + iRet.ToString();
            // return false;
            //}

            //if (pData.dwFileAttributes == 16)              //如是文件夹
            //{
            // return true;
            //}
            //return false;
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="sSourceDir"></param>
        /// <param name="sTargetDir"></param>
        /// <returns></returns>
        public int MoveRemoteFile(string sFileName, string sSourceDir, string sTargetDir)
        {

            CreateDirectory(sTargetDir);

            //将新目录中的同名文件删除
            SetDirectory(sTargetDir);
            FtpDeleteFile(m_ulSession, sFileName);

            //移动文件
            if (FtpRenameFile(m_ulSession, sSourceDir + "/" + sFileName, sTargetDir + "/" + sFileName)) return 0;
            else
            {
                uint nError = 0;      //存放WININET调用错误号
                uint nLength = 1;
                InternetGetLastResponseInfo(ref nError, null, ref nLength);
                string sError = new string(' ', (int)nLength);
                InternetGetLastResponseInfo(ref nError, sError, ref nLength);
                m_sErrMsg = "on FtpRenameFile! " + nError.ToString() + "@" + "@" + sError;
                return -1;
            }
        }

        ///*************************************************************************
        /// </summary>
        public int MoveLocalFile(string sFileName, string sSourceDir, string sTargetDir)
        {
            //将新目录中的同名文件删除
            File.Delete(sTargetDir + "\\" + sFileName);
            //移动文件
            File.Move(sSourceDir + "\\" + sFileName, sTargetDir + "\\" + sFileName);
            return 0;
        }
        /// <summary>
        /// 删除远程文件
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="sTargetDir"></param>
        /// <returns></returns>
        public bool DeleteRemoteFile(string sFileName, string sTargetDir)
        {
            //将新目录中的同名文件删除
            return FtpDeleteFile(m_ulSession, sTargetDir + "/" + sFileName);
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool CreateDirectory(string dir)
        {
            string[] dirs = dir.Split(new char[] { '\\', '/' });
            //string tmp = "";
            bool success = true;

            SetCurrentTop();//返回根目录

            foreach (string d in dirs)
            {
                if (string.IsNullOrEmpty(d)) continue;

                //tmp += (tmp == "" ? "" : "/") + d;
                if (!CheckDirExsts(d))//如果目录不存在
                {
                    success = FtpCreateDirectory(m_ulSession, d);//创建目录
                    if (!success) return success;
                }
                FtpSetCurrentDirectory(m_ulSession, d);
            }

            SetCurrentTop();//返回根目录
            return success;
        }

        /// <summary>
        /// 把当前目录设为DIR
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool SetDirectory(string dir)
        {
            string[] dirs = dir.Split(new char[] { '\\', '/' });
            bool success = true;

            SetCurrentTop();//返回根目录

            foreach (string d in dirs)
            {
                if (string.IsNullOrEmpty(d.Trim())) continue;

                if (!FtpSetCurrentDirectory(m_ulSession, d))
                {
                    return false;
                }
            }
            return success;
        }

        /// <summary>
        /// 把目录设为根目录
        /// </summary>
        /// <returns></returns>
        public bool SetCurrentTop()
        {
            string curDir = GetCurDirectory();
            foreach (char c in curDir)
            {
                if (c == '/' || c == '\\')
                {
                    FtpSetCurrentDirectory(m_ulSession, "..");//返回上一级
                }
            }
            return true;
        }

        /// <summary>
        /// 获取当前目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string GetCurDirectory()
        {
            int len = 256;
            byte[] curdir = new byte[len];
            if (FtpGetCurrentDirectory(m_ulSession, curdir, ref  len))
            {
                return System.Text.Encoding.Default.GetString(curdir).Trim('\0');
            }
            return "";
        }

        /// <summary>
        ///*************************************************************************
        /// 函数名称:PutFile    上传文件
        ///
        ///*************************************************************************
        /// </summary>
        public int PutFile(string sFileName, string sSourceDir, string sTargetDir)
        {
            if (!CreateDirectory(sTargetDir))//创建目录
            {
                this.m_sErrMsg = "目录：" + sTargetDir + " 无法创建";
                return -1;
            }

            SetDirectory(sTargetDir);//设置当前目录为sTargetDir

            sTargetDir = "";
            //sTargetDir = (string.IsNullOrEmpty(sTargetDir) ? "" : sTargetDir + "\\");

            //将远程目录中的同名文件删除
            FtpDeleteFile(m_ulSession, sTargetDir + sFileName);

            //上传文件
            if (FtpPutFile(m_ulSession, sSourceDir + "\\" + sFileName, sTargetDir + sFileName, 0, 0))
            {
                //比对字节数，验证传输是否正确完成
                FileInfo fiCheck = new FileInfo(sSourceDir + "\\" + sFileName);                   //创建本地文件信息对象
                IntPtr hFind;                                                               //文件查找句柄

                WIN32_FIND_DATA pData = new WIN32_FIND_DATA();                                //创建文件查找结构

                Console.WriteLine("find file: [" + sTargetDir + fiCheck.Name + "]"); //用于调试显示
                //在FTP服务器上查找刚刚上传的文件
                hFind = FtpFindFirstFile(m_ulSession, sTargetDir + fiCheck.Name, pData, 0, 0);

                if (hFind.ToInt32() != 0)                                                     //查到文件
                {
                    if (pData.nFileSizeLow != fiCheck.Length)                               //如和本地文件长度不一致,返回失败
                    {
                        InternetCloseHandle(hFind);
                        this.m_sErrMsg = "The file does not transfer correctly !";
                        Console.WriteLine(m_sErrMsg);
                        return -1;
                    }
                }
                else                                                                        //未发现上传的文件,返回失败
                {
                    int nError = Marshal.GetLastWin32Error();
                    this.m_sErrMsg = "Error on find file!" + nError.ToString();
                    Console.WriteLine(m_sErrMsg);
                    return -1;
                }

                InternetCloseHandle(hFind);                                                 //关闭文件查找句柄
                return 0;
            }
            else //上传失败
            {
                int nError = Marshal.GetLastWin32Error();
                return -1;
            }
        }

        /// <summary>
        ///*************************************************************************
        /// 函数名称:Close 关闭FTP服务器函数
        ///
        ///*************************************************************************
        /// </summary>
        public int Close()
        {
            //关闭FTP连接句柄
            InternetCloseHandle(m_ulSession);
            //关闭internet会话句柄
            InternetCloseHandle(m_ulOpen);
            return 0;
        }

        ~FTPClient()
        {
            try
            {
                Close();
            }
            catch { }
        }
    }
}
