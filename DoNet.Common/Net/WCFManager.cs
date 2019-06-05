/////////////////////////////////////////////////////////
// Author   : DingFengfeng
// Date     : 2010/11/12 17:09:29
// Usage    : WCF服务帮助文件
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Runtime.Serialization;
using System.Xml;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DoNet.Common.Net
{
    public class WCFManager
    {
        /// <summary>
        /// 通道类型
        /// </summary>
        public enum WCFBindingType
        {
            /// <summary>
            /// TCP
            /// </summary>
            NetTcpBinding=0,
            /// <summary>
            /// HTTP
            /// </summary>
            BasicHttpBinding=1
        }

        /// <summary>
        /// 服务端
        /// </summary>
        public static WCFServer Server { get { return new WCFServer(); } }

        /// <summary>
        /// 生成客户端
        /// </summary>
        public static WCFClient Client { get { return new WCFClient(); } }

        /// <summary>
        /// 获取路径标识
        /// </summary>
        /// <param name="wcftype"></param>
        /// <returns></returns>
        public static string GetWcfAddressMark(WCFBindingType wcftype)
        {
            switch (wcftype)
            {
                case WCFManager.WCFBindingType.BasicHttpBinding: return "http";
                case WCFManager.WCFBindingType.NetTcpBinding: return "net.tcp";
                default: return "http";
            }
        }

        static TimeSpan _wcfTimeout = TimeSpan.FromMinutes(5);
        /// <summary>
        /// 服务超时间
        /// </summary>
        public static TimeSpan WCFTimeout
        {
            get
            {
                if (_wcfTimeout.Minutes == 5)
                { 
                var tmp=System.Configuration.ConfigurationManager.AppSettings["WcfTimeout"];
                    var timeoutmi=5;//默认五分钟
                    if (int.TryParse(tmp, out timeoutmi))
                    {
                        _wcfTimeout = TimeSpan.FromMinutes(timeoutmi);
                    }                    
                }
                return _wcfTimeout;
            }
            set
            {
                _wcfTimeout = value;
            }
        }


        /// <summary>
        /// 生成binding
        /// </summary>
        /// <param name="wcftype"></param>
        /// <returns></returns>
        public static Binding CreateBinding(WCFBindingType wcftype)
        {
            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
            quotas.MaxStringContentLength = int.MaxValue;
            
            switch (wcftype)
            {
                case WCFBindingType.BasicHttpBinding: 
                    { 
                        var binding = new BasicHttpBinding() { MaxReceivedMessageSize = int.MaxValue };
                        binding.Security.Mode = BasicHttpSecurityMode.None;
                        binding.ReaderQuotas = quotas;
                        binding.ReceiveTimeout = binding.SendTimeout = WCFTimeout;
                        return binding;
                    }
                case WCFBindingType.NetTcpBinding: 
                    { 
                        var binding = new NetTcpBinding() { MaxReceivedMessageSize = int.MaxValue };
                        binding.Security.Mode = SecurityMode.None;
                        binding.ReaderQuotas = quotas;
                        binding.ReceiveTimeout = binding.SendTimeout = WCFTimeout;                        
                        return binding;
                    }
                default: { 
                    var binding = new NetTcpBinding() { MaxReceivedMessageSize = int.MaxValue };
                    binding.Security.Mode = SecurityMode.None;
                    binding.ReaderQuotas = quotas;
                    binding.ReceiveTimeout = binding.SendTimeout = WCFTimeout;
                    return binding; }
            }
        }

        /// <summary>
        /// 获取通信头
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="ns">URL</param>
        /// <returns></returns>
        public static object GetHeader(string name, string ns)
        {
            return GetHeader<object>(name, ns);
        }

        /// <summary>
        /// 获取WCF通信头
        /// </summary>
        /// <typeparam name="T">消息头类型</typeparam>
        /// <param name="name">消息头名称</param>
        /// <param name="ns">URL</param>
        /// <returns></returns>
        public static T GetHeader<T>(string name, string ns)
        {
            foreach (var h in OperationContext.Current.IncomingMessageHeaders)
            {
                if (h.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    var mh = OperationContext.Current.IncomingMessageHeaders.GetHeader<T>(h.Name, ns);
                    return mh;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 向服务端发送消息头
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="ns">URL</param>
        /// <param name="value">消息内容</param>
        public static void SendHeader(string name, string ns, object value)
        {
            //生成头
            MessageHeader header = MessageHeader.CreateHeader(name, ns, value);

            if (OperationContext.Current != null)
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(header);
            }
        }
    }

    /// <summary>
    /// WCF服务端
    /// </summary>
    public class WCFServer
    {
        static Dictionary<string, ServiceHost> BindedWcfServers = new Dictionary<string, ServiceHost>();//记录已绑定的服务

        /// <summary>
        /// 获取客户的IP地址
        /// </summary>
        /// <returns></returns>
        public string GetRemoteIpAddress()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties messageProperties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty =
            messageProperties[RemoteEndpointMessageProperty.Name]
            as RemoteEndpointMessageProperty;

            return endpointProperty.Address;
        }

        /// <summary>
        /// 获取服务端的回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetServerCallBack<T>()
        { 
         // 声明回调接口
            T callback = OperationContext.Current.GetCallbackChannel<T>();
            return callback;
        }

        /// <summary>
        /// 绑定WCF服务通过配置
        /// </summary>
        /// <param name="objectType"></param>
        public void StartServerBind(Type objectType)
        {
            ServiceHost host = new ServiceHost(objectType);

            
            if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                ServiceMetadataBehavior metadata = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(metadata);
            }

            host.Open();
        }

        /// <summary>
        /// 绑定WCF服务
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="interfaceType">接口或相关类型</param>
        /// <param name="ipaddress">绑定IP地址</param>
        /// <param name="port">绑定端口</param>
        /// <param name="servermark">当前类型标识</param>
        public void StartServerBind(Type objectType, Type interfaceType, WCFManager.WCFBindingType wcftype, string ipaddress, int port, string servermark)
        {
            var serverurl = string.Format("{0}://{1}:{2}", WCFManager.GetWcfAddressMark(wcftype), ipaddress, port) + "/" + servermark;
            serverurl = serverurl.ToLower().Trim();
            //如果已经绑定过..则直接返回
            if (BindedWcfServers.ContainsKey(serverurl)) 
            {
                //关闭已存在的服务
                var hosttmp = BindedWcfServers[serverurl];
                hosttmp.Close();
                BindedWcfServers.Remove(serverurl);
            }

            ServiceHost host = new ServiceHost(objectType, new Uri(string.Format("{0}://{1}:{2}", WCFManager.GetWcfAddressMark(wcftype), ipaddress, port)));
            
            BindedWcfServers.Add(serverurl, host);//记录
            
            host.AddServiceEndpoint(interfaceType, WCFManager.CreateBinding(wcftype), servermark);
            
            if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                ServiceMetadataBehavior metadata = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(metadata);                
            }
            
            host.Open();
        }

        /// <summary>
        /// 绑定WCF服务
        /// </summary>
        /// <param name="objpath">对象的文件路径</param>
        /// <param name="objclass">类名</param>
        /// <param name="interfacepath">接口或其它路径</param>
        /// <param name="interfacename">接口或其它类名</param>
        /// <param name="wcftype">WCF类型</param>
        /// <param name="ipaddress">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="servermark">服务标识</param>
        public void StartServerBind(string objpath, string objclass, string interfacepath, string interfacename,
            WCFManager.WCFBindingType wcftype, string ipaddress, int port, string servermark)
        {
            var objtype = DoNet.Common.Reflection.ClassHelper.GetClassType(objpath, objclass);
            var interfacetype = DoNet.Common.Reflection.ClassHelper.GetClassType(interfacepath, interfacename);

            StartServerBind(objtype, interfacetype, wcftype, ipaddress, port, servermark);
        }

        /// <summary>
        /// 绑定WCF服务
        /// </summary>
        /// <param name="objdata"></param>
        /// <param name="objclass"></param>
        /// <param name="interfacedata"></param>
        /// <param name="interfacename"></param>
        /// <param name="wcftype"></param>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        /// <param name="servermark"></param>
        public void StartServerBind(byte[] objdata, string objclass,string interfacename,
            WCFManager.WCFBindingType wcftype, string ipaddress, int port, string servermark)
        {
            var ass = System.Reflection.Assembly.Load(objdata);
            var objtype = ass.GetType(objclass);
            var interfacttype = ass.GetType(interfacename);

            StartServerBind(objtype, interfacttype, wcftype, ipaddress, port, servermark);
        }
    }

    /// <summary>
    /// WCF客户端
    /// </summary>
    public class WCFClient
    {
        static Dictionary<string, ChannelFactory> _channelfactorys = new Dictionary<string, ChannelFactory>();
        static Dictionary<string, object> _channelproxys = new Dictionary<string, object>();

        /// <summary>
        /// 配置节点名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpointConfigurationName"></param>
        /// <returns></returns>
        public T CreateRemoteObject<T>(string endpointConfigurationName)
        {
            var channelFactory = new ChannelFactory<T>(endpointConfigurationName);
            var proxy = channelFactory.CreateChannel();
            return proxy;
        }

        /// <summary>
        /// 配置节点名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpointConfigurationName"></param>
        /// <returns></returns>
        public T CreateRemoteObject<T>(Binding binding,EndpointAddress address)
        {
            var channelFactory = new ChannelFactory<T>(binding, address);
            var proxy = channelFactory.CreateChannel();
            return proxy;
        }

        /// <summary>
        /// 生成远程代理对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wcftype"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public T CreateRemoteObject<T>(WCFManager.WCFBindingType wcftype, string ip, int port, string mark,TimeSpan timeout,object callback=null)
        {
            var binding = WCFManager.CreateBinding(wcftype);
            binding.ReceiveTimeout = timeout;
            var url = string.Format("{0}://{1}:{2}", WCFManager.GetWcfAddressMark(wcftype), ip, port) + "/" + mark;
            url=url.ToLower().Trim();
            if (_channelfactorys.ContainsKey(url))
            {
                if (_channelfactorys[url].State == CommunicationState.Opened)
                {
                    try
                    {
                        _channelfactorys[url].Close();
                    }
                    catch { }
                }
                _channelfactorys.Remove(url);
                _channelproxys.Remove(url);
            }
            if (callback == null)
            {
                var channelFactory = new ChannelFactory<T>(binding, url);

                var proxy = channelFactory.CreateChannel();
                _channelfactorys.Add(url, channelFactory);
                _channelproxys.Add(url, proxy);
                return proxy;
            }
            else
            {
                var ctx = new InstanceContext(callback);
                var channelFactory = new DuplexChannelFactory<T>(ctx, binding, url);                
                var proxy = channelFactory.CreateChannel();
                _channelfactorys.Add(url, channelFactory);
                _channelproxys.Add(url, proxy);
                return proxy;
            }
        }

        /// <summary>
        /// 注册回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="wcftype"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="mark"></param>
        public void RegisterCallbackObject<T>(object instance, WCFManager.WCFBindingType wcftype, string ip, int port, string mark)
        {
            var url = string.Format("{0}://{1}:{2}", WCFManager.GetWcfAddressMark(wcftype), ip, port) + "/" + mark;
            url = url.ToLower().Trim();
            if (_channelfactorys.ContainsKey(url))
            {
                var channel = _channelfactorys[url] as DuplexChannelFactory<T>;

                var ctx = new InstanceContext(instance);
                var proxy = channel.CreateChannel(ctx);
            }
        }

        /// <summary>
        /// 关闭信道
        /// </summary>
        /// <param name="wcftype"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="mark"></param>
        public void CloseChannel(WCFManager.WCFBindingType wcftype, string ip, int port, string mark)
        {
            var url = string.Format("{0}://{1}:{2}", WCFManager.GetWcfAddressMark(wcftype), ip, port) + "/" + mark;
            url = url.ToLower().Trim();
            if (_channelfactorys.ContainsKey(url))
            {
                _channelfactorys[url].Close();
                _channelfactorys.Remove(url);
            }
        }

        /// <summary>
        /// 设置https建立信任关系回调
        /// </summary>
        public static void SetServerCertificateValidationCallback()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
        }

        /// <summary>
        /// SSL回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }

    /// <summary>
    /// WCF通信头
    /// </summary>
    public class WCFHeader:IClientMessageInspector
    {
        MessageHeader _header;

        /// <summary>
        /// 实例化消息头
        /// </summary>
        /// <param name="name">消息头名称</param>
        /// <param name="ns">URL</param>
        /// <param name="value">消息内容</param>
        public WCFHeader(string name,string ns,object value)
        {
            _header = MessageHeader.CreateHeader(name, ns, value);
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 请求服务前
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(_header);
            return _header;
        }
    }

    /// <summary>
    /// WCF消息头属性
    /// </summary>
    public class CallWCFAttribute : Attribute, IEndpointBehavior, IOperationBehavior
    {
        /// <summary>
        /// 所有消息头
        /// </summary>
        ICollection<WCFHeader> _headers = new List<WCFHeader>();

        /// <summary>
        /// WCF通信消息头
        /// </summary>
        public CallWCFAttribute() { }

        /// <summary>
        /// 消息头
        /// </summary>
        /// <param name="name">消息名称</param>
        /// <param name="ns">UL</param>
        /// <param name="value">消息内容</param>
        public CallWCFAttribute(string name, string ns, object value)
        {
            _headers.Add(new WCFHeader(name,ns,value));
        }

        /// <summary>
        /// WCF通信消息头
        /// </summary>
        /// <param name="heads">多个消息头一起</param>
        public CallWCFAttribute(WCFHeader head)
        {
            _headers.Add(head);
        }
        /// <summary>
        /// WCF通信消息头
        /// </summary>
        /// <param name="head1">消息头</param>
        /// <param name="head2">消息头</param>
        public CallWCFAttribute(WCFHeader head1,WCFHeader head2)
        {
            _headers.Add(head1);
            _headers.Add(head2);
        }
        /// <summary>
        /// WCF通信消息头
        /// </summary>
        /// <param name="head1">消息头</param>
        /// <param name="head2">消息头</param>
        /// <param name="head3">消息头</param>
        public CallWCFAttribute(WCFHeader head1, WCFHeader head2, WCFHeader head3)
        {
            _headers.Add(head1);
            _headers.Add(head2);
            _headers.Add(head3);
        }
        
        /// <summary>
        /// WCF通信消息头
        /// </summary>
        /// <param name="heads">多个消息头一起</param>
        public CallWCFAttribute(params WCFHeader[] heads) {
            if (heads != null)
            {
                foreach (var h in heads) {
                    _headers.Add(h);
                }
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            foreach (var h in _headers)
            {
                clientRuntime.MessageInspectors.Add(h);
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="clientOperation"></param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            foreach (var h in _headers)
            {
                clientOperation.Parent.MessageInspectors.Add(h);
            }            
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
        }

        public void Validate(OperationDescription operationDescription)
        {

        }
    }
}
