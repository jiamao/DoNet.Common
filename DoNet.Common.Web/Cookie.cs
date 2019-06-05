/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/1 15:26:04
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DoNet.Common.Web
{
    /// <summary>
    /// 基础cookie操作类
    /// </summary>
    public sealed class Cookie
    {
        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <param name="key">cookie的健值</param>
        /// <returns></returns>
        public static string Read(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            var cok = HttpContext.Current.Request.Cookies.Get(key);
            return cok!= null?cok.Value:string.Empty;
        }

        /// <summary>
        /// 写cookie
        /// </summary>
        /// <param name="key">关健词</param>
        /// <param name="value">值</param>
        /// <param name="timeout">超时设置（分钟）</param>
        /// <param name="domain">域</param>
        public static void Write(string key, string value, int timeout,string domain="")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            var cok = HttpContext.Current.Request.Cookies.Get(key);
            if (cok == null)
            {
                cok = new HttpCookie(key);                
            }

            if (!string.IsNullOrWhiteSpace(domain)) cok.Domain = domain;

            cok.Expires = DateTime.Now.AddMinutes(timeout);
            cok.Value = value;

            HttpContext.Current.Response.SetCookie(cok);
        }
    }
}
