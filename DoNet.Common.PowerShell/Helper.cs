//////////////////////////////////////////////////
// Author   : 家猫
// Date     : 2010/09/15
// Usage    : PowerShell操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace DoNet.Common.PowerShell
{
    /// <summary>
    ///  PowerShell操作类
    /// </summary>
    public static class ScriptRunner
    {
        /// <summary>
        /// 执行单条脚本
        /// </summary>
        /// <param name="script">脚本</param>       
        /// <returns></returns>
        public static IEnumerable<object> RunScript(string script)
        {
            return RunScript(script, null,null);
        }

        /// <summary>
        /// 执行单条脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <param name="dataReader">异步消息阅读器</param>
        /// <returns></returns>
        public static IEnumerable<object> RunScript(string script, EventHandler<PSEventArg> dataReader)
        {
            return RunScript(script, null, dataReader);
        }

        /// <summary>
        /// 执行单条脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <param name="pars">参数集</param>
        /// <returns></returns>
        public static IEnumerable<object> RunScript(string script, IDictionary<string, object> pars)
        {
            return RunScript(script, pars, null);
        }

        /// <summary>
        /// 执行单条脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <param name="pars">执行参数集合</param>
        /// <param name="dataReader">消息异步阅读委托</param>
        /// <returns></returns>
        public static IEnumerable<object> RunScript(string script, IDictionary<string, object> pars, EventHandler<PSEventArg> dataReader)
        {
            var scriptHost = new SpacePiple();
            scriptHost.DataReader += dataReader;

            scriptHost.Scripts.Add(script);

            //传入参数
            if (pars != null)
            {
                foreach (var p in pars)
                {
                    if (!scriptHost.Params.ContainsKey(p.Key)) scriptHost.Params.Add(p.Key, p.Value);
                }
            }
            //执行脚本
            return scriptHost.Execute();
        }

        /// <summary>
        /// 执行多条脚本
        /// </summary>
        /// <param name="scripts">脚本集合</param>
        /// <param name="pars">参数，在脚本执行中用到的参数</param>
        /// <param name="dataReader">异步消息阅读器</param>
        /// <returns></returns>
        public static IEnumerable<object> RunScript(IEnumerable<string> scripts, IDictionary<string,object> pars, EventHandler<PSEventArg> dataReader)
        {
            var scriptHost = new SpacePiple();
            scriptHost.DataReader += dataReader;

            scriptHost.Scripts.AddRange(scripts);
            
            //传入参数
            if (pars != null)
            {
                foreach (var p in pars)
                {
                    if (!scriptHost.Params.ContainsKey(p.Key)) scriptHost.Params.Add(p.Key, p.Value);
                }
            }
            //执行脚本
            return scriptHost.Execute();
        }

        /// <summary>
        /// 生成全全凭证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PSCredential CreateCredential(string username, string password)
        {
            using (System.Security.SecureString secureString = new System.Security.SecureString())
            {
                foreach (char ch in password)
                {
                    secureString.AppendChar(ch);
                }

                return new PSCredential(username, secureString);
            }
        }
    }

    /// <summary>
    /// 脚本执行主体
    /// </summary>
    public class SpacePiple
    {
        public SpacePiple() 
        {
            Params = new Dictionary<string, object>();
            Scripts = new List<string>();
        }

        /// <summary>
        /// 同步锁
        /// </summary>
        object _syncLock = new object();

        /// <summary>
        /// 所有需要执行的脚本
        /// </summary>
        public List<string> Scripts { get; set; }

        /// <summary>
        /// 执行脚本所需要注入的参数
        /// </summary>
        public Dictionary<string, object> Params { get; set; }

        List<EventHandler<PSEventArg>> _readerHandlers = new List<EventHandler<PSEventArg>>();
        /// <summary>
        /// 消息接收委托
        /// PSEventArg 的属性sender,Value 表示异步读取到的对象
        /// </summary>
        public event EventHandler<PSEventArg> DataReader
        {
            add {
                lock (_syncLock)
                {
                    _readerHandlers.Add(value);
                }
            }
            remove {
                lock (_syncLock)
                {
                    _readerHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// 执行当前脚本
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> Execute()
        {
            var spConfig = RunspaceConfiguration.Create();
            using (var sp = RunspaceFactory.CreateRunspace(spConfig))
            {
                sp.Open();

                //注入参数
                if (Params != null)
                {
                    foreach (var par in Params)
                    {
                        sp.SessionStateProxy.SetVariable(par.Key, par.Value);
                    }
                }

                Pipeline pipeline = sp.CreatePipeline();               

                if (_readerHandlers.Count > 0)
                {
                    //消息接收
                    lock (_syncLock)
                    {
                        //绑定异步消息阅读
                        pipeline.Output.DataReady += (object sender, EventArgs e) =>
                        {
                            var arg = new PSEventArg();
                            var reader = (PipelineReader<PSObject>)(sender);
                            arg.Value = reader.Read();
                            foreach (var handler in _readerHandlers)
                            {
                                if (handler != null) handler.BeginInvoke(sender, arg, null, null);//异步执行消息回调
                            }
                        };
                    }
                }

                foreach (var s in Scripts)
                {
                    //var cmd = new Command(s, true, true);
                    //pipeline.Commands.Add(cmd);
                    pipeline.Commands.AddScript(s, true);
                }

                //返回结果
                var results = pipeline.Invoke();
                sp.Close();

                return results;
            }
        }
    }

    /// <summary>
    /// 执行脚本时，消息事件参数
    /// </summary>
    public class PSEventArg:EventArgs
    {
        public object Value
        {
            get;
            set;
        }
    }
}
