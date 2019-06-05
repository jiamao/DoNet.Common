using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.Net
{
    /// <summary>
    /// WEB代理处理函数
    /// </summary>
    public static class WebProxyHandler
    {
        static List<Data.ServiceProxyInvokeItem> methodProxyItems = null;

        /// <summary>
        /// 转换请求参数
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        public static DoNet.Common.Data.ServiceProxyParam RequestParamParse(string requestString)
        {
            if (string.IsNullOrWhiteSpace(requestString)) return null;
            var param = DoNet.Common.Serialization.JSon.JsonToModel<DoNet.Common.Data.ServiceProxyParam>(requestString);
            return param;
        }

        /// <summary>
        /// 根据请求参数执行函数
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        public static Data.ServiceProxyReturn Invoke(string requestString, string configpath = "data/proxy.config")
        {
            var param = RequestParamParse(requestString);
            return Invoke(param, configpath);
        }

        /// <summary>
        /// 根据请求参数执行函数
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        public static Data.ServiceProxyReturn Invoke(DoNet.Common.Data.ServiceProxyParam param, string configpath = "data/proxy.config")
        {
            var result = new Data.ServiceProxyReturn();
            try
            {
                if (param == null)
                {
                    throw new Exception("ServiceProxyParam is null");
                }
                var requestpar = CheckInvoke(param, result, configpath);
                if (requestpar == null)
                {
                    return result;
                }

                var type = System.Reflection.Assembly.Load(requestpar.Namespace).GetType(requestpar.ClassName);
                if (type == null) throw new Exception("load " + requestpar.ClassName + " faild.");

                System.Reflection.MethodInfo method = null;
                System.Reflection.ParameterInfo[] methodpars = null;
                var ms = type.GetMethods();
                foreach (var m in ms)
                {
                    var mpars = m.GetParameters();
                    if (m.Name.Equals(requestpar.MethodName, StringComparison.OrdinalIgnoreCase) && mpars.Length == param.Params.Count)
                    {
                        method = m;
                        methodpars = mpars;
                        break;
                    }
                }

                if (method == null) throw new Exception("get method " + requestpar.MethodName + " faild.");
                //var methodpars = method.GetParameters();
                var instance = Activator.CreateInstance(type);

                object[] invokepars = null;
                if (methodpars != null && methodpars.Length > 0)
                {
                    invokepars = new object[methodpars.Length];
                    if (methodpars.Length == param.Params.Count)
                    {
                        for (var i = 0; i < invokepars.Length; i++)
                        {
                            if (i >= param.Params.Count || param.Params[i] == null) continue;
                            var p = param.Params[i];
                            var t = methodpars[i].ParameterType;//t != typeof(string) && 
                            if ((t.IsClass || t.IsArray || t.IsGenericType))
                            {
                                if (t == typeof(string))
                                {
                                    invokepars[i] = p.Value;
                                }
                                else
                                {
                                    invokepars[i] = p.Value == null ? null : DoNet.Common.Serialization.JSon.JsonToModel(t, p.Value);
                                }
                            }
                            else
                            {
                                invokepars[i] = Convert.ChangeType(p.Value, methodpars[i].ParameterType);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("method " + requestpar.MethodName + " params error.");
                    }
                }

                var re = method.Invoke(instance, invokepars);
                if (re != null)
                {
                    var t = re.GetType();
                    if (t != typeof(string) && (t.IsClass || t.IsArray || t.IsGenericType))
                    {
                        result.Value = DoNet.Common.Serialization.JSon.ModelToJson(re);
                    }
                    else
                    {
                        result.Value = re.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.State = 2;
                DoNet.Common.IO.Logger.Write(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 检查请求是否正确
        /// </summary>
        /// <param name="par"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Data.ServiceProxyInvokeItem CheckInvoke(Data.ServiceProxyParam par, Data.ServiceProxyReturn result, string configpath)
        {
            //从配置文件中读取代理的方法配置
            if (methodProxyItems == null)
            {
                var proxyconfigpath = DoNet.Common.IO.PathMg.CheckPath(configpath);
                if (System.IO.File.Exists(proxyconfigpath))
                {
                    methodProxyItems = (List<Data.ServiceProxyInvokeItem>)DoNet.Common.Serialization.FormatterHelper.XMLDerObject(typeof(List<Data.ServiceProxyInvokeItem>), proxyconfigpath);
                }
                else
                {
                    methodProxyItems = new List<Data.ServiceProxyInvokeItem>();
                }
            }
            foreach (var item in methodProxyItems)
            {
                if (item.Key == par.Key || (item.Namespace == par.Namespace && item.ClassName == par.ClassName && item.MethodName == par.Method))
                {
                    var ip = Net.BaseNet.GetWebIPAddress();
                    if (item.IPAddress == "*" || item.IPAddress.Contains(ip))
                    {
                        return item;
                    }
                    else
                    {
                        DoNet.Common.IO.Logger.Write(ip + "，没有被允许请求");
                        result.Error = "ip error";
                        result.State = 3;//非法请求
                        return null;
                    }
                }
            }
            result.Error = "异常的请求，请确保请求参数正确。";
            result.State = 3;//非法请求
            return null;
        }

        /// <summary>
        /// 请求远程服务代理
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static Data.ServiceProxyReturn Request(string url,string key, params object[] pars)
        {
            var requestpar = new Data.ServiceProxyParam();
            requestpar.Key = key;
            requestpar.Params = new List<Data.ServiceProxyMethodParam>();
            if (pars != null)
            {
                foreach (var p in pars)
                {
                    if (p == null) requestpar.Params.Add(new Data.ServiceProxyMethodParam() {  Value = null });
                    else {
                        var t = p.GetType();
                        if (t == typeof(string))
                        {
                            requestpar.Params.Add(new Data.ServiceProxyMethodParam() { Value = p.ToString(), DataType = t.FullName });
                        }
                        else if (t.IsClass || t.IsGenericType || t.IsArray)
                        {
                            var json = Serialization.JSon.ModelToJson(p);
                            requestpar.Params.Add(new Data.ServiceProxyMethodParam() { Value = json, DataType = t.FullName });
                        }
                        else
                        {
                            requestpar.Params.Add(new Data.ServiceProxyMethodParam() { Value = p.ToString(), DataType = t.FullName });
                        }
                    }
                }
            }

            var request = new System.Net.WebClient();
            request.Encoding = System.Text.Encoding.UTF8;

            var requestdata = Serialization.JSon.ModelToJson(requestpar);
            var r = request.UploadString(url,"post",requestdata);
            var result = Serialization.JSon.JsonToModel<Data.ServiceProxyReturn>(r);
            return result;
        }
    }
}
