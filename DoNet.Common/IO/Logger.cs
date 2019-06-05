//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;

using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DoNet.Common.IO
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 设置日志目录
        /// </summary>
        /// <param name="path"></param>
        public static string SetLogPath(string path)
        {
            //因为是异步必须先确定路径,否则如果是WEB的话无法得到工作路径(context为空)
            path = string.IsNullOrWhiteSpace(path) ? "log" : path;
            return Log.LogDirectory = Common.IO.PathMg.CheckPath(path);
        }

        /// <summary>
        /// 写系统日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void WriteSystemlog(string msg)
        {
            try
            {
                var log = new SystemLog()
                {
                    Date = DateTime.Now,
                    LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg
                };

                LogControl.Instance.AsyncPushLog(log);//加入到队列中
            }
            catch { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(string msg)
        {
            try
            {
                var log = new NormalLog()
                {
                    Date = DateTime.Now,
                    LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg
                };

                LogControl.Instance.AsyncPushLog(log);//加入到队列中
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
        public static void FlushWrite(string msg)
        {
            try
            {
                var log = new NormalLog()
                {
                    Date = DateTime.Now,
                    LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg
                };

                LogControl.Instance.PushLog(log);//直接写入文件
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 写日志到指定的文件
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(string file,string msg)
        {
            try
            {
                var log = new NormalLog()
                {
                    Date = DateTime.Now,
                    LogFileName=file,
                    LogInfo = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + msg
                };

                LogControl.Instance.AsyncPushLog(log);//加入到队列中
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(object msg)
        {
            try
            {
                var log = new NormalLog()
                {
                    Date = DateTime.Now                   
                };

                if (msg is ValueType || msg is string)
                {
                    log.LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg.ToString();
                }
                else
                {
                    log.LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + Serialization.FormatterHelper.XMLSerObjectToString(msg);
                }

                LogControl.Instance.AsyncPushLog(log);//加入到队列中
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="l1">日志信息</param>
        public static void Debug(string l1)
        {
            //如果没有配置debug模式就直接退出
            if (!DebugLog.WDebugLog) return;
            var log = new DebugLog()
            {
                Date = DateTime.Now,
                LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + l1
            };

            LogControl.Instance.AsyncPushLog(log);//加入到队列中
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="l1">信息1</param>
        /// <param name="l2">信息2</param>
        public static void Debug(string l1, string l2)
        {
            //如果没有配置debug模式就直接退出
            if (!DebugLog.WDebugLog) return;

            Debug(l1);
            Debug(l2);
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="l1">信息1</param>
        /// <param name="l2">信息2</param>
        /// <param name="l3">信息3</param>
        public static void Debug(string l1, string l2,string l3)
        {
            //如果没有配置debug模式就直接退出
            if (!DebugLog.WDebugLog) return;

            Debug(l1);
            Debug(l2);
            Debug(l3);
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="LogItems"></param>
        public static void Debug(params object[] LogItems)
        {
            //如果没有配置debug模式就直接退出
            if (!DebugLog.WDebugLog) return;

            try
            {
                if (LogItems != null)
                {
                    foreach (var item in LogItems)
                    {
                        if (item == null) continue;
                        var log = new DebugLog()
                        {
                            Date = DateTime.Now
                        };

                        if (item is ValueType || item is string)
                        {
                            log.LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + item.ToString();
                        }
                        else
                        {
                            log.LogInfo = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + Serialization.FormatterHelper.XMLSerObjectToString(item);
                        }
                        LogControl.Instance.AsyncPushLog(log);//加入到队列中
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 等待日志完成
        /// </summary>
        public static void WaitLogFlush()
        {
            LogControl.WaitLogFlush();
        }
    }

    /// <summary>
    /// 日志处理类
    /// </summary>
    class LogControl
    {
        private LogControl()
        {
            //因为是异步必须先确定路径,否则如果是WEB的话无法得到工作路径(context为空)
            if (string.IsNullOrWhiteSpace(Log.LogDirectory))
            {
                Log.LogDirectory = Common.IO.PathMg.CheckPath("log");
            }
        }

        static LogControl() {
            _instance = new LogControl();
        }

        static LogControl _instance = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static LogControl Instance { get { return _instance; } }

        /// <summary>
        /// 需要写的日志队列
        /// </summary>
        static List<Log> LogCache = new List<Log>();

        /// <summary>
        /// 写日志缓存字符串
        /// </summary>
        static Dictionary<string, StringBuilder> loginfos = new Dictionary<string, StringBuilder>();

        //消息队列锁
        static object _pushLock = new object();
        //线程执行标记:如果为false则表示没有正在执行.为true则表示正在则行
        static bool Flag = false;

        delegate void DelegateFlushLog();
        //处理日志委托
        static DelegateFlushLog FlushLog = new DelegateFlushLog(Flush);

        /// <summary>
        /// 把日志对象加入到队列
        /// </summary>
        /// <param name="log"></param>
        public void AsyncPushLog(Log log)
        {
            lock (LogCache)
            {
                LogCache.Add(log);//加入到队列中               
            }
            FlushLog.BeginInvoke(null, null);//异步调用处理函数                 
        }

        /// <summary>
        /// 直接写日志
        /// </summary>
        /// <param name="log"></param>
        public void PushLog(Log log)
        {
            log.Write();//异步调用处理函数           
        }

        /// <summary>
        /// 处理所有日志
        /// </summary>
        public static void Flush()
        {
            if (Flag || LogCache.Count == 0) return;
            //锁
            lock (_pushLock)
            {
                Flag = true;//标记我正在处理                

                //锁后继续处理
                while (LogCache.Count > 0)
                {
                    try
                    {
                        var log = LogCache[0];//获取第一个对象
                        LogCache.RemoveAt(0);//返回并移除第一个写成功的对象

                        if (log != null && !string.IsNullOrWhiteSpace(log.LogInfo))
                        {
                            //日志路径
                            var logpath = log.GetLogPath();

                            StringBuilder loginfo;
                            if (!loginfos.TryGetValue(logpath, out loginfo))
                            {
                                //每次写入多个日志
                                loginfo = loginfos[logpath] = new StringBuilder(102400);
                            }
                            //如果当前日志本身长度就超出预设。则直接写入
                            if (log.LogInfo.Length >= loginfo.Capacity)
                            {
                                File.AppendAllText(logpath, log.LogInfo + Environment.NewLine, Log.LogEncoding);
                            }
                            else
                            {
                                //如果加上当前日志超出预设长度则写入以前的日志
                                if (loginfo.Length > 0 && loginfo.Length + log.LogInfo.Length > loginfo.Capacity)
                                {
                                    File.AppendAllText(logpath, loginfo.ToString(), Log.LogEncoding);
                                    loginfo.Clear();
                                }
                                //加入日志字符中
                                loginfo.AppendLine(log.LogInfo);
                            }
                        }

                        //表示为最后一个日志
                        //则写入所有字符串缓存
                        if (LogCache.Count == 0)
                        {
                            foreach (var l in loginfos)
                            {
                                if (l.Value.Length > 0)
                                {
                                    File.AppendAllText(l.Key, l.Value.ToString(), Log.LogEncoding);
                                    l.Value.Clear();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("logflush:" + ex.ToString());
                    }
                }
                Flag = false;//标记我已处理完
            }
        }

        /// <summary>
        /// 等待日志完成
        /// </summary>
        /// <param name="filename"></param>
        public static void WaitLogFlush()
        {
            while (!CheckLogComplete())
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 检查日志是否写完
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool CheckLogComplete()
        {
            try
            {                
               return LogCache.Count == 0;                           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    [Serializable]
    abstract class Log
    {
        /// <summary>
        /// 写日志的路径
        /// </summary>
        /// <summary>
        /// 写日志的路径
        /// </summary>
        public static string LogDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// 生成路径
        /// </summary>
        /// <param name="logfile"></param>
        /// <returns></returns>
        public static string GetLogPath(string logfile)
        {
            if (string.IsNullOrEmpty(logfile))
            {
                logfile = DateTime.Now.ToString("yyyyMMdd");
            }
            return System.IO.Path.Combine(LogDirectory, logfile + LogFileExt);
        }

        /// <summary>
        /// 日志文件后缀
        /// </summary>
        protected const string LogFileExt = ".log";

        /// <summary>
        /// 默认以utf-8编码写日志
        /// </summary>
        public static Encoding LogEncoding = Encoding.UTF8;

        string _logfilename;
        /// <summary>
        /// 日志文件名
        /// </summary>
        public string LogFileName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_logfilename))
                {
                   return _logfilename;
                }
                return Date.ToString("yyyyMMdd");
            }
            set {
                _logfilename = value;
            }
        }

        /// <summary>
        /// 生成日志路径
        /// </summary>
        /// <returns></returns>
        public abstract string GetLogPath();

        /// <summary>
        /// 日志信息
        /// </summary>
        public string LogInfo { get; set; }

        /// <summary>
        /// 写日志时间
        /// </summary>
        public DateTime Date { get; set; }

        public virtual string Write()
        {
            var path = System.IO.Path.Combine(LogDirectory, LogFileName);
            System.IO.File.AppendAllText(path, LogInfo, System.Text.Encoding.UTF8);
            return path;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public virtual void Write(StreamWriter sw)
        {
            if (!string.IsNullOrWhiteSpace(LogInfo))
            {
                //写入文件流
                sw.WriteLine("[{0}] {1}", DateTime.Now.ToShortTimeString(), LogInfo);
            }
        }
    }
    /// <summary>
    /// 调试日志
    /// </summary>
    [Serializable]
    class DebugLog : Log
    {
        /// <summary>
        /// 获取写日志的路径
        /// </summary>
        /// <returns></returns>
        public override string GetLogPath()
        {
            var dir = PathMg.CheckPath(Log.LogDirectory, "debug");
            DirectoryHelper.CreateDirectory(dir);//创建目录
            return PathMg.CheckPath(dir, LogFileName + Log.LogFileExt);//调试日志
        }

        /// <summary>
        /// 是否写调试日志
        /// </summary>
        public static bool WDebugLog
        {
            get
            {
                var tmp = System.Configuration.ConfigurationManager.AppSettings["Debug"];
                return !string.IsNullOrWhiteSpace(tmp) && tmp.Equals("true", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public override void Write(StreamWriter sw)
        {
            if (WDebugLog)
            {
                base.Write(sw);
            }
        }
    }

    /// <summary>
    /// 普通日志
    /// </summary>
    [Serializable]
    class NormalLog : Log
    {
        /// <summary>
        /// 获取写日志的路径
        /// </summary>
        /// <returns></returns>
        public override string GetLogPath()
        {
            DirectoryHelper.CreateDirectory(Log.LogDirectory);//创建目录
            return PathMg.CheckPath(Log.LogDirectory, LogFileName + Log.LogFileExt);
        }
    }

    /// <summary>
    /// 普通日志
    /// </summary>
    [Serializable]
    class SystemLog : Log
    {
        /// <summary>
        /// 获取写日志的路径
        /// </summary>
        /// <returns></returns>
        public override string GetLogPath()
        {
            var dir = PathMg.CheckPath(Log.LogDirectory, "System");
            DirectoryHelper.CreateDirectory(dir);//创建目录
            return PathMg.CheckPath(dir, LogFileName + Log.LogFileExt);
        }
    }
}
