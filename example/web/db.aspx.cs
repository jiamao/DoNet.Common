/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/10 10:12:00
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace DoNet.Common.Examples.web
{
    public partial class db : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Paging1.OnPageChange += new EventHandler<Web.PagingEventArg>(Paging1_OnPageChange);

            Button1.Click += new EventHandler(Button1_Click);

            if (!Page.IsPostBack)
            {
                //第一次加载显示第一页
                BindData(1);//显示第一页
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var age = 0;
                //这里简单的判断下
                if (!int.TryParse(txtage.Text, out age) || string.IsNullOrWhiteSpace(txtname.Text))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('用户名或年龄格式不正确')", true);
                    return;
                }

                //新增SQL语句
                var sql = string.Format("insert into {0}({1}) values(@mark,@username,@age)", TableName, Fields);

                //传参执行语句
                //如果不传参也可以直接拼SQL语句,只要能保证参数特殊符号
                var pars = new Dictionary<string, object>();
                pars.Add("@mark", Guid.NewGuid().ToString("n"));
                pars.Add("@username", txtname.Text);
                pars.Add("@age", txtage.Text);

                //通过web.config中的配置创建连接
                var db = DoNet.Common.DbUtility.DbFactory.CreateDbORM("dbconnection");
               
                if (db.ExecuteNonQuery(sql, pars) > 0)
                {
                    BindData(1);//转到第一页
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('新增成功')", true);                    
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('新增失败')", true);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('" + ex.Message.Replace("\n","") + "')", true);
            }
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Paging1_OnPageChange(object sender, Web.PagingEventArg e)
        {
            BindData(e.PageIndex);//转到第几页
        }

        /// <summary>
        /// 表名
        /// </summary>
        const string TableName = "jm_table";
        /// <summary>
        /// 表字段
        /// </summary>
        const string Fields = "mark,username,age";
        /// <summary>
        /// 每页显示条数
        /// </summary>
        const int CountInPage = 10;
        /// <summary>
        /// 字段映射
        /// 格式是:对象属性|表字段
        /// </summary>
        const string Mapping = "Mark|Mark,Name|username,Age|age";

        /// <summary>
        /// 绑定第几页的数据
        /// </summary>
        /// <param name="p"></param>
        private void BindData(int p)
        {
            //通过web.config中的配置创建连接
            var db = DoNet.Common.DbUtility.DbFactory.CreateDbORM("dbconnection");

            //查询，通过age逆序分页
            var data = db.SearchDataPage<JMUser>(TableName, Fields, "age > 16 and mark is not null", p, CountInPage, "Age desc", Mapping);

            GridView1.DataSource = data.Data;
            GridView1.DataBind();

            Paging1.PageIndex = data.Index;//第几页

            Paging1.PageCount = data.PageCount;//总页数


            //获取所有记录
            //var alldata = db.GetDataCollectionBySql<JMUser>(string.Format("select {0} from {1}", Fields, TableName), Mapping);
        }
    }

    /// <summary>
    /// 跟表jm-table对应
    /// </summary>
    public class JMUser
    {
        public string Mark { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
