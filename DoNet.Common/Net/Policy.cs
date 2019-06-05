using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace DoNet.Common.Net
{
    /// <summary>
    /// Silverlihgt等通信策略监听类
    /// </summary>
    public class Policy
    {
        #region 策略请求943端口监听

        #region 策略请求部分-公共变量
        const string POLICY_STRING = "<policy-file-request/>";//这个是固定的
        static Socket sktPolicy; // 服务端监听的 socket        
        static byte[] arrPolicyClient; //收到客户端发送来的策略字符串请求 缓冲区
        static int policyReceiveByteCount; // 接收到的信息字节数(用来辅助判断字符串最否接收完整)  
        static byte[] arrPolicyServerFile;//策略文件转化成的字节数组   
        #endregion

        /// <summary>
        /// 启动 PolicyServer
        /// </summary>
        public static void StartPolicyListen()
        {
            //OutPut("(943端口)策略请求监听开始...");
            string policyFile = Path.Combine(Application.StartupPath, "Policy.xml");
            arrPolicyServerFile = File.ReadAllBytes(policyFile);

            // 初始化 socket ， 然后与端口绑定， 然后对端口进行监听
            sktPolicy = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sktPolicy.Bind(new IPEndPoint(IPAddress.Any, 943)); // socket 请求策略文件使用 943 端口
            sktPolicy.Listen(100);

            // 开始接受客户端传入的连接
            sktPolicy.BeginAccept(new AsyncCallback(PolicyConnectComplate), null);
        }

        /// <summary>
        /// （循环调用）连接回调处理
        /// </summary>
        /// <param name="result"></param>
        static void PolicyConnectComplate(IAsyncResult result)
        {
            Socket sktClient; // 客户端发过来的 socket
            try
            {
                // 完成接受客户端传入的连接的这个异步操作，并返回客户端连入的 socket
                sktClient = sktPolicy.EndAccept(result);
            }
            catch (SocketException)
            {
                return;
            }

            arrPolicyClient = new byte[POLICY_STRING.Length];//设置策略请求接收缓冲区大小

            policyReceiveByteCount = 0;

            try
            {
                // 开始接收客户端传入的数据
                sktClient.BeginReceive(arrPolicyClient, 0, POLICY_STRING.Length, SocketFlags.None, new AsyncCallback(PolicyReceiveComplte), sktClient);
            }
            catch (SocketException)
            {
                // socket 出错则关闭客户端 socket
                sktClient.Close();
            }


            // 继续开始接受客户端传入的连接
            sktPolicy.BeginAccept(new AsyncCallback(PolicyConnectComplate), null);
        }


        /// <summary>
        /// 客户端策略字符串接收回调函数
        /// </summary>
        /// <param name="result"></param>
        static void PolicyReceiveComplte(IAsyncResult result)
        {
            Socket sktClient = result.AsyncState as Socket;

            try
            {
                // 完成接收数据的这个异步操作，并计算累计接收的数据的字节数
                policyReceiveByteCount += sktClient.EndReceive(result);

                if (policyReceiveByteCount < POLICY_STRING.Length)
                {
                    // 没有接收到完整的数据，则继续开始接收(循环调用)
                    sktClient.BeginReceive(arrPolicyClient, policyReceiveByteCount, POLICY_STRING.Length - policyReceiveByteCount, SocketFlags.None, new AsyncCallback(PolicyReceiveComplte), sktClient);
                    return;
                }

                // 把接收到的数据转换为字符串
                string _clientPolicyString = Encoding.UTF8.GetString(arrPolicyClient, 0, policyReceiveByteCount);

                // OutPut("收到来自" + sktClient.RemoteEndPoint + " 的策略请求字符：" + _clientPolicyString);

                if (_clientPolicyString.ToLower().Trim() != POLICY_STRING.ToLower().Trim())
                {
                    // 如果接收到的数据不是“<policy-file-request/>”，则关闭客户端 socket
                    sktClient.Close();
                    return;
                }

                // 开始向客户端发送策略文件的内容
                sktClient.BeginSend(arrPolicyServerFile, 0, arrPolicyServerFile.Length, SocketFlags.None, new AsyncCallback(PolicySendComplete), sktClient);
            }
            catch (SocketException)
            {
                // socket 出错则关闭客户端 socket
                sktClient.Close();
            }
        }

        /// <summary>
        /// 策划文件发送回调处理
        /// </summary>
        /// <param name="result"></param>
        static void PolicySendComplete(IAsyncResult result)
        {
            Socket sktClient = result.AsyncState as Socket;
            try
            {
                // 完成将信息发送到客户端的这个异步操作
                sktClient.EndSend(result);

                // 获取客户端的ip地址及端口号，并显示
                // OutPut("已回发策略文件到 " + sktClient.RemoteEndPoint.ToString());
            }
            finally
            {
                // 关闭客户端 socket
                sktClient.Close();
            }
        }
        #endregion
    }
}
