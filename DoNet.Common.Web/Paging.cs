/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/7/28 14:39:39
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DoNet.Common.Web
{
    [DefaultProperty("PageIndex")]
    [ToolboxData("<{0}:Paging runat=server></{0}:Paging>")]
    public class Paging : WebControl,IPostBackEventHandler
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Paging() {
            this.CssClass = "jm-paging";
        }

        /// <summary>
        /// 当前页附近显示多少页
        /// 默认为10
        /// </summary>
        private int _showPageCount = 5;
        [Bindable(true)]
        [Category("SelectPageCount")]
        [DefaultValue("5")]
        [Localizable(true)]
        public int ShowPageCount
        {
            get
            {
                return _showPageCount;
            }
            set { _showPageCount = value; }
        }

        private int _PageIndex = 1;
        /// <summary>
        /// 当前页码
        /// </summary>
        [Bindable(true)]
        [Category("PageIndex")]
        [DefaultValue("10")]
        [Localizable(true)]
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set { _PageIndex = value; }
        }

        private int _PageCount = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        [Bindable(true)]
        [Category("PageCount")]
        [DefaultValue("1")]
        [Localizable(true)]
        public int PageCount
        {
            get
            {
                return _PageCount;
            }
            set { _PageCount = value; }
        }

        string _curPageindexCss = "jm_curPage";
        /// <summary>
        /// 当前页的样式
        /// </summary>
        [Bindable(true)]
        [Category("CurPageIndexCss")]
        [DefaultValue("jm_curPage")]
        [Localizable(true)]
        public string CurPageIndexCss
        {
            get { return _curPageindexCss; }
            set { _curPageindexCss = value; }
        }

        string _preText = "上一页";
        /// <summary>
        /// 上一页显示字符
        /// </summary>
        [Bindable(true)]
        [Category("PreText")]
        [DefaultValue("上一页")]
        [Localizable(true)]
        public string PreText
        {
            get { return _preText; }
            set { _preText = value; }
        }

        string _nextText = "下一页";
        /// <summary>
        /// 下一页显示字符
        /// </summary>
        [Bindable(true)]
        [Category("NextText")]
        [DefaultValue("下一页")]
        [Localizable(true)]
        public string NextText
        {
            get { return _nextText; }
            set { _nextText = value; }
        }

        /// <summary>
        /// 页码改变事件
        /// </summary>
        public event EventHandler<PagingEventArg> OnPageChange;


        //重写默认的标签
        protected override HtmlTextWriterTag TagKey { get { return HtmlTextWriterTag.Div; } }

        /// <summary>
        /// 呈现控件
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            //获取要显示的页码
            var pages = CreatePages();

            //写入上一页
            int preIndex = PageIndex > 1 ? PageIndex - 1 : 0;
            //如果当前页不如首页.则显示上一页
            if (preIndex > 0)
            {
                RendPage(output, preIndex,PreText);
            }
            //显示中间页
            int showedindex = 0;
            foreach (int index in pages)
            {
                if (index - showedindex > 1)
                {
                    output.RenderBeginTag(HtmlTextWriterTag.Span);
                    output.Write("&nbsp;...&nbsp;");
                    output.RenderEndTag();
                }

                output.AddAttribute(HtmlTextWriterAttribute.Class, index == PageIndex ? CurPageIndexCss : "");
                //呈现
                RendPage(output, index, index.ToString());

                showedindex = index;
            }

            //写入下一页
            int nextIndex = PageIndex < PageCount ? PageIndex + 1 : 0;
            //如果当前页不为最后一页.则显示下一页
            if (nextIndex > 0)
            {
                RendPage(output, nextIndex, NextText);
            }
        }

        /// <summary>
        /// 呈显页码
        /// </summary>
        /// <param name="index">第几页</param>
        /// <param name="text">显示字符</param>
        protected void RendPage(HtmlTextWriter output,int index, string text)
        {
            //如果不为当前页才能选择
            if (index != PageIndex)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Onclick,
                    Page.ClientScript.GetPostBackEventReference(this, "__sp_TurnPage_" + index.ToString() + "_" + PageCount.ToString()));
            }
            output.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:;"); 
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(text);
            output.RenderEndTag();
        }

        /// <summary>
        /// 生成页码
        /// 
        /// </summary>
        /// <returns></returns>
        protected List<int> CreatePages()
        {
            var showPagesList = new List<int>();
            showPagesList.Add(1);//加上第一页   
            //如果总页数大于1
            //则第二页也显示，因为默认前二页是必显示的
            if (PageCount > 1) showPagesList.Add(2);

            //计算中间显示的页码部分
            //当前页往前推需要显示数的一半
            int showFirst = PageIndex - ShowPageCount / 2 + 1;
            //中间的后一个为当前向前推显示数的一半
            int showLast = PageIndex + ShowPageCount / 2;

            //如果第一个小于0，最后一个向前推小于0的部分，
            //保证连在一起的显示页码数为showpagecount个数
            if (showFirst < 0) showLast = showLast - showFirst;
            if (showLast > PageCount) showLast = PageCount;

            for (int i = showFirst; i <= showLast; i++)
            {
                if (!showPagesList.Contains(i) && i > 0) showPagesList.Add(i);//如果里面不存在则加入显示行列
            }

            //最后第二页
            var lastP = PageCount - 1;
            if (lastP - 1 > 1 && !showPagesList.Contains(lastP) && lastP > 0) showPagesList.Add(lastP);//加上最后第二页
            if (PageCount > 1 && !showPagesList.Contains(PageCount)) showPagesList.Add(PageCount);//加上最后一页

            return showPagesList;
        }

        /// <summary>
        /// 响应事件
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            string[] pcs = eventArgument.Split('_');
            PageIndex = int.Parse(pcs[4]);//转到第几页
            PageCount = int.Parse(pcs[5]);//总共有几页
            //如果绑定了翻页事件
            if (OnPageChange != null)
            {
                var arg = new PagingEventArg();
                arg.PageCount = PageCount;
                arg.PageIndex = PageIndex;
                OnPageChange(this, arg);//触发翻页事件
            }
        }
    }

    /// <summary>
    /// 翻页事件参数
    /// </summary>
    public class PagingEventArg : EventArgs
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
    }
}
