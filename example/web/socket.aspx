<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="socket.aspx.cs" Inherits="JM.Common.Examples.web.socket1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <script>
        var socket;
        var host = 'ws://localhost:32738/socket.ashx';
        socket = new WebSocket(host);
        socket.onopen = function (m) {
            alert(m);

        };
        socket.onmessage = function (m) {
            alert(m);

        };
        socket.onclose = function (m) {
            alert(m);
        };
        socket.onerror = function (m) {
            alert(m);
        }
    </script>
</body>
</html>
