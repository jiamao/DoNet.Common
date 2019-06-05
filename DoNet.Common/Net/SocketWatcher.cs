using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DoNet.Common.Net
{
    /// <summary>
    /// 监听本机数据包
    /// </summary>
    public class SocketWatcher
    {
        /// <summary>
        /// 当前监听socket
        /// </summary>
        Socket socket = null;

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="ip"></param>
        public void Start(string ip = "")
        {
            //默认使用本机ipv4
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = BaseNet.GetLocalIPAddress();
            }
            if (socket == null)
            {                
               socket = new Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Raw, System.Net.Sockets.ProtocolType.IP);
               socket.Bind(new System.Net.IPEndPoint(IPAddress.Parse(ip), 0));               
               socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP, System.Net.Sockets.SocketOptionName.HeaderIncluded, 1);
               socket.IOControl(unchecked((int)0x98000001), new byte[4] { 1, 0, 0, 0 }, new byte[4]);

               var arg = new SocketAsyncEventArgs();
               var seg = new ArraySegment<byte>(new byte[65536]);
               arg.BufferList = new List<ArraySegment<byte>>();
               arg.BufferList.Add(seg);
               arg.Completed += arg_Completed;
               socket.ReceiveAsync(arg);
            }
        }

        /// <summary>
        /// 接收到数据包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void arg_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            { 
            
            }
        }
    }
}
