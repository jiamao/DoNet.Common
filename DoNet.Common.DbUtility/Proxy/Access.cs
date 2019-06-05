using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.DbUtility.Proxy
{
    /// <summary>
    /// 数据库代理
    /// </summary>
    internal static class Access
    {
        /// <summary>
        /// 生成远程代理
        /// </summary>
        /// <returns></returns>
        public static IDBHelper CreateProxy()
        {
            //var proxy = DoNet.Common.Net.WCFManager.Client.CreateRemoteObject<IDBHelper>("DBProxy");
            //return proxy;
            return new DBRequester();
        }
    }
}
