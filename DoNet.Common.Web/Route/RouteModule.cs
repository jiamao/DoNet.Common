using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;

namespace DoNet.Common.Web.Route
{
    /// <summary>
    /// 实现asp.net路由httpmodule
    /// </summary>
    public class RouteModule:IHttpModule
    {
        //保证线程安全
        static object _syncLock = new object();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            //绑定请求启始事件
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        /// <summary>
        /// 请求开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void context_BeginRequest(object sender, EventArgs e)
        {
            var context = sender as HttpApplication;
            if (context == null) return;

            Rewrite(context);
        }        

        //记录已匹配置的,如果下次再请求则直接跳 转
        Dictionary<string, string> _routeResult = new Dictionary<string, string>();

        /// <summary>
        /// 重写URL路径
        /// </summary>
        /// <param name="context"></param>
        protected void Rewrite(HttpApplication context)
        {
            var path = context.Request.Url.PathAndQuery;
            //var application = context.Request.ApplicationPath.TrimEnd('/');
            //if(application.Length > 0)
            //    path = path.Substring(application.Length);

            //如果已经跳转过则直接跑转.不再重新处理
            if (_routeResult.ContainsKey(path))
            {
                var target = _routeResult[path];
                //如果没有变更路径,则无需跳转
                if (!target.Equals(path, StringComparison.OrdinalIgnoreCase))
                {                    
                    //输出debug记录
                    System.Diagnostics.Debug.WriteLine("rewrite {0} to {1}", path, target);
                    //跳转到目标路径
                    context.Context.RewritePath(target,true);
                }
                return;
            }

            var routeConfigs = RouteConfig.Instance.ReadConfig();//读取配置

            if (routeConfigs != null && routeConfigs.Count > 0)
            {
                var target = "";

                foreach (var con in routeConfigs)
                { 
                    //如果只是替换规则，则直接替换
                    if (con.RouteType == RouteItem.EnumRouteType.Replace && con.Reg.IsMatch(context.Request.Url.AbsoluteUri))
                    {
                        target = con.Reg.Replace(context.Request.Url.AbsoluteUri, con.Url);//替换
                    }
                }

                //如果有替换，则直接重定向
                if (!string.IsNullOrWhiteSpace(target))
                {
                    DoNet.Common.IO.Logger.Debug("route.replace:" , path , "->" + target);
                    context.Response.Redirect(target);
                    return;
                }

                foreach (var con in routeConfigs)
                {
                    if (con.RouteType == RouteItem.EnumRouteType.Replace || !con.Reg.IsMatch(path)) continue;

                    if (con.RouteType == RouteItem.EnumRouteType.Route)
                    {
                        target = con.Url;//跳转目标

                        foreach (Match m in con.Reg.Matches(path))
                        {
                            foreach (var gn in con.Reg.GetGroupNames())
                            {
                                if (gn == "0") continue;
                                target = target.Replace(string.Format("<{0}>", gn), m.Groups[gn].Value);
                            }
                        }
                    }        
                }
    
                //记录此次跳转为本路径
                //以便下次访问时重复处理
                if (!_routeResult.ContainsKey(path)) {
                    //保证线程安全访问
                    lock (_syncLock)
                    {
                        if (!_routeResult.ContainsKey(path)) 
                            _routeResult.Add(path, string.IsNullOrWhiteSpace(target)?path:target);
                    }
                }

                if (!string.IsNullOrWhiteSpace(target))
                {
                    //输出debug记录
                    System.Diagnostics.Debug.WriteLine("rewrite {0} to {1}", path, target);
                    DoNet.Common.IO.Logger.Debug("route.rewrite:", path, "->" + target);

                    //跳转到目标路径
                    context.Context.RewritePath(target,true);

                    //context.Context.Response.(
                }             
            }
                
          
        }

        /// <summary>
        /// 终结
        /// </summary>
        public void Dispose()
        {
            this._routeResult.Clear();
        }
    }
}
