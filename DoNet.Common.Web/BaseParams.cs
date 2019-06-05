using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DoNet.Common.Web
{
    /// <summary>
    /// 基础参数
    /// </summary>
    public class BaseParams
    {
        /// <summary>
        /// WEB根路径标识
        /// </summary>
        public const string WebRootMark = "~";

        /// <summary>
        /// 当前绝对的根路径
        /// </summary>
        public static string RootUrl
        {
            get
            {               
                var scheme = HttpContext.Current.Request.Url.Scheme;
                var _rootUrl = string.Format("{0}://{1}{2}", scheme, Host, HttpContext.Current.Request.ApplicationPath).TrimEnd('/');

                return _rootUrl;
            }
        }

        /// <summary>
        /// 返回没有协议的url地址,例如: //jm47.com
        /// </summary>
        public static string NoSchemeRootUrl
        {
            get
            {
                var _rootUrl = string.Format("//{0}{1}", Host, HttpContext.Current.Request.ApplicationPath).TrimEnd('/');

                return _rootUrl;
            }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public static string Host
        {
            get {
                var host = HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 80 ? (":" + HttpContext.Current.Request.Url.Port) : "");
                if (HttpContext.Current.Request.Headers.AllKeys.Contains<string>("X-Forwarded-Host"))
                {
                    host = HttpContext.Current.Request.Headers.Get("X-Forwarded-Host");
                }
                else if (HttpContext.Current.Request.Headers.AllKeys.Contains<string>("X-Forwarded-Server"))
                {
                    host = HttpContext.Current.Request.Headers.Get("X-Forwarded-Server");
                }
                return host;
            }
        }

        static string _rootphysicalpath;
        /// <summary>
        /// WEB的物理根路径
        /// </summary>
        public static string RootPhysicalpath
        {
            get {
                if (string.IsNullOrWhiteSpace(_rootphysicalpath))
                {
                    _rootphysicalpath = HttpContext.Current.Server.MapPath(WebRootMark);
                }
                return _rootphysicalpath;
            }
        }
    }
}
