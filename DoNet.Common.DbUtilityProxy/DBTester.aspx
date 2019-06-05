<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBTester.aspx.cs" Inherits="DoNet.Common.DbUtilityProxy.DBTester" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
        <tr>
            <td width="100">
                数据库类型:
            </td>
            <td>
                <asp:DropDownList ID="ddlDbType" runat="server">
                    <asp:ListItem Text="MySql" Value="MySql"></asp:ListItem>
                    <asp:ListItem Text="SqlServer" Value="SqlServer"></asp:ListItem>
                    <asp:ListItem Text="Oracle" Value="Oracle"></asp:ListItem>
                    <asp:ListItem Text="Sqlite" Value="Sqlite"></asp:ListItem>
                    <asp:ListItem Text="Npgsql" Value="Npgsql"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                库名：
            </td>
            <td>
                <asp:TextBox ID="txtdb" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td width="100">
                编码:
            </td>
            <td>
               <asp:TextBox ID="txtcharset" runat="server"></asp:TextBox>
            </td>
            <td>
               
            </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td>
                IP:
            </td>
            <td>
                <asp:TextBox ID="txtip" runat="server" Text="localhost"></asp:TextBox>
            </td>
            <td>
                PORT:
            </td>
            <td>
                <asp:TextBox ID="txtport" runat="server" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                User:
            </td>
            <td>
                <asp:TextBox ID="txtuser" runat="server"></asp:TextBox>
            </td>
            <td>
                Password:
            </td>
            <td>
                <asp:TextBox ID="txtpwd" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                执行的SQL：
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtsql" runat="server" TextMode="MultiLine" Width="100%" Height="250"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnok" runat="server" Text="执行" />
            </td>
        </tr>
    </table>
    <div id="gridcontainer" runat="server">
        
    </div>
    </form>
</body>
</html>
