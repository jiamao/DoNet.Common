<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DoNet.Common.Examples.web.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>家猫WEB库使用简例</title>
    <style type="text/css">
        body
        {
            background-color: #ccc;
        }
        a
        {
            font-size: 16px;
            color: Blue;
        }
        table
        {
            width: 100%;
        }
        td
        {
            vertical-align: top;
        }
        ul
        {
            margin: 0;
            padding: 0;
        }
        ul li
        {
            list-style: none;
            margin: 10px 0px;
        }
    </style>
    <script type="text/javascript">
        function frameAutoHeight(f) {
            try {
                if (f.contentDocument && f.contentDocument.body.offsetHeight) {
                    f.height = f.contentDocument.body.offsetHeight + 100;
                    if (f.contentDocument.body.offsetWidth) f.width = f.contentDocument.body.offsetWidth;
                } else if (f.Document && f.Document.body.scrollHeight) {
                    f.height = f.Document.body.scrollHeight + 100;
                    if (f.Document.body.scrollWidth) f.width = f.Document.body.scrollWidth
                }
            }
            catch (e)
            { }
        }
    </script>
</head>
<body>
    <table width="100%">
        <td style="width: 120px;">
            <ul>
                <li><a href="paging.aspx" target="jmtest">分页组件</a></li>
                <li><a href="cookies.aspx" target="jmtest">cookie操作</a></li>
                <li><a href="route.html" target="jmtest">Route组件</a></li>
                <li><a href="rss.aspx" target="jmtest">RSS组件</a></li>
                <li><a href="IOMG.aspx" target="jmtest">IO类库</a></li>
                <li><a href="xml.aspx" target="jmtest">xml操作组件</a></li>
                <li><a href="Serialization.aspx" target="jmtest">序列化</a></li>
                <li><a href="net.aspx" target="jmtest">网络操作类库</a></li>
                <li><a href="db.aspx" target="jmtest">数据库操作类</a></li>
            </ul>
        </td>
        <td>
            <iframe id="jmtest" name="jmtest" src="paging.aspx" width="100%" style="border: 1px solid #000;margin:0;
                min-height: 200px; background-color: #fff;" frameborder="0" onload="javascript:frameAutoHeight(this);">
            </iframe>
        </td>
    </table>
</body>
</html>
