<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="db.aspx.cs" Inherits="DoNet.Common.Examples.web.db" %>
<%@ Register Assembly="DoNet.Common.Web" Namespace="DoNet.Common.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://www.jm47.com/static/css/jm.paging.css" rel="stylesheet" type="text/css" />
    <style type="text/css">


p.p0{
margin:0pt;
margin-bottom:0.0001pt;
margin-bottom:0pt;
margin-top:0pt;
text-align:justify;
font-size:10.5000pt; font-family:'Times New Roman'; }
        .style1
        {
            color: #339933;
            background-color: #3333CC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="新增用户" /><br />
        用户名称：<asp:TextBox ID="txtname" runat="server" MaxLength="8"></asp:TextBox><br />
        用户年龄：<asp:TextBox ID="txtage" runat="server" MaxLength="3"></asp:TextBox><br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <cc1:Paging ID="Paging1" runat="server" CssClass="jm_paging" />
    </div>
    </form>
    <div style="layout-grid: 15.6000pt; page: Section0;">
        <p class="p0" 
            style="color: #339933; font-size: x-large; background-color: #3333CC">
            <strong>web.config中的配置如下：</strong></p>
        <p class="p0" 
            style="color: #339933; font-size: x-large; background-color: #3333CC">
            &nbsp;</p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
            &lt;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">connectionStrings</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&lt;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">add</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">name</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&quot;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">dbconnection</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&quot;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">connectionString</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&quot;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">Data&nbsp;Source=|DataDirectory|test.db3;charset=utf8;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&quot;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">providerName</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&quot;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">System.Data.SQLite</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&quot;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;/&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&lt;/</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">connectionStrings</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&gt;<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            &nbsp;</p>
        <p class="p0" 
            style="margin-bottom:0pt; margin-top:0pt; color: #00CC00; font-size: x-large; background-color: #0066FF;">
            <strong>执行sql语句示例</strong></p>
        <p class="p0" 
            style="margin-bottom:0pt; margin-top:0pt; color: #00CC00; font-size: x-large; background-color: #0066FF;">
            &nbsp;</p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
            var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;age&nbsp;=&nbsp;0;<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//这里简单的判断下</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">if</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;(!</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">int</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.TryParse(txtage.Text,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">out</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;age)&nbsp;||&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.IsNullOrWhiteSpace(txtname.Text))<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            {<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;Page.ClientScript.RegisterStartupScript(</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">this</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.GetType(),&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;error&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;alert(&#39;用户名或年龄格式不正确&#39;)&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">true</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">return</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            }<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//新增SQL语句</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;sql&nbsp;=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.Format(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;insert&nbsp;into&nbsp;{0}({1})&nbsp;values(@mark,@username,@age)&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;TableName,&nbsp;Fields);<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//传参执行语句</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//如果不传参也可以直接拼SQL语句,只要能保证参数特殊符号</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;pars&nbsp;=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">Dictionary</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&lt;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">object</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;();<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            pars.Add(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;@mark&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">Guid</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.NewGuid().ToString(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;n&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">));<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            pars.Add(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;@username&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;txtname.Text);<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            pars.Add(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;@age&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;txtage.Text);<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//通过web.config中的配置创建连接</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;db&nbsp;=&nbsp;DoNet.Common.DbUtility.</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">DbFactory</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.CreateDbORM(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;dbconnection&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">if</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;(db.ExecuteNonQuery(sql,&nbsp;pars)&nbsp;&gt;&nbsp;0)<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            {<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;BindData(1);</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//转到第一页</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;Page.ClientScript.RegisterStartupScript(</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">this</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.GetType(),&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;error&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;alert(&#39;新增成功&#39;)&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">true</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);&nbsp;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            }<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">else</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            {<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;Page.ClientScript.RegisterStartupScript(</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">this</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.GetType(),&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;error&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;alert(&#39;新增失败&#39;)&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">true</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            }<o:p></o:p></span></p>
        <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
            <span style="mso-spacerun:'yes'; font-size:10.5000pt; font-family:'Times New Roman'; "><o:p></o:p>
            </span>
        </p>
    </div>
    <p class="p0" 
        style="font-size: x-large; margin-top: 0pt; margin-bottom: 0pt; background-color: #3333CC">
        <strong><span class="style1">执行查询示例</span></strong></p>
    <p class="p0" 
        style="font-size: x-large; margin-top: 0pt; margin-bottom: 0pt; background-color: #3333CC">
        &nbsp;</p>
    <div style="layout-grid: 15.6000pt; page: Section0;">
        <p class="p0">
            <span style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">
            ///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;表名</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">const</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;TableName&nbsp;=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;jm_table&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;表字段</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">const</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;Fields&nbsp;=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;mark,username,age&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;每页显示条数</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">const</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">int</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;CountInPage&nbsp;=&nbsp;10;<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;字段映射</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;格式是:对象属性|表字段</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">const</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;Mapping&nbsp;=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;Mark|Mark,Name|username,Age|age&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;绑定第几页的数据</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;param&nbsp;name=&quot;p&quot;&gt;&lt;/param&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            </span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">private</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">void</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;BindData(</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">int</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;p)<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            {<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//通过web.config中的配置创建连接</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;db&nbsp;=&nbsp;DoNet.Common.DbUtility.</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">DbFactory</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.CreateDbORM(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;dbconnection&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//查询，通过age逆序分页</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;data&nbsp;=&nbsp;db.SearchDataPage&lt;</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">JMUser</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;(TableName,&nbsp;Fields,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;age&nbsp;&gt;&nbsp;16&nbsp;and&nbsp;mark&nbsp;is&nbsp;not&nbsp;null&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;p,&nbsp;CountInPage,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;Age&nbsp;desc&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;Mapping);<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;GridView1.DataSource&nbsp;=&nbsp;data.Data;<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;GridView1.DataBind();<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;Paging1.PageIndex&nbsp;=&nbsp;data.Index;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//第几页</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;Paging1.PageCount&nbsp;=&nbsp;data.PageCount;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//总页数</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//获取所有记录</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//var&nbsp;alldata&nbsp;=&nbsp;db.GetDataCollectionBySql&lt;JMUser&gt;(string.Format(&quot;select&nbsp;{0}&nbsp;from&nbsp;{1}&quot;,&nbsp;Fields,&nbsp;TableName),&nbsp;Mapping);</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            }<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun:'yes'; font-size:10.5000pt; font-family:'Times New Roman'; "><o:p></o:p>
            </span>
        </p>
    </div>
</body>
</html>
