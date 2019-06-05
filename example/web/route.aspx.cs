/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/3 15:15:07
// Usage    : http://www.jm47.com
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DoNet.Common.Examples.web
{
    public partial class route : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request["id"];
            Response.Write("获得变量id:" + id);
            Response.Write("<br/>实际访问的URL:" + Request.Url.AbsoluteUri);
        }
    }
}
