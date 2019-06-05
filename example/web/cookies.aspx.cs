/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/1 15:31:51
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
    public partial class cookies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnget.Click += new EventHandler(btnget_Click);
            btnwrite.Click += new EventHandler(btnwrite_Click);
        }

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnwrite_Click(object sender, EventArgs e)
        {
            //写入cookie
            //10分钟有效期
            DoNet.Common.Web.Cookie.Write(txtkey.Text, txtvalue.Text, 10,"");

        }

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnget_Click(object sender, EventArgs e)
        {
            txtvalue.Text = DoNet.Common.Web.Cookie.Read(txtkey.Text);
        }
    }
}
