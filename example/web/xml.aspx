<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xml.aspx.cs" Inherits="DoNet.Common.Examples.web.xml" %>

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
    <asp:Button ID="Button1" runat="server" Text="读取XML" />
    <div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    </form>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">
        ///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        </span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;生成XML文档</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        </span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        </span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">void</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;CreateXml()<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        {<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//生成XML文档</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;xmlwr&nbsp;=&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;DoNet.Common.IO.</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlWR</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">();<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;xmlwr.CreateNewXML();<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;root&nbsp;=&nbsp;xmlwr.CreateNewNode(</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlNodeType</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.Element,</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;root&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//创建root的子节点，并生成属性</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;attr=</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">Dictionary</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&lt;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;();<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;attr.Add(</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;key&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;key1&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;attr.Add(</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;value&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;v1&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;xn&nbsp;=&nbsp;xmlwr.CreateNewNode(root,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlNodeType</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.Element,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;item1&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;attr);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//创建xn的子节点，并创建属性</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;attr&nbsp;=&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">Dictionary</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&lt;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">string</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&gt;();<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;attr.Add(</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;abd&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;key1&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;attr.Add(</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;fsdfsf&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;v1&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;xnh&nbsp;=&nbsp;xmlwr.CreateNewNode(xn,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlNodeType</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.Element,&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;item11&quot;</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;attr);<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//展示</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;Literal1.Text&nbsp;=&nbsp;xmlwr.ToString();<o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
        </span>
    </p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//调用save可以保存为本地文档</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        &nbsp;&nbsp;&nbsp;&nbsp;</span><span 
            style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//xmlwr.Save(&quot;c:\\1.xml&quot;);</span><span 
            style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
    <p class="style1">
        <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
        }<o:p></o:p></span></p>
    <p class="style1">
        &nbsp;</p>
    <p class="style1">
        &nbsp;</p>
    <div style="layout-grid: 15.6000pt; page: Section0;">
        <p class="style1">
            <span style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            ///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;读取XML操作</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">///</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(128,128,128); font-size:9.5000pt; font-family:'新宋体'; ">&lt;/summary&gt;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">void</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;ReadXml()<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//生成XML文档</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//也可以输入文件地址读取</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;xmlwr&nbsp;=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">new</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;DoNet.Common.IO.</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlWR</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">();<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;xmlwr.LoadXmlString(Literal1.Text);</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//读取XML字符</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//查找父节点为root。且节点名为item1的节点</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;xn1&nbsp;=&nbsp;xmlwr.GetNodeByDelegate((</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlNode</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;x)&nbsp;=&gt;<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">return</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;x.ParentNode&nbsp;!=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">null</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&amp;&amp;<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;x.ParentNode.Name.Equals(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;root&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">StringComparison</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.OrdinalIgnoreCase)&nbsp;&amp;&amp;<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;x.Name.Equals(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;item1&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">StringComparison</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.OrdinalIgnoreCase);<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;});<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">if</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;(xn1&nbsp;!=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">null</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">)<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Response.Write(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;item1:&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;+&nbsp;DoNet.Common.Text.</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">StringMg</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.ToHtml(xn1.InnerXml));<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//获取root下的子节点</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;xns&nbsp;=&nbsp;xmlwr.GetNodeListByDelegate((</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">XmlNode</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;x)&nbsp;=&gt;<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">return</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;x.ParentNode&nbsp;!=&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">null</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;&amp;&amp;<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;x.ParentNode.Name.Equals(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;root&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">,&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(43,145,175); font-size:9.5000pt; font-family:'新宋体'; ">StringComparison</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">.OrdinalIgnoreCase);<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;});<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//显示个数</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Response.Write(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;&lt;br&nbsp;/&gt;&nbsp;root&nbsp;children:&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;+&nbsp;xns.Count);<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//把所有属性转为集合</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,128,0); font-size:9.5000pt; font-family:'新宋体'; ">//第一个参数表示是否转为最小化</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;dic&nbsp;=&nbsp;xmlwr.TurnXmlToDictionary(</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">false</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">);<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; "><o:p></o:p>
            </span>
        </p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">foreach</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;(</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">var</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;d&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(0,0,255); font-size:9.5000pt; font-family:'新宋体'; ">in</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;dic)<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Response.Write(</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;&lt;br&nbsp;/&gt;&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;+&nbsp;d.Key&nbsp;+&nbsp;</span><span 
                style="mso-spacerun:'yes'; color:rgb(163,21,21); font-size:9.5000pt; font-family:'新宋体'; ">&quot;:&quot;</span><span 
                style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">&nbsp;+&nbsp;d.Value);<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:9.5000pt; font-family:'新宋体'; ">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<o:p></o:p></span></p>
        <p class="style1">
            <span style="mso-spacerun:'yes'; font-size:10.5000pt; font-family:'Times New Roman'; "><o:p></o:p>
            </span>
        </p>
    </div>
</body>
</html>
