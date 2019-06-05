<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IOMG.aspx.cs" Inherits="DoNet.Common.Examples.web.logger" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        p.p0
        {
            margin: 0pt;
            margin-bottom: 0.0001pt;
            margin-bottom: 0pt;
            margin-top: 0pt;
            text-align: justify;
            font-size: 10.5000pt;
            font-family: 'Times New Roman';
        }
        .style1
        {
            color: #339933;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="写日志" OnClientClick="javascript:return confirm('确定要写日志?');" />
        &nbsp;点此触发写日志<br />
        <br />
    </div>
    </form>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">DoNet.Common.IO.</span><span
            style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">Logger</span>.SetLogPath(&quot;log&quot;);<span
                class="style1">//设置日志目录为当前目录下的log文件夹，此句可省。默认就为log目录</span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        &nbsp;</p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
            //写调试日志，必须得config中配置debug=true项，如：</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt;
                font-family: '新宋体';"><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
            //&nbsp;&lt;appSettings&gt;</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt;
                font-family: '新宋体';"><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
            //&nbsp;&nbsp;&nbsp;&lt;add&nbsp;key=&quot;debug&quot;&nbsp;value=&quot;true&quot;/&gt;</span><span
                style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';"><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
            //&nbsp;&lt;/appSettings&gt;</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt;
                font-family: '新宋体';"><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">DoNet.Common.IO.</span><span
            style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">Logger</span><span
                style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">.Debug(</span><span
                    style="mso-spacerun: 'yes'; color: rgb(163,21,21); font-size: 9.5000pt; font-family: '新宋体';">&quot;测试&quot;</span><span
                        style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">,&nbsp;</span><span
                            style="mso-spacerun: 'yes'; color: rgb(163,21,21); font-size: 9.5000pt; font-family: '新宋体';">&quot;debug日志&quot;</span><span
                                style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">);<o:p></o:p></span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">
            <o:p></o:p>
        </span>
    </p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
            //此日志不受debug影响</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';"><o:p></o:p></span></p>
    <p class="p0" style="margin-bottom: 0pt; margin-top: 0pt;">
        <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">DoNet.Common.IO.</span><span
            style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">Logger</span><span
                style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">.Write(</span><span
                    style="mso-spacerun: 'yes'; color: rgb(163,21,21); font-size: 9.5000pt; font-family: '新宋体';">&quot;写正常日志&quot;</span><span
                        style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">);</span><span
                            style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">//写日志，在log目录下生成当前日期的日志<o:p></o:p></span></p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        此组件可用于winform,webform,控制台都可以。为异步写入，不阻塞主进程</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
    <div style="layout-grid: 15.6000pt; page: Section0;">
        <p class="p0">
            <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">
                <o:p></o:p>
            </span>
        </p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
                //获取当前路径</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';"><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">DoNet.Common.IO.</span><span
                style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">PathMg</span><span
                    style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">.CheckWebPath(</span><span
                        style="mso-spacerun: 'yes'; color: rgb(163,21,21); font-size: 9.5000pt; font-family: '新宋体';">&quot;~&quot;</span><span
                            style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">);<o:p></o:p></span></p>
        <p class="p0">
            <o:p></o:p>
        </p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
                //获取当前目录下的日志目录路径,可获取当前目录下任意目录</span></p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; color: rgb(0,0,255); font-size: 9.5000pt; font-family: '新宋体';">
                var</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">&nbsp;logpath&nbsp;=&nbsp;Common.IO.</span><span
                    style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">PathMg</span><span
                        style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">.CheckPath(</span><span
                            style="mso-spacerun: 'yes'; color: rgb(163,21,21); font-size: 9.5000pt; font-family: '新宋体';">&quot;log&quot;</span><span
                                style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">);<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; color: rgb(0,128,0); font-size: 9.5000pt; font-family: '新宋体';">
                //组合当前路径下的日志路径</span><span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';"><o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">DoNet.Common.IO.</span><span
                style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">PathMg</span><span
                    style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">.CheckPath(logpath,&nbsp;</span><span
                        style="mso-spacerun: 'yes'; color: rgb(43,145,175); font-size: 9.5000pt; font-family: '新宋体';">DateTime</span><span
                            style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">.Now.ToString(</span><span
                                style="mso-spacerun: 'yes'; color: rgb(163,21,21); font-size: 9.5000pt; font-family: '新宋体';">&quot;yyyyMMdd&quot;</span><span
                                    style="mso-spacerun: 'yes'; font-size: 9.5000pt; font-family: '新宋体';">));<o:p></o:p></span></p>
        <p class="p0">
            <span style="mso-spacerun: 'yes'; font-size: 10.5000pt; font-family: 'Times New Roman';">
                <o:p></o:p>
            </span>
        </p>
    </div>
    <p class="p0" style="margin-top: 0pt; margin-bottom: 0pt">
        &nbsp;</p>
</body>
</html>
