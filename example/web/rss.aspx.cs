/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/3 16:37:32
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
    public partial class rss : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(Button1_Click);

            Button2.Click += new EventHandler(Button2_Click);
        }
        /// <summary>
        /// 生成rss内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button2_Click(object sender, EventArgs e)
        {
            var doc = DoNet.Common.Text.Rss.Create();
            doc.Description = doc.Title = "家猫测试RSS";
            doc.Link = "http://www.jm47.com";
            doc.Language = "zh-cn";

            doc.PubDate = DateTime.Now;
            doc.LastBuildDate = DateTime.Now;
            doc.Docs ="http://www.jm47.com/jmrss";
            doc.Generator = "家猫测试RSS";
            doc.ManagingEditor = "test@163.com";
            doc.WebMaster = "test@163.com";

            //写入五个节点
            //您可以写您的文章信息节点
            for (var i = 0; i < 5; i++)
            {
                var node = new DoNet.Common.Text.RssItem();

                node.Title = "家猫RSS " + i;
                node.Link = doc.Link + "/jmrss/" + i;//这里为您的文章访问地址
                node.Description = "简要说明";

                node.PubDate = DateTime.Now;//日期

                node.Cagtegory = "分类";

                node.Author = "家猫作者";

                node.Guid = "";

                doc.Add(node);
            }

            //输出到当前页面

            rssstring.Text =doc.ToString();
        }

        /// <summary>
        /// 读取RSS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button1_Click(object sender, EventArgs e)
        {
            //DoNet.Common.Net.JMRequest.ReadHttpContent(txtrss.Text) 也可以这样不输编码。但这样通常会编码出错
            var rssContent = DoNet.Common.Net.JMRequest.ReadHttpContent(txtrss.Text, System.Text.Encoding.UTF8);//使用utf8读取RSS内容
            
            var rssDoc = DoNet.Common.Text.Rss.CreateDoc(rssContent);//分解RSS内容

            ////只显示二个属性示例。更多属性自已看了啦 
            txtrssdoc.Text = "title:" + rssDoc.Title + " link:" + rssDoc.Link;

            GridView1.DataSource = rssDoc.Items;//显示子节点RSS文章详细
            GridView1.DataBind();

        }
    }
}
