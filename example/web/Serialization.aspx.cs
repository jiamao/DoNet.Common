/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/5 14:40:09
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;

namespace DoNet.Common.Examples.web
{
    public partial class Serialization : System.Web.UI.Page
    {
        persion _persion;
        protected void Page_Load(object sender, EventArgs e)
        {
            _persion = new persion()
            {
                Address = "地球人",
                Age = 26,
                Name = "家猫"//,
                //dic = new Dictionary<string, object>() { {"aaa","bbbb"}}
            };

            btnxmlser.Click += new EventHandler(btnxmlser_Click);
            btnxmlder.Click += new EventHandler(btnxmlder_Click);

            btnjsonser.Click += new EventHandler(btnjsonser_Click);
            btnjsonder.Click += new EventHandler(btnjsonder_Click);
        }
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnjsonder_Click(object sender, EventArgs e)
        {
            try
            {
                //json反序列化
                var p = DoNet.Common.Serialization.JSon.JsonToModel<persion>(Literal1.Text);

                p.Name = "json反序列化出来的人";

                GridView1.DataSource = new List<persion>() { p };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnjsonser_Click(object sender, EventArgs e)
        {
            //json序列化
            Literal1.Text = DoNet.Common.Serialization.JSon.ModelToJson(_persion);
        }
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnxmlder_Click(object sender, EventArgs e)
        {
            try
            {
                //xml反序列化
                var p = DoNet.Common.Serialization.FormatterHelper.XMLDerObjectFromString<persion>(Literal1.Text);
                p.Name = "xml反序列化出来的人";
                GridView1.DataSource = new List<persion>() { p };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// xml序列化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnxmlser_Click(object sender, EventArgs e)
        {
            //xml序列化
            Literal1.Text = DoNet.Common.Serialization.FormatterHelper.XMLSerObjectToString(_persion);
        }
    }

    [Serializable]
    [DataContract]
    public class persion
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public string Address { get; set; }
        //[DataMember]
        //public Dictionary<string, object> dic { get; set; }
    }

}
