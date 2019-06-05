using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;

namespace JM.Common.Examples.web
{
    public partial class testserver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var client1 = new ServiceReference1.test1SoapClient();
            client1.Get("");
            var binding = new BasicHttpBinding("BasicHttpBinding_Itest2");
            var epa = new EndpointAddress("http://127.0.0.1:8080/test2.svc");
            var client = new monoserver.Itest2Client(binding,epa);
            var r = client.DoWork();
            Response.Write(r);
        }
    }
}