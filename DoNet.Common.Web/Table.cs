using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;

namespace DoNet.Common.Web
{
    public class Table
    {
        private TableRow tr = new TableRow();
        //private int iRecLength;			//记录每行字符长度
        //private int iTotalLenght;       //行的长度
        private int strFontSize = 9;
        private System.Web.UI.WebControls.Table mtb;				//要处理的表



        int _FontWidth = 9;			//字体宽度
        int _CurScreenWidth = 800;	//屏幕宽度
        int _SEQ = 1;                  //设置一个标示，给行增加样式

        public Table(System.Web.UI.WebControls.Table tb, int fCurScreenWidth, int iFontWidth)
        {
            mtb = tb;

            if (string.IsNullOrWhiteSpace(tb.CssClass)) tb.CssClass = "jm-table";

            _FontWidth = iFontWidth;

            _CurScreenWidth = fCurScreenWidth;
        }

        public Table()
        {
        }

        /// <summary>
        /// 增加表的row
        /// </summary>
        public void AddRow()
        {

            if (_SEQ % 2 == 0)
            {
                tr.CssClass = "even";                
            }
           
            _SEQ++;

            mtb.Controls.Add(tr);
            tr = new TableRow();

        }

        /// <summary>
        /// 增加表的Cell
        /// </summary>
        /// <param name = "ct"></param>
        /// <param name = "strText"></param>
        /// <param name = "strLoc">第一位align属性，第二位颜色属性</param>
        public void AddCell(System.Web.UI.WebControls.WebControl ct, string strText, params string[] strLoc)
        {
            TableCell tbcl = new TableCell();
            tbcl.Controls.Add(ct);

            if (strLoc.Length >= 1) tbcl.Attributes["align"] = strLoc[0];
            if (strLoc.Length >= 2)
            {
                tbcl.Attributes["bgcolor"] = strLoc[1];
            }
            if (strLoc.Length >= 3)
            {
                tbcl.Style["width"] = strLoc[2];
                //iRecLength += int.Parse(strLoc[2]);
            }
            tr.Controls.Add(tbcl);
        }


        public void AddCell(System.Web.UI.HtmlControls.HtmlControl ct, string strText, params string[] strLoc)
        {
            TableCell tbcl = new TableCell();
            tbcl.Controls.Add(ct);

            if (strLoc.Length >= 1) tbcl.Attributes["align"] = strLoc[0];
            if (strLoc.Length >= 2)
            {
                tbcl.Attributes["bgcolor"] = strLoc[1];
            }
            if (strLoc.Length >= 3)
            {
                tbcl.Style["width"] = strLoc[2];
                //iRecLength += int.Parse(strLoc[2]);
            }
            tr.Controls.Add(tbcl);
        }

        public void AddCell(string strText, params string[] strLoc)
        {
            TableCell tbcl = new TableCell();
            tbcl.Text = strText;

            if (strLoc.Length >= 1) tbcl.Attributes["align"] = strLoc[0];
            if (strLoc.Length >= 2)
            {
                tbcl.Attributes["bgcolor"] = strLoc[1];
            }
            if (strLoc.Length >= 3)
            {
                tbcl.Style["width"] = strLoc[2];
                //iRecLength += int.Parse(strLoc[2]);
            }
            tr.Controls.Add(tbcl);
        }

        /// <summary>
        /// 调整表的宽度
        /// </summary>
        public void SetWidth()
        {
            //if ((iTotalLenght / f_CurScreenWidth) < 1)
            //{
            //    mtb.Width = Unit.Parse("100%");
            //}
            //else
            //{
            //    mtb.Width = Unit.Parse(iTotalLenght.ToString());

            //}
        }
        /// <summary>
        /// 增加表头
        /// </summary>
        /// <param name = "strTableTitle"></param>
        /// <param name = "tb"></param>
        public void CreateTitle(params string[] strTableTitle)
        {
           
            TableHeaderRow tr;
            TableHeaderCell htc;
            Label lblData;
            tr = new TableHeaderRow();
            tr.Height = Unit.Parse("20px");
            tr.Style["Font-Names"] = "宋体";
            tr.Font.Size = strFontSize;
            
            for (int i = 0; i < strTableTitle.Length; i++)
            {
                htc = new TableHeaderCell();
                htc.Attributes["align"] = "center";
                if (i == 0 && strTableTitle[0].Trim() == "") htc.Style["Width"] = "7";
                lblData = new Label(); ;
                lblData.EnableViewState = false;
                lblData.Text = strTableTitle[i];
                //lblData.ForeColor = Color.FromName("#0000CC");

                //htc.BackColor = Color.FromName("#DBEAF5");//Color.FromName("#4682B4");
                htc.Controls.Add(lblData);
                tr.Controls.Add(htc);

                //iRecLength += iCaluFldLength(strTableTitle[i]);
            }
            mtb.Controls.Add(tr);
            SetWidth();
        }
        /// <summary>
        /// 增加表头
        /// </summary>
        /// <param name = "strTableTitle"></param>
        /// <param name = "tb"></param>
        public void CreateTitle(System.Web.UI.HtmlControls.HtmlControl ct, params string[] strTableTitle)
        {
            TableRow tr;
            TableCell htc;
            Label lblData;
            //
            tr = new TableRow();   
            htc = new TableCell();
            htc.Controls.Add(ct);
            
            tr.Controls.Add(htc);

            //
            for (int i = 0; i < strTableTitle.Length; i++)
            {
                htc = new TableCell();
                lblData = new Label();
                lblData.ID = "label_" + mtb.ID + i.ToString();
                lblData.EnableViewState = false;
                lblData.Text = strTableTitle[i];
              
                htc.Controls.Add(lblData);
                tr.Controls.Add(htc);

            }
            mtb.Controls.Add(tr);
            SetWidth();
        }

        /// <summary>
        /// 增加表头
        /// </summary>
        /// <param name = "strTableTitle"></param>
        /// <param name = "tb"></param>
        public void CreateTitle(System.Web.UI.WebControls.WebControl ct, params string[] strTableTitle)
        {
            TableRow tr;
            TableCell htc;
            Label lblData;
            //
            tr = new TableRow();
         
            tr.Font.Size = strFontSize;
            htc = new TableCell();
            htc.Controls.Add(ct);
            tr.Controls.Add(htc);
            for (int i = 0; i < strTableTitle.Length; i++)
            {
                htc = new TableCell();
                lblData = new Label();
                lblData.ID = "label" + mtb.ID + i.ToString();
                lblData.EnableViewState = false;
                lblData.Text = strTableTitle[i];
              
                htc.Controls.Add(lblData);
                tr.Controls.Add(htc);
            }
            mtb.Controls.Add(tr);
            SetWidth();
        }


        /// <summary>
        /// 计算列表的长度
        /// </summary>
        /// <param name = "alLength"></param>
        /// <returns></returns>
        private int iCalLength(ArrayList alLength)
        {
            //计算列表的长度

            int iDisplayCount = 0;
            for (int i = 0; i < alLength.Count; i++)
            {
                int iLength = 0;
                try
                {
                    iLength = int.Parse(alLength[i].ToString().Trim());
                }
                catch
                {
                    iLength = 0;
                }

                if (iLength > iDisplayCount) iDisplayCount = iLength;

            }

            return (int)(iDisplayCount * _FontWidth);
        }
        /// <summary>
        /// 计算一个字符串的长度

        /// </summary>
        /// <param name = "strFldValue"></param>
        /// <returns></returns>
        public int iCaluFldLength(string strFldValue)
        {
            //如果是ascii码，如字母、数值等，长度为1
            //否则为2
            if (strFldValue.Length > 100)
            {
                strFldValue = strFldValue.Substring(0, 99);
            }
            System.Text.UnicodeEncoding ueFldValueEn = new System.Text.UnicodeEncoding();

            char[] cFldValue = new char[1];
            int iFldLength = 0;
            for (int i = 0; i < strFldValue.Length; i++)
            {
                cFldValue[0] = strFldValue[i];
                Byte[] bTemp = new Byte[2];
                try
                {
                    bTemp = ueFldValueEn.GetBytes(cFldValue, 0, 1);
                }
                catch
                {
                    ;
                }

                if (bTemp[0] <= 127 && bTemp[1] == 0)
                {
                    iFldLength += 1;
                }
                else
                {
                    iFldLength += 2;
                }
            }
            return iFldLength;
        }
    }
}
