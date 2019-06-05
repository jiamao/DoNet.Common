using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JM.UpdateClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frmpath = System.IO.Path.Combine(Application.StartupPath,"JM.Update.dll");
            var bs = System.IO.File.ReadAllBytes(frmpath);
            var ass = System.Reflection.Assembly.Load(bs);
            var frmtype = ass.GetType("JM.Update.frmUpdate");
            var frm = Activator.CreateInstance(frmtype) as Form;
            frm.Tag = args;
            Application.Run(frm);
        }
    }
}
