<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cookies.aspx.cs" Inherits="DoNet.Common.Examples.web.cookies" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">


p.p0{
margin:0pt;
margin-bottom:0.0001pt;
margin-bottom:0pt;
margin-top:0pt;
text-align:justify;
font-size:10.5000pt; font-family:'Times New Roman'; }
            table
            {
            border: 1px solid black;
            margin-bottom:30px;
            margin-top: 5px;
            }
            td {
            font-size:12pt;
            border-collapse:collapse;
            border-spacing:0;
            border-bottom: 1px solid black;
            }
            </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br /><br />
        key:<asp:TextBox ID="txtkey" runat="server" Text="test"></asp:TextBox><br /><br />
        value:<asp:TextBox ID="txtvalue" Text="testvalue"
            runat="server"></asp:TextBox>
            <br />
        <asp:Button ID="btnget" runat="server" Text="读" />
        <asp:Button ID="btnwrite" runat="server" Text="写" /><br /><br />
    </div>
    </form>
    <p class="p0" 
        style="margin-left:21.0000pt; text-indent:21.0000pt; margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp; 
        ///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;写</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;param&nbsp;name=&quot;sender&quot;&gt;&lt;/param&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;param&nbsp;name=&quot;e&quot;&gt;&lt;/param&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">void</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;btnwrite_Click(</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">object</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;sender,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">EventArgs</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;e)<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//写入cookie</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//10分钟有效期</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;DoNet.Common.Web.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">Cookie</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.Write(txtkey.Text,&nbsp;txtvalue.Text,&nbsp;10);<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;读</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;param&nbsp;name=&quot;sender&quot;&gt;&lt;/param&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;param&nbsp;name=&quot;e&quot;&gt;&lt;/param&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">void</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;btnget_Click(</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">object</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;sender,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">EventArgs</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;e)<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;txtvalue.Text&nbsp;=&nbsp;DoNet.Common.Web.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">Cookie</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.Read(txtkey.Text);<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom:0pt; margin-top:0pt; ">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<o:p></o:p></span></p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <div class="CookieRecord" xmlns="">
        <table cellpadding="0" cellspacing="0" width="400">
            <tr>
                <td class="title">
                    NAME
                </td>
                <td>
                    test</td>
            </tr>
            <tr>
                <td class="title">
                    VALUE
                </td>
                <td>
                    testvalue</td>
            </tr>
            <tr>
                <td class="title">
                    DOMAIN
                </td>
                <td>
                    localhost</td>
            </tr>
            <tr>
                <td class="title">
                    PATH
                </td>
                <td>
                    /</td>
            </tr>
            <tr>
                <td class="title">
                    EXPIRES
                </td>
                <td>
                    2011/8/3 10:42:19</td>
            </tr>
        </table>
    </div>
</body>
</html>
