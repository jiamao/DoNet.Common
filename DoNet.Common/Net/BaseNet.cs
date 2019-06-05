using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Management;
using System.Runtime.InteropServices;

namespace DoNet.Common.Net
{
    /// <summary>
    /// 基础网络类
    /// </summary>
    public class BaseNet
    {
        /// <summary>
        /// 获取本地IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            IPAddress[] ips = Dns.GetHostEntry(GetHostName()).AddressList;
            if (ips != null)
            {
                foreach (IPAddress ip in ips)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取本地机器名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// 获取指定主机IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetHostName(string ip)
        {
            return GetRemoteHostName(ip);
        }

        /// <summary>
        /// 获取本机MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalMAC()
        {
            ManagementObjectSearcher MACQUERY = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = MACQUERY.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    return mo["MacAddress"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取主机名
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetRemoteHostName(string ip)
        {
            var dns = Dns.GetHostEntry(ip);
            return dns.HostName;
        }

        /// <summary>
        /// 获取WEB访问者的IP
        /// </summary>
        /// <returns></returns>
        public static string GetWebIPAddress()
        {
            return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 获取远程IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteIPAddress(string remoteName)
        {
            IPAddress[] ips = Dns.GetHostEntry(remoteName).AddressList;
            if (ips != null)
            {
                foreach (IPAddress ip in ips)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取远程机器MAC
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static string GetRemoteMAC(string addr)
        {

            string strRet = "Unknown";
            string strIPPattern = @"^\d+\.\d+\.\d+\.\d+$";
            Regex objRex = new Regex(strIPPattern);
            if (!objRex.IsMatch(addr))
            {
                addr = GetRemoteIPAddress(addr);
            }
            Int32 intDest = NETAPI.inet_addr(addr);
            Int32[] arrMAC = new Int32[2];
            Int32 intLen = 6;
            int intResult = NETAPI.SendARP(intDest, 0, ref arrMAC[0], ref intLen);
            if (intResult == 0)
            {
                Byte[] bs = new Byte[8];
                bs[5] = (Byte)(arrMAC[1] >> 8);
                bs[4] = (Byte)arrMAC[1];
                bs[3] = (Byte)(arrMAC[0] >> 24);
                bs[2] = (Byte)(arrMAC[0] >> 16);
                bs[1] = (Byte)(arrMAC[0] >> 8);
                bs[0] = (Byte)arrMAC[0];
                StringBuilder strbMAC = new StringBuilder();
                for (int intIndex = 0; intIndex < 6; intIndex++)
                {
                    if (intIndex > 0) strbMAC.Append("-");
                    strbMAC.Append(bs[intIndex].ToString("X2"));
                }
                strRet = strbMAC.ToString();
            }


            return strRet;

        }
    }

    public class NETAPI
    {
        [DllImport("Iphlpapi.dll")]
        public static extern int SendARP(Int32 dest, Int32 host, ref Int32 mac, ref Int32 length);

        [DllImport("Ws2_32.dll")]
        public static extern Int32 inet_addr(string ip);
    }
}
