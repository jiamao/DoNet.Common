<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rss.aspx.cs" Inherits="DoNet.Common.Examples.web.rss" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>        
        <br />
        <asp:Button ID="Button2" runat="server" Text="生成RSS" />
        <br />
        <asp:Literal ID="rssstring" Mode="Encode" runat="server"></asp:Literal>
        <br /><br />
        <asp:Button ID="Button1" runat="server" Text="读取RSS" />

        rss访问地址:<asp:TextBox ID="txtrss" runat="server" Width="60%" Text="http://www.jm47.com/jmrss"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtrssdoc" runat="server" Width="80%" TextMode="MultiLine" Height="40"></asp:TextBox>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
