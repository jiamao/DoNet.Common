using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web;

namespace DoNet.Common.Web.Route
{
    /// <summary>
    /// 路由配置
    /// </summary>
    public sealed class RouteConfig
    {
        static RouteConfig _instance = new RouteConfig();
        /// <summary>
        /// 路由配置
        /// </summary>
        public static RouteConfig Instance {
            get {
                return _instance;
            }
        }

        string _configPath;
        /// <summary>
        /// 获取路由配置文件地址
        /// </summary>
        protected string RouteConfigPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_configPath))
                {
                    var tmp = ConfigurationManager.AppSettings["RouteConfigPath"];
                    if (!string.IsNullOrWhiteSpace(tmp))
                    {
                        _configPath = tmp;
                    }
                    else
                    {
                        _configPath = "route.config";
                    }

                    //获取绝对路径
                    _configPath = HttpContext.Current.Server.MapPath("~/" + _configPath);
                }
                return _configPath;
            }
        }

        List<RouteItem> _routeConfig = null;
        DateTime _configLastModified = DateTime.MinValue;
        /// <summary>
        /// 读取路由配置
        /// </summary>
        /// <returns></returns>
        public List<RouteItem> ReadConfig()
        {
            
                try
                {
                    if (System.IO.File.Exists(RouteConfigPath))
                    {
                        var lastModified = System.IO.File.GetLastWriteTime(RouteConfigPath);
                        if (_routeConfig == null || _routeConfig.Count == 0 || lastModified != _configLastModified) {
                            _routeConfig = new List<RouteItem>();
                            var dom = new System.Xml.XmlDocument();
                            dom.Load(RouteConfigPath);//加载XML文档
                            var nodes = dom.SelectNodes("Items/Item");
                            foreach (System.Xml.XmlNode xn in nodes)
                            {
                                RouteItem routeItem = new RouteItem();

                                //读取路由方式
                                var routeTypeTmp = ReadXmlAttributeValue(xn, "type", RouteItem.EnumRouteType.Route.ToString());
                                RouteItem.EnumRouteType routeType;
                                Enum.TryParse<RouteItem.EnumRouteType>(routeTypeTmp, true, out routeType);
                                routeItem.RouteType = routeType;

                                foreach (System.Xml.XmlNode item in xn.ChildNodes)
                                {
                                    if (item.NodeType != System.Xml.XmlNodeType.Element) continue;

                                    if (item.Name.Equals("reg", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        var v = ReadXmlAttributeValue(item, "value");
                                        if (string.IsNullOrWhiteSpace(v)) break;//如果表达式为空，则直接跳出
                                        v = v.Replace("/", "\\/");//把/转义
                                        routeItem.Reg = new System.Text.RegularExpressions.Regex(v, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                    }
                                    else if (item.Name.Equals("url", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        routeItem.Url = ReadXmlAttributeValue(item, "value");
                                    }
                                }
                                if (routeItem.Reg != null && !string.IsNullOrWhiteSpace(routeItem.Url))
                                {
                                    _routeConfig.Add(routeItem);
                                }
                            }
                            _configLastModified = lastModified;
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    DoNet.Common.IO.Logger.Write(ex.ToString());
                }

            if (_routeConfig == null) _routeConfig =  new List<RouteItem>();
            return _routeConfig;
        }

        /// <summary>
        /// 读取XML节点的属性值
        /// </summary>
        /// <param name="xn"></param>
        /// <param name="attrName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string ReadXmlAttributeValue(System.Xml.XmlNode xn, string attrName, string defaultValue = "")
        {
            var xv = xn.Attributes[attrName];
            if (xn.Attributes != null && xv != null)
            {
                var v = xv.Value;
                if (!string.IsNullOrWhiteSpace(v))
                {
                    v = v.Replace('{', '<').Replace('}', '>').Replace('#', '&');//转换为正则表达式配匹项
                    return v;
                }
            }
            return defaultValue;
        }
    }

    /// <summary>
    /// 配置路由项
    /// </summary>
    public class RouteItem
    {
        /// <summary>
        /// 寻路方式
        /// </summary>
        public enum EnumRouteType
        {
            /// <summary>
            /// 标准路由方式
            /// </summary>
            Route=0,

            /// <summary>
            /// 直接替换字符串方式
            /// </summary>
            Replace=1
        }
        /// <summary>
        /// 正则字符串
        /// </summary>
        public System.Text.RegularExpressions.Regex Reg { get; set; }

        /// <summary>
        /// 替换的目标地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 替换路径方式
        /// </summary>
        public EnumRouteType RouteType { get; set; }
    }
}
