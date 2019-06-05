/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/3 18:47:35
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DoNet.Common.Examples.web
{
    public partial class logger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(Button1_Click);

            DoNet.Common.IO.Logger.SetLogPath("log");//设置日志目录为当前目录下的log文件夹，此句可省。默认就为log目录

            //获取当前路径
            Response.Write(Common.IO.PathMg.CheckWebPath("~"));

            Response.Write("<br />");

            var logpath = Common.IO.PathMg.CheckPath("log");
            //获取当前目录下的日志目录路径
            Response.Write(logpath);

            Response.Write("<br />");

            //组合当前路径下的日志路径
            Response.Write(Common.IO.PathMg.CheckPath(logpath, DateTime.Now.ToString("yyyyMMdd")));
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button1_Click(object sender, EventArgs e)
        {
            //写调试日志，必须得config中配置debug=true项，如：
            // <appSettings>
            //   <add key="debug" value="true"/>
            // </appSettings>
            DoNet.Common.IO.Logger.Debug("测试", "debug日志");

            //此日志不受debug影响
            DoNet.Common.IO.Logger.Write("写正常日志");//写日志，在log目录下生成当前日期的日志
        }
    }
}
