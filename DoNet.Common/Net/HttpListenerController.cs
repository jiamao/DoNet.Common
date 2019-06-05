#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Threading;
using System.Diagnostics;
#endregion

namespace DoNet.Common.Net
{
	public class HttpListenerController
	{
		private Thread _pump;
		private bool _listening = false;
		private string _virtualDir;
		private string _physicalDir;
		private HttpListenerWrapper _listener;

		public HttpListenerController(string vdir, string pdir)
		{
			_virtualDir = vdir;
			_physicalDir = pdir;

            var dllpath = this.GetType().Assembly.Location;
            var binpath = System.IO.Path.Combine(pdir, "bin", System.IO.Path.GetFileName(dllpath));
            if (!System.IO.File.Exists(binpath))
            {
                IO.FileHelper.CopyFile(dllpath, System.IO.Path.Combine(pdir, "bin", System.IO.Path.GetFileName(dllpath)));
            }

			_listener = (HttpListenerWrapper)ApplicationHost.CreateApplicationHost(
				typeof(HttpListenerWrapper), _virtualDir, _physicalDir);
			_listener.Configure(_virtualDir, _physicalDir);
		}

		public bool IsAlive
		{
			get
			{
				return _pump == null ? false : _pump.IsAlive;
			}
		}

		public void Start()
		{
			_listening = true;
			//_pump = new Thread(new ThreadStart(Pump));
			//_pump.Start();
            _listener.Start();
            _listener.ProcessRequest();
		}

		public void Stop()
		{
			_listening = false;
			_listener.Stop();
		}

		public void AddPrefix(string Prefix)
		{
			_listener.AddPrefix(Prefix);
		}

		public void RemovePrefix(string Prefix)
		{
			_listener.RemovePrefix(Prefix);
		}

		private void Pump()
		{
			_listener.Start();
            _listener.ProcessRequest();
			/*while (_listening)
			{
				try
				{
					_listener.ProcessRequest();
				}
				catch
				{
				}
			}*/
		}
	}

}