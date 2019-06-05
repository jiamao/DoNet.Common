/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/5 10:39:11
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DoNet.Common.Examples.web
{
    public partial class xml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Literal1.Mode = LiteralMode.Encode;

             CreateXml();//生成文档
            
            Button1.Click += new EventHandler(Button1_Click);
        }

        void Button1_Click(object sender, EventArgs e)
        {
            ReadXml();
        }

        /// <summary>
        /// 生成XML文档
        /// </summary>
        void CreateXml()
        {
            //生成XML文档
            var xmlwr = new DoNet.Common.IO.XmlHelper();

            xmlwr.CreateNewXML();

            var root = xmlwr.CreateNewNode(XmlNodeType.Element,"root");

            //创建root的子节点，并生成属性
            var attr=new Dictionary<string,string>();
            attr.Add("key","key1");
            attr.Add("value","v1");
            var xn = xmlwr.CreateNewNode(root, XmlNodeType.Element, "item1", attr);

            //创建xn的子节点，并创建属性
            attr = new Dictionary<string, string>();
            attr.Add("abd", "key1");
            attr.Add("fsdfsf", "v1");
            var xnh = xmlwr.CreateNewNode(xn, XmlNodeType.Element, "item11", attr);

            //展示
            Literal1.Text = xmlwr.ToString();

            //调用save可以保存为本地文档
            //xmlwr.Save("c:\\1.xml");
        }

        /// <summary>
        /// 读取XML操作
        /// </summary>
        void ReadXml()
        {
            //生成XML文档
            //也可以输入文件地址读取
            var xmlwr = new DoNet.Common.IO.XmlHelper();

            xmlwr.LoadXmlString(Literal1.Text);//读取XML字符

            //查找父节点为root。且节点名为item1的节点
            var xn1 = xmlwr.GetNodeByDelegate((XmlNode x) =>
                {
                    return x.ParentNode != null &&
                        x.ParentNode.Name.Equals("root", StringComparison.OrdinalIgnoreCase) &&
                        x.Name.Equals("item1", StringComparison.OrdinalIgnoreCase);
                });

            if (xn1 != null)
            {
                Response.Write("item1:" + DoNet.Common.Text.StringHelper.ToHtml(xn1.InnerXml));
            }

            //获取root下的子节点
            var xns = xmlwr.GetNodeListByDelegate((XmlNode x) =>
            {
                return x.ParentNode != null &&
                    x.ParentNode.Name.Equals("root", StringComparison.OrdinalIgnoreCase);
            });

            //显示个数
             Response.Write("<br /> root children:" + xns.Count);
            

            //把所有属性转为集合
            //第一个参数表示是否转为最小化
            var dic = xmlwr.TurnXmlToDictionary(false);

            foreach (var d in dic)
            {
                Response.Write("<br />" + d.Key + ":" + d.Value);
            }
        }
    }
}
