<%@ Page Language="C#" AutoEventWireup="true" CodeFile="page.aspx.cs" Inherits="page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" id="refreshPage" runat="server"/>

    <link id="cssStyle" runat="server" href="css/views/page/default.css" rel="stylesheet" />
    
    <!-- JQuery -->
    <script src="js/jquery-1.9.0.js"></script>
    
    <!-- Site  -->
    <script src="js/views/page/page.js"></script>

    <title id="pageTitle"></title>
</head>
<body id="pageBody" runat="server">
    <form id="mform" runat="server">
        <div id="posDiv" runat="server">
            <asp:Panel id="mPage" runat="server" />
        </div>
    </form>
</body>
</html>
