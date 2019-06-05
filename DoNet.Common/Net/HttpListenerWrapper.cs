#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Remoting.Lifetime;
#endregion

namespace DoNet.Common.Net
{
    internal class HttpListenerWrapper : MarshalByRefObject
    {
		#region 重构 MarshalByRefObject 成员
		/// <summary>
		/// 用来防止对象过期
		/// </summary>
		/// <returns> </returns>
		public override object InitializeLifetimeService()
		{
			ILease lease = (ILease)base.InitializeLifetimeService();
			if (lease.CurrentState == LeaseState.Initial)
			{
				lease.InitialLeaseTime = TimeSpan.Zero;
			}
			return lease;
		}
		#endregion

        private HttpListener _listener;
        private string _virtualDir;
        private string _physicalDir;
        private delegate void RequestHandler(HttpListenerContext context);

        public void Configure(string vdir, string pdir)
        {
            _virtualDir = vdir;
            _physicalDir = pdir;
            _listener = new HttpListener();
        }

		public void AddPrefix(string Prefix)
		{
			_listener.Prefixes.Add(Prefix);
		}

		public void RemovePrefix(string Prefix)
		{
			_listener.Prefixes.Remove(Prefix);
		}

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
			//_listener.Abort();
            _listener.Stop();
        }

        public void ProcessRequest()
        {
            _listener.BeginGetContext(GetContextCallback, null);				
        }

        private void GetContextCallback(IAsyncResult ir)
        {
            var context = _listener.EndGetContext(ir);
            var handler = new RequestHandler(Request);
            handler.BeginInvoke(context, null, null);
            //继续下一次请求
            _listener.BeginGetContext(GetContextCallback, null);	

            Console.WriteLine(context.Request.Url.ToString());

        }

        /// <summary>
        /// 异步处理请求
        /// </summary>
        /// <param name="context"></param>
        private void Request(HttpListenerContext context)
        {
            HttpListenerWorkerRequest workerRequest =
                new HttpListenerWorkerRequest(context, _virtualDir, _physicalDir);
            HttpRuntime.ProcessRequest(workerRequest);
        }
    }
}