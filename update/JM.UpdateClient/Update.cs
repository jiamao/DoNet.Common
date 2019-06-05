using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JM.UpdateClient
{
    public class Update
    {
        public static bool Run(string[] args = null)
        {
            //已经执行过更新则不再启动更新程序
            if (args != null && args.Length > 0 && args[0] == "updated")
            {
                return false;
            }
            var excpath = System.Windows.Forms.Application.ExecutablePath;
            var path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "JM.UpdateClient.exe");
            
            System.Diagnostics.Process.Start(path, excpath);
            System.Windows.Forms.Application.Exit();
            return true;
        }
    }
}
