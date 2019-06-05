/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/7/8 16:42:35
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace DoNet.Common.Text
{
    /// <summary>
    /// RSS操作
    /// </summary>
    public class Rss
    {
        /// <summary>
        /// 生成空的ＲＳＳ文档
        /// </summary>
        /// <returns></returns>
        public static RssDoc Create()
        {
            return new RssDoc();
        }

        /// <summary>
        /// 通过RSS内容生成文档
        /// </summary>
        /// <param name="content">RSS内容</param>
        /// <returns></returns>
        public static RssDoc CreateDoc(string content)
        {
            return new RssDoc(content);
        }

        /// <summary>
        /// 通过文档生成RSS内容
        /// </summary>
        /// <param name="doc">文档</param>
        /// <returns></returns>
        public static string CreateContent(RssDoc doc)
        {
            var wr = doc.CreateXml();
            return wr.ToString();
        }
    }

    /// <summary>
    /// RSS文档信息
    /// </summary>
    public class RssDoc
    {
        public RssDoc() { }

        /// <summary>
        /// 通过RSS文档内容生成RSS对象
        /// </summary>
        /// <param name="rssContent">RSS内容</param>
        /// </summary>
        public RssDoc(string rssContent)
        {
            FromRss(rssContent);
        }

        /// <summary>
        /// 编码
        /// </summary>
        protected const string RssEncoding = "utf-8";
        /// <summary>
        /// 版本
        /// </summary>
        protected const string Version = "2.0";
        /// <summary>
        /// XML版本
        /// </summary>
        protected const string XmlVersion = "1.0";
        /// <summary>
        /// xml文档头部
        /// </summary>
        protected const string XmlHeader = "version=\"" + XmlVersion + "\" encoding=\"" + RssEncoding + "\"";

        /// <summary>
        /// 标题 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 标志
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 简要说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 指定语言，如：中文:zh-cn
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime PubDate { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastBuildDate { get; set; }

        /// <summary>
        /// RSS访问路径
        /// </summary>
        public string Docs { get; set; }

        /// <summary>
        /// 创建说明
        /// </summary>
        public string Generator { get; set; }

        /// <summary>
        /// 联系
        /// </summary>
        public string ManagingEditor { get; set; }

        /// <summary>
        /// 管理邮箱
        /// </summary>
        public string WebMaster { get; set; }

        List<RssItem> _items = new List<RssItem>();
        /// <summary>
        /// 所有RSS子项
        /// 默认为0个
        /// </summary>
        public List<RssItem> Items { get { return _items; } set { _items = value; } }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="item">ＲＳＳ子项</param>
        public void Add(RssItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="link">链接地址</param>
        public void Add(string title, string link)
        {
            Items.Add(new RssItem() { Title = title, Link = link });
        }

        /// <summary>
        /// 生成XML操作实例
        /// </summary>
        /// <returns></returns>
        public IO.XmlHelper CreateXml()
        {
            var rssXmlWr = new DoNet.Common.IO.XmlHelper();//XML读写

            rssXmlWr.CreateNewXML(RssDoc.XmlHeader);
            //结点属性集合
            var attributeDic = new Dictionary<string, string>();           
            attributeDic.Add("version", RssDoc.Version);

            //创建根结点rss
            var rssRoot = rssXmlWr.CreateNewNode(null, XmlNodeType.Element, "rss", attributeDic);
            //创建频道
            var channcel = rssXmlWr.CreateNewNode(rssRoot, XmlNodeType.Element, "channel");
            CreateNode("title", Title, rssXmlWr, channcel);//写入标题 
            CreateNode("link", Link, rssXmlWr, channcel);
            if (!string.IsNullOrWhiteSpace(Logo))
            {
                CreateNode("image", string.Format("<url>{0}</url>", Logo), rssXmlWr, channcel);
            }
            CreateNode("description", Description, rssXmlWr, channcel);
            CreateNode("language", Language, rssXmlWr, channcel);
            if (PubDate != DateTime.MinValue && PubDate != DateTime.MaxValue)
                CreateNode("pubDate", PubDate.ToString("yyyy/MM/dd HH:mm:ss"), rssXmlWr, channcel);
            //var ci = new CultureInfo("en-us");
            if (LastBuildDate != DateTime.MinValue && LastBuildDate != DateTime.MaxValue)
                CreateNode("lastBuildDate", this.LastBuildDate.ToString("yyyy/MM/dd HH:mm:ss"), rssXmlWr, channcel);

            CreateNode("docs", Docs, rssXmlWr, channcel);
            CreateNode("generator", Generator, rssXmlWr, channcel);
            CreateNode("managingEditor", ManagingEditor, rssXmlWr, channcel);
            CreateNode("webMaster", WebMaster, rssXmlWr, channcel);
            CreateNode("ttl", Items.Count.ToString(), rssXmlWr, channcel);

            foreach (var item in Items)
            {
                CreateRssItem(item, rssXmlWr, channcel);
            }

            return rssXmlWr;
        }

        /// <summary>
        /// 把RSS项转为xml节点
        /// </summary>
        /// <param name="item">RSS项信息</param>
        /// <returns></returns>
        protected XmlNode CreateRssItem(RssItem item, IO.XmlHelper xmlwr, XmlNode parent)
        {
            var xn = xmlwr.CreateNewNode(parent, XmlNodeType.Element, "item");
            CreateNode("title", item.Title, xmlwr, xn);
            CreateNode("link", item.Link, xmlwr, xn);
            CreateNode("description", item.Description, xmlwr, xn);
            CreateNode("pubDate", item.PubDate.ToString("yyyy/MM/dd HH:mm:ss"), xmlwr, xn);
            CreateNode("category", item.Cagtegory, xmlwr, xn);
            CreateNode("author", item.Author, xmlwr, xn);
            CreateNode("guid", item.Guid, xmlwr, xn);
            return xn;
        }

        /// <summary>
        /// 生成节点
        /// </summary>
        /// <param name="name">节点名</param>
        /// <param name="value">节点值</param>
        /// <param name="xmlwr">写对象</param>
        /// <param name="parent">父节点</param>
        protected static XmlNode CreateNode(string name, string value, IO.XmlHelper xmlwr, XmlNode parent)
        {
            if (!string.IsNullOrWhiteSpace(value) && parent != null)
            {
                var rsstitle = xmlwr.CreateNewNode(parent, XmlNodeType.Element, name);
                rsstitle.InnerText = value;
                return rsstitle;
            }
            return null;
        }

        /// <summary>
        /// 通过RSS内容获取整个信息
        /// </summary>
        /// <param name="rssContent">内容</param>
        /// <returns></returns>
        protected void FromRss(string rssContent)
        {
            var xmlwr = new IO.XmlHelper();//读取器
            xmlwr.LoadXmlString(rssContent);

            this.Title = GetNodeValue("title", "channel", xmlwr);
            this.Description = GetNodeValue("description", "channel", xmlwr);
            this.Docs = GetNodeValue("docs", "channel", xmlwr);
            this.Generator = GetNodeValue("generator", "channel", xmlwr);
            this.Language = GetNodeValue("language", "channel", xmlwr);
            
            this.Link = GetNodeValue("link", "channel", xmlwr);
            this.ManagingEditor = GetNodeValue("managingeditor", "channel", xmlwr);
            this.WebMaster = GetNodeValue("WebMaster", "channel", xmlwr);           

            //获取子项
            var nodes = xmlwr.GetNodeListByDelegate(delegate(XmlNode xn)
            {
                return xn.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase);
            });

            Items.Clear();
            foreach (var xn in nodes)
            {
                if (xn.ChildNodes != null)
                {
                    var item = new RssItem();
                    foreach (XmlNode chn in xn.ChildNodes)
                    {
                        switch (chn.Name.ToLower())
                        {
                            case "title":
                                {
                                    item.Title = chn.InnerText;
                                    break;
                                }
                            case "link":
                                {
                                    item.Link = chn.InnerText;
                                    break;
                                }
                            case "description":
                                {
                                    item.Description = chn.InnerText;
                                    break;
                                }
                            case "author":
                                {
                                    item.Author = chn.InnerText;
                                    break;
                                }
                            case "comments":
                                {
                                    item.Comments = chn.InnerText;
                                    break;
                                }
                            case "pubdate":
                                {
                                    DateTime dt;
                                    if (DateTime.TryParse(chn.InnerText, out dt))
                                    {
                                        item.PubDate = dt;
                                    }
                                    break;
                                }
                            case "category":
                                {
                                    item.Cagtegory = chn.InnerText;
                                    break;
                                }
                            case "guid":
                                {
                                    item.Guid = chn.InnerText;
                                    break;
                                }
                        }
                    }

                    Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 获取节点的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        /// <param name="xmlwr"></param>
        /// <returns></returns>
        protected string GetNodeValue(string name, string parentName, IO.XmlHelper xmlwr)
        {
            var node = xmlwr.GetNodeByDelegate((XmlNode xn) =>
            {
                return xn.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrWhiteSpace(parentName) ||
                    (xn.ParentNode != null && xn.ParentNode.Name.Equals(parentName, StringComparison.OrdinalIgnoreCase)));
            });
            if(node != null)return node.InnerText;
            return "";
        }

        /// <summary>
        /// 转为RRS字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var xml = CreateXml();
            return xml.ToString();
        }
    }

    /// <summary>
    /// RSS项
    /// </summary>
    public class RssItem
    {
        /// <summary>
        /// 标题 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 简要说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime PubDate { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Cagtegory { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Comments{get;set;}
    }
}
