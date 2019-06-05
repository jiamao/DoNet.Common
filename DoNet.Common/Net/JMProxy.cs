using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DoNet.Common.Net.JMProxy
{
    /// <summary>
    /// 远程代理类
    /// 通过socket代理远程访问对象
    /// </summary>
    public class Server
    {
        private Socket listener;

        public event EventHandler AcceptRequestHandler;

        public void Start(int port)
        {
            this.Start(IPAddress.Any, port);
        }

        public void Start(IPAddress address, int port)
        {
            if (listener == null)
            {
                // 初始化 socket ， 然后与端口绑定， 然后对端口进行监听
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(address, port)); // socket 请求策略文件使用 943 端口
            }
            listener.Listen(100);

            // 开始接受客户端传入的连接
            listener.BeginAccept(new AsyncCallback(ConnectComplate), null);
        }

        public void Close()
        {
            if (listener != null)
            {
                listener.Close();
            }
        }

        /// <summary>
        /// （循环调用）连接回调处理
        /// </summary>
        /// <param name="result"></param>
        void ConnectComplate(IAsyncResult result)
        {
            Socket connect = null; // 客户端发过来的 socket
            try
            {
                // 完成接受客户端传入的连接的这个异步操作，并返回客户端连入的 socket
                connect = listener.EndAccept(result);
                var messager = new MessageManager(connect);
                if (AcceptRequestHandler != null)
                {
                    AcceptRequestHandler.BeginInvoke(messager, null, null, null);
                }
                else
                {
                    messager.Send("服务器还未准备接收连接，请稍候再试!");
                    messager.Close();
                }
            }
            catch (SocketException)
            {
                if (connect != null && connect.Connected)
                {
                    connect.Close();
                }
                return;
            }

            // 继续开始接受客户端传入的连接
            listener.BeginAccept(new AsyncCallback(ConnectComplate), null);
        }  
    }

    public class Client : MessageManager
    {
        public Client()
        {
            this.Connecter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Client(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            this.Connecter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public void Open()
        {
            this.Connecter.Connect(this.Host, this.Port);
            this.Init();//初始化
        }

    }

    /// <summary>
    /// 通信协议的包头
    /// </summary>
    public class PacketHeader
    {
        public PacketHeader()
        {
            this.Data = new List<byte>();
        }
        /// <summary>
        /// 包头byte数
        /// </summary>
        public const int HeaderLength = 16;

        /// <summary>
        /// 头部信息
        /// </summary>
        public List<byte> Data
        {
            get;
            set;
        }

        /// <summary>
        /// 包头是否已收集完毕
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return this.Data != null && this.Data.Count >= HeaderLength;
            }
        }

        /// <summary>
        /// 当前包的偏移量
        /// </summary>
        public Int64 Offset
        {
            get
            {
                return BitConverter.ToInt64(this.Data.Take<byte>(8).ToArray(), 0);
            }
            set
            {
                var bs = BitConverter.GetBytes(value);                
                for (var i = 0; i < bs.Length; i++)
                {
                    if (i < this.Data.Count)
                    {
                        this.Data[i] = bs[i];
                    }
                    else
                    {
                        this.Data.Add(bs[i]);
                    }
                }               
            }
        }

        public byte[] ToData()
        {
            var bs = new byte[HeaderLength];
            this.Data.CopyTo(bs, 0);
            return bs;
        }
    }

    public class Packet
    {
        public Packet()
        {
            Header = new PacketHeader();
            this.Data = new List<byte>();
        }

        /// <summary>
        /// 当前包头
        /// </summary>
        public PacketHeader Header
        {
            get;
            set;
        }

        public List<byte> Data
        {
            get;
            set;
        }

        public byte[] ToData()
        {
            this.Header.Offset = this.Data.Count;
            var bs = this.Header.ToData();
            return bs.Concat<byte>(this.Data).ToArray<byte>();
        }
    }

    /// <summary>
    /// 消息管理器
    /// </summary>
    public class MessageManager : IDisposable
    {
        public MessageManager()
        {            
            Buffer = new List<byte>();
        }

        public MessageManager(Socket s)
        {
            this.Connecter = s;
            Buffer = new List<byte>();
            this.Init();
        }

        public Packet DataPacket = new Packet();
        public event EventHandler<MessageEventArgs> ReceiveMessageHandle;
        /// <summary>
        /// 连接被关闭回调
        /// </summary>
        public event EventHandler ConnectColsedHandle;

        /// <summary>
        /// 当前连接
        /// </summary>
        public Socket Connecter
        {
            get;
            set;
        }

        public EndPoint RemoteAddress
        {
            get;
            set;
        }

        byte[] buffer = new byte[1];

        public List<byte> Buffer
        {
            get;
            set;
        }

        public void Init()
        {
            EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            var buffer = new byte[1024];
            this.RemoteAddress = this.Connecter.RemoteEndPoint;
            /*var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = this.Connecter.RemoteEndPoint;
            
            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += args_Completed;
            this.Connecter.ReceiveAsync(args);*/
            this.Connecter.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ep, msg_receive, buffer);
        }

        void msg_receive(IAsyncResult asy)
        {
            var buffer = asy.AsyncState as byte[];
            if (this.Connecter.Connected)
            {
                try
                {
                    var ep = this.Connecter.RemoteEndPoint;
                    var len = this.Connecter.EndReceiveFrom(asy, ref ep);
                    for (var i = 0; i < len; i++)
                    {
                        ReceiveComplte(buffer[i]);
                    }
                    this.Connecter.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ep, msg_receive, buffer);
                }
                catch (Exception ex)
                {
                    if (!this.Connecter.Connected)
                    {
                        //连接被断开
                        if (ConnectColsedHandle != null)
                        {
                            ConnectColsedHandle(this, null);
                        }
                    }
                }
            }
            else
            {
                //连接被断开
                if (ConnectColsedHandle != null)
                {
                    ConnectColsedHandle(this, null);
                }
            }
        }

        void args_Completed(object sender, SocketAsyncEventArgs e)
        {
            var socket = sender as Socket;
            if (socket.Connected)
            {
                for (var i = 0; i < e.BytesTransferred; i++)
                {
                    ReceiveComplte(e.Buffer[i]);
                }
                socket.ReceiveAsync(e);
            }
            else
            {
                //连接被断开
                if (ConnectColsedHandle != null)
                {
                    ConnectColsedHandle(this, null);
                }
            }
        }

        /// <summary>
        /// 接收回调函数
        /// </summary>
        /// <param name="result"></param>
        void ReceiveComplte(byte b)
        {
            //如果包头已完成
            if (DataPacket.Header.IsCompleted)
            {
                DataPacket.Data.Add(b);
                //包已完成收集则回调
                if (DataPacket.Data.Count == DataPacket.Header.Offset)
                {
                    if (ReceiveMessageHandle != null)
                    {
                        ReceiveMessageHandle.BeginInvoke(this, new MessageEventArgs(DataPacket), null, null);
                    }
                    DataPacket = new Packet();//重置包
                }
            }
            else
            {
                DataPacket.Header.Data.Add(b);
            }
        }

        public void Close()
        {
            if (this.Connecter != null && this.Connecter.Connected)
            {
                this.Connecter.Close();
            }
        }

        public void Send(string data)
        {
            var bs = System.Text.Encoding.UTF8.GetBytes(data);
            this.Send(bs);
        }

        public void Send(byte[] data)
        {
            var packet = new Packet();
            packet.Data.AddRange(data);
            this.Send(packet);
        }

        public void Send(Packet packet)
        {
            this.Connecter.Send(packet.ToData());
        }

        public void Dispose()
        {
            this.Close();
        }
    }

    /// <summary>
    /// 接收到的消息回调参数
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(Packet p)
        {
            this.Packet = p;
        }
        public Packet Packet
        {
            get;
            set;
        }
    }
}
