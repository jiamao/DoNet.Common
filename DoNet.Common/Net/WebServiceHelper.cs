//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : webservice操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Net;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Web.Services.Discovery;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace DoNet.Common.Net
{
    public static class WebServiceHelper
    {
        //缓存服务实例
        static Dictionary<string, object> ServiceCache = new Dictionary<string, object>();

        /// <summary>
        /// 动态调用WebService
        /// </summary>
        /// <param name="url">WebService地址</param>
        /// <param name="classname">类名</param>
        /// <param name="methodname">方法名(模块名)</param>
        /// <param name="args">参数列表</param>
        /// <returns>object</returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            var cachekey = url + "." + classname;
            object obj = ServiceCache.ContainsKey(cachekey) ? ServiceCache[cachekey] : null;
            if (obj == null)
            {
                lock (ServiceCache)
                {
                    if (!ServiceCache.ContainsKey(cachekey))
                    {
                        obj = GetWebServiceClassObject(url, classname);
                        ServiceCache.Add(cachekey, obj);
                    }
                    else
                    {
                        obj = ServiceCache[cachekey];
                    }
                }
            }

            Type t = obj.GetType();
            System.Reflection.MethodInfo mi = t.GetMethod(methodname);//通过反射获得方法名
            return mi.Invoke(obj, args);//使用制定的参数调用当前实例所表示的方法，执行方法
        }
        /// <summary>
        /// 获取webService远程对象
        /// </summary>
        /// <param name="url"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static object GetWebServiceClassObject(string url, string classname)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            if (string.IsNullOrWhiteSpace(classname))
            {
                classname = GetClassName(url);
            }

            if (!url.EndsWith("?WSDL", StringComparison.OrdinalIgnoreCase))
            {
                url = url + "?WSDL";
            }
            ///动态调用类所执行的过程
            //1.获取服务描述语言(WSDL)
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url);
            ServiceDescription sd = ServiceDescription.Read(stream);//设置Web服务描述语言
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();//生成客户端代理类
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);//声明命名空间
            //2.生成客户端代理类代码
            CodeCompileUnit ccu = new CodeCompileUnit();//为CodeDOM程序图形提供容器
            ccu.Namespaces.Add(cn);//获取命名空间集合
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();//提供对 C# 代码生成器和代码编译器的实例的访问
            //ICodeCompiler icc = csc.CreateCompiler();//定义用于调用源代码编译的接口或使用指定编译器的 CodeDOM 树
            //3.设定编译器的参数
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;//设置是否为可执行文件
            cplist.GenerateInMemory = true;//设置是否在内存中生成输出
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            //4.编译代理类
            CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);// icc.CompileAssemblyFromDom(cplist, ccu);//使用指定的编译器设置编译程序集
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }

            //5.生成代理实例,并调用方法
            System.Reflection.Assembly assembly = cr.CompiledAssembly;//获取或设置已编译的程序集
            Type t = assembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);//为 COM 对象提供对方法的版本无关的访问
            return obj;
        }

        /// <summary>
        /// 组合类路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetClassName(string url)
        {
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}
