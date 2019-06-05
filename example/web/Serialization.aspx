<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Serialization.aspx.cs"
    Inherits="DoNet.Common.Examples.web.Serialization" %>

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
        <asp:Button ID="btnxmlser" runat="server" Text="XML序列化" />
        <asp:Button ID="btnxmlder" runat="server" Text="XML反序列化" />
        <asp:Button ID="btnjsonser" runat="server" Text="json序列化" />
        <asp:Button ID="btnjsonder" runat="server" Text="json反序列化" />
    </div>
    <asp:Literal ID="Literal1" runat="server" Mode="Encode"></asp:Literal>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    </form>
    <p>
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        Var&nbsp;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">_persion&nbsp;=&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">persion</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">()&nbsp;{&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Address=</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;地球人&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Age=26,<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name=</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;家猫&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;};<o:p></o:p></span></p>
    <p>
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">
        //json序列化</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;DoNet.Common.Serialization.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">JSon</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.ModelToJson(_persion);<o:p></o:p></span></p>
    <p class="style1">
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">
        //json反序列化</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;p&nbsp;=&nbsp;DoNet.Common.Serialization.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">JSon</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.JsonToModel&lt;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">persion</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;(Literal1.Text);<o:p></o:p></span></p>
    <p class="style1">
        &nbsp;</p>
    <p class="style1">
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">
        //xml序列化</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        Literal1.Text&nbsp;=&nbsp;DoNet.Common.Serialization.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">FormatterMg</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.XMLSerObjectToString(_persion);<o:p></o:p></span></p>
    <p class="style1">
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">
        //xml反序列化</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;p&nbsp;=&nbsp;DoNet.Common.Serialization.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">FormatterMg</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.XMLDerObjectFromString&lt;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">persion</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;(Literal1.Text);<o:p></o:p></span></p>
</body>
</html>
