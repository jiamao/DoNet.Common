using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JM.Common.Examples.web
{
    public partial class replace : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnstart.ServerClick += btnstart_ServerClick;
        }

        void btnstart_ServerClick(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/Conts");
            var files = System.IO.Directory.GetFiles(path, "*.dat", System.IO.SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var content = DoNet.Common.IO.FileHelper.ReadAllText(f, System.Text.Encoding.UTF8);
                content = content.Replace(".shtml", ".aspx");
                System.IO.File.WriteAllText(f, content, System.Text.Encoding.UTF8);
                Response.Write(f + "<br />");
            }
            files = System.IO.Directory.GetFiles(path, "*.htm", System.IO.SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var content = DoNet.Common.IO.FileHelper.ReadAllText(f, System.Text.Encoding.UTF8);
                content = content.Replace(".shtml", ".aspx");
                System.IO.File.WriteAllText(f, content, System.Text.Encoding.UTF8);
                Response.Write(f + "<br />");
            }
            files = System.IO.Directory.GetFiles(path, "*.html", System.IO.SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var content = DoNet.Common.IO.FileHelper.ReadAllText(f, System.Text.Encoding.UTF8);
                content = content.Replace(".shtml", ".aspx");
                System.IO.File.WriteAllText(f, content, System.Text.Encoding.UTF8);
                Response.Write(f + "<br />");
            }
            files = System.IO.Directory.GetFiles(path, "*.shtml", System.IO.SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var content = DoNet.Common.IO.FileHelper.ReadAllText(f, System.Text.Encoding.UTF8);
                content = content.Replace(".shtml", ".aspx");
                System.IO.File.WriteAllText(f, content, System.Text.Encoding.UTF8);
                Response.Write(f + "<br />");
            }
        }
    }
}