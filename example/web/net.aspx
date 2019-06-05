<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="net.aspx.cs" Inherits="DoNet.Common.Examples.web.net" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            text-align: justify;
            font-size: 10.5000pt;
            font-family: "Times New Roman";
            margin: 0pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
    测试邮件发送:<br />
        smtp:<asp:TextBox ID="txtsmtp" runat="server" placeholder="smtp.jiamaocode.com"></asp:TextBox><br />
        收件人:<asp:TextBox ID="txtreceiver" runat="server" placeholder="jiamao@jiamaocode.com"></asp:TextBox><br />
        发件人邮箱:<asp:TextBox ID="txtmail" runat="server" placeholder="jiamao@jiamaocode.com"></asp:TextBox><br />
        发件人:<asp:TextBox ID="txtuser" runat="server" placeholder="jiamao@jiamaocode.com"></asp:TextBox><br />
        发件人密码:<asp:TextBox ID="txtpwd" runat="server"></asp:TextBox><br />
        邮件标题:<asp:TextBox ID="txtsubject" runat="server"></asp:TextBox><br />
        邮件内容:<asp:TextBox ID="txtcontent" runat="server"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="发送" />
    </div>

    </form>
    邮件发送说明请查看：<a href="http://www.jm47.com/document/1008">http://www.jm47.com/document/1008</a>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        DoNet.Common.Net.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">MailMg</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;mail&nbsp;=&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;DoNet.Common.Net.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">MailMg</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">();<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">
        //发送</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">
        //如果需要发送附件等请看其它重载</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        mail.QueueSend(txtsmtp.Text,&nbsp;txtmail.Text,&nbsp;txtuser.Text,&nbsp;txtpwd.Text,&nbsp;txtreceiver.Text,&nbsp;txtsubject.Text,&nbsp;txtcontent.Text,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">true</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>

            WCF等类库就不说了，想了解的自已可以看源码了哦！！！

            有需要的请联系本人
</body>
</html>
