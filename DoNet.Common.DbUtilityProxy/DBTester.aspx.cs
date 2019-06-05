using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DoNet.Common.DbUtilityProxy
{
    public partial class DBTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnok.Click += new EventHandler(btnok_Click);
        }

        void btnok_Click(object sender, EventArgs e)
        {
            try
            {
                var db = DoNet.Common.DbUtility.DbFactory.CreateDbORM();
                db.DBInfo.IsProxy = "true".Equals(System.Configuration.ConfigurationManager.AppSettings["IsDBProxy"], StringComparison.OrdinalIgnoreCase);
                db.SetConnection(ddlDbType.SelectedValue, txtip.Text, int.Parse(txtport.Text), txtuser.Text, txtpwd.Text, txtdb.Text,txtcharset.Text);


                Response.Write("<font color=\"blue\">" + db.ConnectionString + "</font><br />");

                var data = db.GetDataSet(txtsql.Text);

                if (data.Tables.Count > 0)
                {
                    foreach (System.Data.DataTable dt in data.Tables)
                    {
                        if (txtcharset.Text.IndexOf("latin1", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                foreach (System.Data.DataColumn dc in dt.Columns)
                                {
                                    if (dc.DataType == typeof(string))
                                    {
                                        var v = dr[dc];
                                        if (v == null || v == DBNull.Value) continue;

                                        dr[dc] = turnlatin1string(v.ToString());
                                    }
                                }
                            }
                        }
                        AddGrid(dt);
                    }
                }
                //using (var data = db.GetDataReader(txtsql.Text))
                //{
                //    while (true)
                //    {
                //        var html = new System.Text.StringBuilder("<table>");
                //        //Response.Write("<table>");
                //        //var tb = data.GetSchemaTable();
                //        //AddGrid(tb);
                //        while (data.Read())
                //        {
                //            html.Append("<tr>");
                //            for (var i = 0; i < data.FieldCount; i++)
                //            {
                //                var value = data.GetValue(i);

                //                html.Append("<td>");
                //                html.Append(value);
                //                html.Append("</td>");
                //                DoNet.Common.IO.Logger.Debug(value);
                //            }
                //            html.Append("</tr>");
                //            DoNet.Common.IO.Logger.Debug("\n");
                //        }
                        
                //        html.Append("</table>");
                //        gridcontainer.InnerHtml += html.ToString();
                //        if (!data.NextResult()) break;
                //    }
                //}

                

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        private void AddGrid(System.Data.DataTable dt)
        { 
        //<asp:GridView ID="GridView1" runat="server" Width="100%" CellPadding="3" GridLines="Vertical"
        //    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">
        //    <AlternatingRowStyle BackColor="#DCDCDC" />
        //    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        //    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        //    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        //    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        //    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        //    <SortedAscendingCellStyle BackColor="#F1F1F1" />
        //    <SortedAscendingHeaderStyle BackColor="#0000A9" />
        //    <SortedDescendingCellStyle BackColor="#CAC9C9" />
        //    <SortedDescendingHeaderStyle BackColor="#000065" />
        //</asp:GridView>
            var grid = new GridView();
            gridcontainer.Controls.Add(grid);
            grid.DataSource = dt;
            grid.DataBind();
        }

        string turnlatin1string(string source)        {
            var en = System.Text.Encoding.GetEncoding("latin1");
            var bs = en.GetBytes(source);
            
            var a = System.Text.Encoding.Default.GetString(bs);
            return a;
        }
    }
}