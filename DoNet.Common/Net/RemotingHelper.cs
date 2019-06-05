//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : Remoting操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;
using System.Globalization;

namespace DoNet.Common.Net
{
    public class RemoteObjectManager
    {

        #region 客户端RemoteConfig
        /// <summary>
        /// 远程对象代理设置
        /// </summary>
        static Dictionary<string, string> m_LocalObjectConfig = null;
        public static Dictionary<string, string> LocalObjectConfig
        {
            get
            {
                if (m_LocalObjectConfig == null || m_LocalObjectConfig.Count == 0)
                {
                    string configpath = System.Configuration.ConfigurationManager.AppSettings["LocalObjectConfig"];
                    if (string.IsNullOrEmpty(configpath))
                    {
                        configpath = IO.PathMg.CheckPath("config\\LocalObjectConfig.xml");
                    }

                    configpath = IO.PathMg.CheckPath(configpath);

                    IO.XmlHelper xmlwr = new IO.XmlHelper(configpath);
                    m_LocalObjectConfig = xmlwr.TurnXmlToDictionary(true);
                }
                return RemoteObjectManager.m_LocalObjectConfig;
            }
            set { RemoteObjectManager.m_LocalObjectConfig = value; }
        }
        /// <summary>
        /// a获取配置值
        /// </summary>
        /// <param name="itempath"></param>
        /// <returns></returns>
        public static string GetObjectConfig(string itempath)
        {
            itempath = itempath.ToLower(CultureInfo.CurrentCulture);
            return LocalObjectConfig[itempath];
        }





        //已生成代理的远程对象
        //static Dictionary<string, object> m_RemoteObject = new Dictionary<string, object>();

        /// <summary>
        /// 获取远程代理对象
        /// </summary>
        /// <param name="ObjectName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object CreateRemoteObject(string ObjectName, Type t)
        {
            try
            {
                string objecturi = GetObjectConfig("LocalConfig/RemoteObject/" + ObjectName + "/Uri/value");//代理对象的标识
                string objip = GetObjectConfig("LocalConfig/RemoteObject/" + ObjectName + "/NetAddress/value");//代理对象的IP地址
                string objport = GetObjectConfig("LocalConfig/RemoteObject/" + ObjectName + "/Port/value");//代理对象的端口
                string objprotocol = GetObjectConfig("LocalConfig/RemoteObject/" + ObjectName + "/Protocol/value");//代理对象的协议

                string uri = string.Format("{0}://{1}:{2}/{3}", objprotocol, objip, objport, objecturi);

                object obj = !string.IsNullOrEmpty(objprotocol) && objprotocol.ToLower(CultureInfo.CurrentCulture).Trim() == "http" ?
                             RemotingClient.GetHttpObject(t, objip, int.Parse(objport), objecturi) :
                             RemotingClient.GetTcpObject(t, objip, int.Parse(objport), objecturi);//获取远程对象

               
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 动态加载本地对象
        /// </summary>
        /// <param name="ObjectName"></param>
        /// <returns></returns>
        public static object CreateLocalObject(string ObjectName)
        {
            try
            {
                string objpath = GetObjectConfig("LocalConfig/LocalObject/" + ObjectName + "/Path/value");//对象路径
                string objclass = GetObjectConfig("LocalConfig/LocalObject/" + ObjectName + "/ClassName/value");//对象类名 

                objpath = IO.PathMg.CheckPath(objpath);
                object obj = Reflection.ClassHelper.GetClassObject(objpath, objclass);//\动态加载本地对象

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取本地属性
        /// </summary>
        /// <param name="ObjectName"></param>
        /// <returns></returns>
        public static object CreateLocalPorperty(string ObjectName)
        {
            string dllpath = GetObjectConfig("LocalConfig/LocalObject/" + ObjectName + "/Path/value");//对象路径
            string classname = GetObjectConfig("LocalConfig/LocalObject/" + ObjectName + "/ClassName/value");//对象类名          
            string isstatic = RemoteObjectManager.GetRemoteServerValue("LocalConfig/LocalObject/" + ObjectName + "/isStatic");
            string property = RemoteObjectManager.GetRemoteServerValue("LocalConfig/LocalObject/" + ObjectName + "/Property/value");
            return Reflection.ClassHelper.GetPropertyValue(Reflection.ClassHelper.GetClassObject(dllpath, classname), property,
                !string.IsNullOrEmpty(isstatic) && isstatic.ToLower(CultureInfo.CurrentCulture).Trim() == "true");
        }

        #endregion

        #region 服务端Remote配置
        /// <summary>
        /// 远程代理设置
        /// </summary>
        static Dictionary<string, string> m_RemoteObjectConfig = null;

        public static Dictionary<string, string> RemoteObjectConfig
        {
            get
            {
                if (m_RemoteObjectConfig == null || m_RemoteObjectConfig.Count == 0)
                {
                    string configpath = System.Configuration.ConfigurationManager.AppSettings["RemoteObjectConfig"];
                    if (string.IsNullOrEmpty(configpath))
                    {
                        configpath = System.IO.Path.Combine(IO.PathMg.GetRootPath(), "config\\RemoteObjectConfig.xml");
                    }
                    IO.XmlHelper xmlwr = new IO.XmlHelper(configpath);
                    m_RemoteObjectConfig = xmlwr.TurnXmlToDictionary(true);
                }
                return RemoteObjectManager.m_RemoteObjectConfig;
            }
            set { RemoteObjectManager.m_RemoteObjectConfig = value; }
        }
        /// <summary>
        /// 获取远程对象配置值
        /// </summary>
        /// <param name="itempath"></param>
        /// <returns></returns>
        public static string GetRemoteValue(string itempath)
        {
            itempath = itempath.ToLower(CultureInfo.CurrentCulture);

            if (RemoteObjectConfig.ContainsKey(itempath))
            {
                return RemoteObjectConfig[itempath];
            }
            else
            {
                return "";
            }
        }

        static Dictionary<string, string> m_RemoteServerConfig = new Dictionary<string, string>();//服务端配置

        public static Dictionary<string, string> RemoteServerConfig
        {
            get
            {
                if (m_RemoteServerConfig.Count == 0)
                {
                    string configpath = System.Configuration.ConfigurationManager.AppSettings["RemoteConfig"];
                    if (string.IsNullOrEmpty(configpath))
                    {
                        configpath = System.IO.Path.Combine(IO.PathMg.GetRootPath(), "config\\RemotingConfig.xml");
                    }
                    IO.XmlHelper xmlwr = new IO.XmlHelper(configpath);
                    m_RemoteServerConfig = xmlwr.TurnXmlToDictionary(true);//读取配置
                }
                return m_RemoteServerConfig;
            }
            set { RemoteObjectManager.m_RemoteServerConfig = value; }
        }
        /// <summary>
        /// 获取SERVER配置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRemoteServerValue(string path)
        {
            path = path.ToLower(CultureInfo.CurrentCulture);
            if (RemoteServerConfig.ContainsKey(path))
                return RemoteServerConfig[path];
            else return "";
        }
        /// <summary>
        /// 获取所有需要注册类前缀
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllRegisterTypes()
        {
            List<string> list = new List<string>();
            foreach (string key in RemoteServerConfig.Keys)
            {
                if (key.ToLower(CultureInfo.CurrentCulture).StartsWith("remotingconfig/registerwellknownservicetype"))
                {
                    if (key.ToLower(CultureInfo.CurrentCulture).Replace("remotingconfig/registerwellknownservicetype/", "").Contains("/register") && m_RemoteServerConfig[key] == "true")
                    {
                        list.Add(key.Substring(0, key.LastIndexOf("/")));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 注册远程代理对象
        /// </summary>
        public static void RegisterObject(RemotingServer remotingserver)
        {
            List<string> alltypepaths = RemoteObjectManager.GetAllRegisterTypes();//获取所有要注册的类前缀路径

            foreach (string beforekey in alltypepaths)
            {
                string regist = RemoteObjectManager.GetRemoteServerValue(beforekey + "/Register");
                if (!string.IsNullOrEmpty(regist) && regist.ToLower(CultureInfo.CurrentCulture).Trim() == "false") continue;
                string dllpath = RemoteObjectManager.GetRemoteServerValue(beforekey + "/" + "Path/value");
                string classname = RemoteObjectManager.GetRemoteServerValue(beforekey + "/" + "ClassName/value");
                string uriname = RemoteObjectManager.GetRemoteServerValue(beforekey + "/" + "UriName/value");
                string wellmode = RemoteObjectManager.GetRemoteServerValue(beforekey + "/" + "WellKnownObjectMode/value");
                string isstatic = RemoteObjectManager.GetRemoteServerValue(beforekey + "/isStatic");

                string typeinfo = "";
                dllpath = IO.PathMg.CheckPath(dllpath);

                if (!string.IsNullOrEmpty(isstatic) && isstatic.ToLower(CultureInfo.CurrentCulture).Trim() == "true")
                {
                    string property = RemoteObjectManager.GetRemoteServerValue(beforekey + "/" + "Property/value");
                    typeinfo = remotingserver.RegisteRemoteStaticObject(dllpath, classname, property, uriname);//设置代理类
                }
                else
                {
                    typeinfo = remotingserver.RegisteRemoteObject(dllpath, classname, uriname, wellmode);//设置代理类
                }

                //Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("------------------------------");               
            }
        }

        /// <summary>
        /// 注册远程代理对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="remotingserver"></param>
        /// <param name="uriname"></param>
        /// <param name="wellmode"></param>
        public static void RegisterObject<T>(RemotingServer remotingserver, string uriname, string wellmode)
        {

            string typeinfo = remotingserver.RegisteRemoteObject(typeof(T), uriname, wellmode);//设置代理类

            //Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------");            

        }

        /// <summary>
        /// 注册广播对象
        /// </summary>
        /// <param name="remotingserver"></param>
        /// <param name="obj"></param>
        /// <param name="uriName"></param>
        public static void RegisteRemoteStaticObject(RemotingServer remotingserver, object obj, string uriName)
        {
            string re = remotingserver.RegisteRemoteStaticObject(null, obj as MarshalByRefObject, uriName);
            //Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------");            
        }
        #endregion
    }

    /// <summary>
    /// 服务端类
    /// </summary>
    [Serializable]
    [SecurityPermission(SecurityAction.LinkDemand)]
    public class RemotingServer
    {

        public enum ChannelType
        {
            Tcp,
            Http
        }

        int m_iPort;//端口        

        public int IPort
        {
            get { return m_iPort; }
            set { m_iPort = value; }
        }
        string m_sChannelName;//信道名称

        public string SChannelName
        {
            get { return m_sChannelName; }
            set { m_sChannelName = value; }
        }

        ChannelType m_sChannelType;//信道种类

        public ChannelType SChannelType
        {
            get { return m_sChannelType; }
            set { m_sChannelType = value; }
        }

        IChannel m_ServerChannel = null;
        /// <summary>
        /// 当前信道
        /// </summary>
        public IChannel ServerChannel
        {
            get { return m_ServerChannel; }
            set { m_ServerChannel = value; }
        }

        #region 构造函数

        public RemotingServer()
        { }
        /// <summary>
        /// 初始化类
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="classname">类名</param>

        /// <param name="mode">single表示为每个对象使用同一个实例,否则为每个对象提供单独的实例</param>
        /// <param name="channelname">信道名称</param>
        public RemotingServer(int port, string channelname, ChannelType channeltype)
        {
            m_iPort = port;
            m_sChannelName = channelname;
            m_sChannelType = channeltype;
        }

        #endregion

        /// <summary>
        /// 开始服务
        /// </summary>
        public string StartServer()
        {
            if (m_sChannelType == ChannelType.Tcp)
            {
                return this.tcpserverstart();//如果是TCP则起动tcp 信道服务
            }
            else
            {
                return this.httpserverstart();//如果不是则起动http信道服务
            }
        }

        /// <summary>
        /// 开始服务
        /// </summary>
        public string StartServer(int port, string channelname, ChannelType channeltype)
        {
            m_iPort = port;
            m_sChannelName = channelname;
            m_sChannelType = channeltype;

            if (m_sChannelType == ChannelType.Tcp)
            {
                return this.tcpserverstart();//如果是TCP则起动tcp 信道服务
            }
            else
            {
                return this.httpserverstart();//如果不是则起动http信道服务
            }
        }

        /// <summary>
        /// tcp服务器
        /// </summary>
        /// <returns>执行信息</returns>
        private string tcpserverstart()
        {
            try
            {
                StringBuilder strinfo = new StringBuilder();
                System.Collections.IDictionary dict = new System.Collections.Hashtable();//一个 IDictionary 集合,为客户端和服务器信道要使用的配置属性指定值
                if (!string.IsNullOrEmpty(m_sChannelName)) dict["name"] = m_sChannelName;

                dict["port"] = m_iPort;
                // dict["authenticationMode"] = "IdentifyCallers";

                LifetimeServices.LeaseTime = TimeSpan.Zero;

                BinaryServerFormatterSinkProvider bsf = new BinaryServerFormatterSinkProvider();//为服务器二进制格式化接受器提供实现
                BinaryClientFormatterSinkProvider bcf = new BinaryClientFormatterSinkProvider();//为客服端二进制格式化接受器提供实现
                bsf.TypeFilterLevel = TypeFilterLevel.Full;

                m_ServerChannel = new TcpChannel(dict, bcf, bsf);//提供使用 TCP 协议传输消息的信道实现
                //TcpServerChannel tcl = new TcpServerChannel(port);
                //tcl.IsSecured = true;//设置当前信道为安全信道

                //RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;

                ChannelServices.RegisterChannel(m_ServerChannel, false);//向信道服务注册信道。

                strinfo.AppendLine("当前信道的名称： " + m_ServerChannel.ChannelName.ToString());
                strinfo.AppendLine("当前信道的优先级： " + m_ServerChannel.ChannelPriority.ToString());

                //tcl.StartListening(null);//监听
                ChannelDataStore data = (ChannelDataStore)((TcpChannel)m_ServerChannel).ChannelData;

                foreach (string strurl in data.ChannelUris)
                {
                    strinfo.AppendLine("当前信道所映射到的信道 URI： " + strurl);
                }

                //strinfo.AppendLine(string.Format("当前信道安全与否 {0}", tcl.IsSecured));
                return strinfo.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// http服务器
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="classname">类名</param>
        /// <param name="classtype">类类型</param>
        /// <param name="mode">single表示为每个对象使用同一个实例,否则为每个对象提供单独的实例</param>
        /// <returns>执行信息</returns>
        private string httpserverstart()
        {
            try
            {
                StringBuilder strinfo = new StringBuilder();

                System.Collections.IDictionary dict = new System.Collections.Hashtable();//一个 IDictionary 集合,为客户端和服务器信道要使用的配置属性指定值
                if (!string.IsNullOrEmpty(m_sChannelName)) dict["name"] = m_sChannelName;
                dict["port"] = m_iPort;
                //dict["authenticationMode"] = "IdentifyCallers";
                SoapServerFormatterSinkProvider bsf = new SoapServerFormatterSinkProvider();//为服务器SOAP格式化接受器提供实现
                //BinaryClientFormatterSinkProvider bcf = new BinaryClientFormatterSinkProvider();//为客服端二进制格式化接受器提供实现
                bsf.TypeFilterLevel = TypeFilterLevel.Full;
                m_ServerChannel = new HttpServerChannel(dict, bsf);//提供使用 Http 协议传输消息的信道实现
                //Hcl.IsSecured = true;//设置当前信道为安全
                //m_ServerChannel.WantsToListen = true;//挂钩到外部针听端口
                LifetimeServices.LeaseTime = TimeSpan.Zero;
                ChannelServices.RegisterChannel(m_ServerChannel, false);//向信道服务注册信道。   

                strinfo.AppendLine("当前信道的名称： " + m_ServerChannel.ChannelName.ToString());
                strinfo.AppendLine("当前信道的优先级： " + m_ServerChannel.ChannelPriority.ToString());

                RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;

                // Hcl.ChannelScheme = "http";//要挂钩到监听的类型
                //m_ServerChannel.StartListening(null);//监听
                ChannelDataStore data = (ChannelDataStore)((HttpServerChannel)m_ServerChannel).ChannelData;

                foreach (string strurl in data.ChannelUris)
                {
                    strinfo.AppendLine("当前信道所映射到的信道 URI： " + strurl);
                }

                // strinfo.AppendLine(string.Format("当前信道安全与否 {0}", Hcl.IsSecured));
                return strinfo.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 注册远程代理
        /// </summary>
        /// <param name="dllpath"></param>
        /// <param name="classname"></param>
        /// <param name="uriName"></param>
        /// <param name="m_sMode"></param>
        /// <returns></returns>
        public string RegisteRemoteObject(string dllpath, string classname, string uriName, string m_sMode)
        {
            Type t = Common.Reflection.ClassHelper.GetClassType(dllpath, classname);
            return RegisteRemoteObject(t, uriName, m_sMode);
        }
        /// <summary>
        /// 注册远程代理
        /// </summary>
        /// <param name="t"></param>
        /// <param name="uriName"></param>
        /// <param name="m_sMode"></param>
        /// <returns></returns>
        public string RegisteRemoteObject(Type t, string uriName, string m_sMode)
        {
            StringBuilder strinfo = new StringBuilder("注册代理对象：" + t.FullName + "\r\n");

            //将服务端上的对象 Type 注册为已知类型
            WellKnownServiceTypeEntry wkste;
            if (!string.IsNullOrEmpty(m_sMode) && !(m_sMode.ToLower(CultureInfo.CurrentCulture).Trim() == "singleton"))
            {
                wkste = new WellKnownServiceTypeEntry(t, uriName, WellKnownObjectMode.Singleton);
                // RemotingConfiguration.RegisterWellKnownServiceType(classtype, url, WellKnownObjectMode.SingleCall);
                strinfo.AppendLine("代理方式：每个对象使用同一个实例。");
            }
            else
            {
                wkste = new WellKnownServiceTypeEntry(t, uriName, WellKnownObjectMode.SingleCall);
                //RemotingConfiguration.RegisterWellKnownServiceType(classtype, url, WellKnownObjectMode.Singleton);
                strinfo.AppendLine("代理方式：为每个对象提供单独的实例。");
            }

            RemotingConfiguration.RegisterWellKnownServiceType(wkste);

            string[] urls = m_sChannelType == ChannelType.Tcp ? ((TcpChannel)m_ServerChannel).GetUrlsForUri(uriName) : ((HttpServerChannel)m_ServerChannel).GetUrlsForUri(uriName);

            if (urls.Length > 0)
            {
                foreach (string objectUrl in urls)
                {
                    string objectUri;
                    string channelUri = m_ServerChannel.Parse(objectUrl, out objectUri);
                    strinfo.AppendLine(string.Format("远程已知对象的 URI： {0}.", objectUrl));
                    strinfo.AppendLine(string.Format("远程已知对象的 URI： {0}.", objectUri));
                    strinfo.AppendLine(string.Format("当前信道的 URI： {0}.", channelUri));
                }
            }

            //var oco = Console.ForegroundColor;
            //Console.ForegroundColor = ConsoleColor.Green;
            string info = strinfo.ToString();
            Console.WriteLine(info);
            //Console.ForegroundColor = oco;
            return info;
        }

        /// <summary>
        /// 注册服务端对象，例如广播对象
        /// </summary>
        /// <param name="t">对象类型</param>
        /// <param name="obj">对象</param>
        /// <param name="uriName">注册名</param>
        /// <returns></returns>
        public string RegisteRemoteStaticObject(string dllpath, string classname, string property, string uriName)
        {
            MarshalByRefObject obj = Common.Reflection.ClassHelper.GetPropertyValue(Common.Reflection.ClassHelper.GetClassObject(dllpath, classname), 
                property, true) as MarshalByRefObject;
            return RegisteRemoteStaticObject(obj.GetType(), obj, uriName);
        }

        /// <summary>
        /// 注册服务端对象，例如广播对象
        /// </summary>
        /// <param name="t">对象类型</param>
        /// <param name="obj">对象</param>
        /// <param name="uriName">注册名</param>
        /// <returns></returns>
        public string RegisteRemoteStaticObject(Type t, MarshalByRefObject obj, string uriName)
        {
            string result = "注册远程广播对象：" + uriName + "\r\n";
            ObjRef objref = RemotingServices.Marshal(obj, uriName, t);//注册服务端对象

            result += "远程已知对象的 URI：" + objref.URI;

            return result;
        }
    }
    /// <summary>
    /// remotingclient
    /// </summary>
    [Serializable]
    public class RemotingClient
    {
        static bool _ChannelRegistered = false;
        /// <summary>
        /// 如果使用广播需求设置
        /// </summary>
        public static void SetChannel()
        {
            if (_ChannelRegistered) return;
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            LifetimeServices.LeaseTime = TimeSpan.Zero;
            IDictionary props = new Hashtable();
            props["port"] = 0;
            TcpChannel channel = new TcpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(channel, false);
            _ChannelRegistered = true;
        }

        /// <summary>
        /// tcpremoting
        /// </summary>
        /// <param name="classtype">类类型</param>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="classname">类名</param>
        /// 
        public static object GetTcpObject(Type classtype, string ip, int port, string objUri)
        {
            string url = string.Format("tcp://{0}:{1}/{2}", ip.Trim(), port, objUri.Trim());

            try
            {
                return Activator.GetObject(classtype, url);//返回代理类

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// httpclient
        /// </summary>
        /// <param name="classtype">类类型</param>
        /// <param name="ip">ＩＰ地址</param>
        /// <param name="port">端口</param>
        /// <param name="classname">类名</param>
        /// <returns>实类</returns>
        public static Object GetHttpObject(Type classtype, string ip, int port, string objUri)
        {
            string url;
            url = string.Format("http://{0}:{1}/{2}", ip.Trim(), port, objUri.Trim());

            return Activator.GetObject(classtype, url);

        }
        /// <summary>
        /// 得到指定的信道
        /// </summary>
        /// <param name="channelname">信道的名称</param>
        /// <returns></returns>
        public IChannel getchannel(string channelname)
        {
            return ChannelServices.GetChannel(channelname);
        }
        /// <summary>
        /// 得到所有已注册的信道
        /// </summary>
        /// <returns>当前信道数组</returns>
        public static IChannel[] getchannels()
        {
            return ChannelServices.RegisteredChannels;
        }
        /// <summary>
        /// 注销指定的信道
        /// </summary>
        /// <param name="ic">信道</param>
        public static bool unregchannel(IChannel ic)
        {
            try
            {

                ChannelServices.UnregisterChannel(ic);//注销
                return true;
            }
            //注销失败
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }       
    }
}
