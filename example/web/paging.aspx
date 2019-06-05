<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paging.aspx.cs" Inherits="DoNet.Common.Examples.web.paging" %>

<%@ Register Assembly="DoNet.Common.Web" Namespace="DoNet.Common.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://www.jm47.com/static/css/jm.paging.css" rel="stylesheet" type="text/css" />
<style type="text/css">
/*分页样式
.jm_paging
{
    font-size:16px;
    margin-top:10px;
}
.jm_paging a
{
    padding: 4px 8px;
    background: #fff;
    border: 1px solid #9AAFE5;   
    color: #2452ac;
    margin:10px 4px; 
    text-decoration:none;
}
.jm_paging a:hover
{
    border: 1px solid #2452ac;
    color:Green;    
    cursor: pointer;
}
.jm_paging a.jm_curPage
{
    border: 1px solid #2452ac;
    background-color: #2452ac;   
    color: #fff;   
}
转向输入框
.jm_paging input
{
    margin:16px 4px; 
    width:20px;
    height:20px;
}*/
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
    <asp:GridView ID="GridView1" runat="server" BackColor="White" 
        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        GridLines="Vertical" Height="129px" Width="548px">
        <AlternatingRowStyle BackColor="#DCDCDC" />
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#0000A9" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#000065" />
    </asp:GridView>
    <div style="clear:both;">
    <cc1:Paging ID="Paging1" runat="server" CssClass="jm_paging" />
    </div>
    <p>
        CSS:</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        &lt;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">style</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">type</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;text/css&quot;&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,100,0); font-size:9.5000pt; font-family:'新宋体'; ">/*分页样式*/</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">
        .jm_paging</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">font-size</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">16px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">
        .jm_paging</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">a</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">padding</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">4px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">8px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">background</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#fff</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">border</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">1px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">solid</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#9AAFE5</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">color</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#2452ac</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">margin</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">10px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">4px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">text-decoration</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">none</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">
        .jm_paging</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">a:hover</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">border</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">1px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">solid</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#2452ac</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">color</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">Green</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;&nbsp;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">cursor</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">pointer</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">
        .jm_paging</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">a.jm_curPage</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">border</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">1px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">solid</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#2452ac</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">background-color</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#2452ac</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">color</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">#fff</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,100,0); font-size:9.5000pt; font-family:'新宋体'; ">
        /*转向输入框*/</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">
        .jm_paging</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">input</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">margin</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">16px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">4px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;&nbsp;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">width</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">20px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">height</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">20px</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">;<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        &lt;/</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">style</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">&gt;<o:p></o:p></span></p>
    <p>
        &nbsp;</p>
    <p>
        前台：</p>
    <p class="style1">
        <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; background: rgb(255,255,0); mso-highlight: rgb(255,255,0); font-family: '新宋体';">
        &lt;%</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">@</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">Register</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">Assembly</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;DoNet.Common.Web&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">Namespace</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;DoNet.Common.Web&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">TagPrefix</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;cc1&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun: 'yes'; font-size: 9.5000pt; background: rgb(255,255,0); mso-highlight: rgb(255,255,0); font-family: '新宋体';">%&gt;<o:p></o:p></span></p>
    
   
    </form>
   
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        &lt;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">cc1</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">:</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,0,0); font-size:9.5000pt; font-family:'新宋体'; ">Paging</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">ID</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;Paging2&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">runat</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;server&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(255,0,0); font-size:9.5000pt; font-family:'新宋体'; ">CssClass</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">=&quot;jm_paging&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">/&gt;<o:p></o:p></span></p>
    <p class="style1">
        <o:p></o:p>
    </p>
    <p class="style1">
        <o:p>后台：</o:p></p>
    <p class="style1">
        <o:p></o:p>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        protected</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">void</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;Page_Load(</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">object</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;sender,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">EventArgs</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;e)<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//绑定翻页事件</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;Paging1.OnPageChange&nbsp;+=&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">EventHandler</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&lt;DoNet.Common.Web.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">PagingEventArg</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;(Paging1_OnPageChange);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">if</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;(!Page.IsPostBack)<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//初始化页码</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//根据您查得的记录来赋值。这里写死</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Paging1.PageIndex&nbsp;=&nbsp;1;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//当前页</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Paging1.PageCount&nbsp;=&nbsp;100;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//查得记录总共多少页</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;}<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">
        void</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;Paging1_OnPageChange(</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">object</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;sender,&nbsp;DoNet.Common.Web.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">PagingEventArg</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;e)<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;{<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;Response.Write(</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;index:&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;+&nbsp;e.PageIndex&nbsp;+&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;&nbsp;count:&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;+&nbsp;e.PageCount);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;}<o:p></o:p></span></p>
    <p class="style1">
        <o:p></o:p></p>
   
</body>
</html>

