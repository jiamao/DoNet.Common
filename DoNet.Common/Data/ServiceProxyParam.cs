using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 服务代理请求参数
    /// </summary>
    [DataContract]
    [Serializable]
    public class ServiceProxyParam
    {
        [DataMember]
        public ServiceProxyParamHeader Header { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Namespace { get; set; }

        [DataMember]
        public string ClassName { get; set; }

        [DataMember]
        public string Method { get; set; }

        [DataMember]
        public List<ServiceProxyMethodParam> Params { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ServiceProxyParamHeader
    {
        [DataMember]
        public string SessionId { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ServiceProxyMethodParam
    {
        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// 代理请求返回值
    /// </summary>
    [DataContract]
    [Serializable]
    public class ServiceProxyReturn
    {
        [DataMember]
        public int State { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// 被代理对象配置项
    /// </summary>
    [DataContract]
    [Serializable]
    public class ServiceProxyInvokeItem
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Namespace { get; set; }

        [DataMember]
        public string ClassName { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public string IPAddress { get; set; }
    }
}
