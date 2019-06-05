/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/5 16:44:14
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
    public partial class net : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("当前服务端IP：" + DoNet.Common.Net.BaseNet.GetLocalIPAddress());
            Response.Write("<br />");
            Response.Write("当前服务端机器名：" + DoNet.Common.Net.BaseNet.GetHostName());
            Response.Write("<br />");
            Response.Write("当前访问者IP:" + DoNet.Common.Net.BaseNet.GetWebIPAddress());

            Button1.Click += new EventHandler(Button1_Click);
        }

        void Button1_Click(object sender, EventArgs e)
        {
            //发送邮件
            var mail = new DoNet.Common.Net.MailHelper();

            var content=@"<div id='slhost' class='tabContent' style='overflow: hidden; height: 100%; width: 100%;'>   

     <object id='oap_sl' data='data:application/x-silverlight-2,' type='application/x-silverlight-2'

                width='100%' height='100%' style='height: 600px;'>

                <param name='source' value='/ClientBin/OAP.xap?ver=2.0.3.170'/>

                <param name='onError' value='onSilverlightError' />

                <param name='onLoad' value='slOnLoad' />

                <param name='background' value='white' />

                <param name='minRuntimeVersion' value='4.0.50826.0' />

                <param name='autoUpgrade' value='true' />

               <param name='enableGPUAcceleration' value='true' />

                <param name='splashscreensource' value='/Content/Loading.xaml'/>

            <param name='onSourceDownloadProgressChanged' value='onSourceDownloadProgressChanged' />

                 <param name='culture' value='zh-CN' />

            <param name='uiCulture' value='zh-CN' />

                <param name='windowless' value='true' />

                <param name='initParams' value='TType=2,LType=2,Ver=2011.0.3.5' />

                <a href='http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0' style='text-decoration: none'>

                    <img src='http://go.microsoft.com/fwlink/?LinkId=161376' alt='获取 Microsoft Silverlight'

                        'style='border-style: none' />

                </a>

            </object>

    </div>
";
            //发送
            //如果需要发送附件等请看其它重载
            mail.QueueSend(txtsmtp.Text, txtmail.Text, txtuser.Text, txtpwd.Text, txtreceiver.Text, txtsubject.Text, content, true);
            mail.Startup();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "ok", "alert('发送完成');", true);
        }
    }
}
