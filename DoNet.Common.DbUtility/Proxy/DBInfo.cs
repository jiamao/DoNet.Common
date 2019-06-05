using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.DbUtility.Proxy
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class DBInfo
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        [DataMember]
        public DbType DBType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        [DataMember]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否为代理访问
        /// </summary>
        [DataMember]
        public bool IsProxy { get; set; }

        /// <summary>
        /// 代理地址
        /// </summary>
        [DataMember]
        public string ProxyUrl { get; set; }
    }
}
