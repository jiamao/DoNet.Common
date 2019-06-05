/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/7/28 15:00:56
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
    public partial class paging : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //绑定翻页事件
            Paging1.OnPageChange += new EventHandler<DoNet.Common.Web.PagingEventArg>(Paging1_OnPageChange);

            if (!Page.IsPostBack)
            {
                search(1);//查询第一页
            }
        }

        void Paging1_OnPageChange(object sender, DoNet.Common.Web.PagingEventArg e)
        {
            //根据e.pageindex得到翻到哪一页
            //再重新查询此页即可
            Response.Write("总共有:" + e.PageCount + " 页");
            search(e.PageIndex);
        }

        /// <summary>
        /// 测试的查询函数
        /// </summary>
        /// <param name="index">第几页</param>
        void search(int index)
        {
            //因为页码是从1开始的
            if (index <= 0) index = 1;

            //写死查询数据方法。
            //直接返回10条记录
            var data = new List<testData>();
            for (var i = 0; i < 10; i++)
            {
                var d = new testData()
                {
                    id=i + index,
                    name="test_" + index.ToString()
                };

                data.Add(d);
            }

            GridView1.DataSource = data;
            GridView1.DataBind();

            //初始化页码
            //根据您查得的记录来赋值。这里写死
            Paging1.PageIndex = index;//当前页
            Paging1.PageCount = 100;//查得记录总共多少页
        }
    }

    class testData
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
