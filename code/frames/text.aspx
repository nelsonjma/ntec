<%@ Page Language="C#" AutoEventWireup="true" CodeFile="text.aspx.cs" Inherits="text" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link href="~/css/views/frames/default.css" rel="stylesheet" />
    <link id="cssStyle" runat="server" href="~/css/views/frames/default_text.css" rel="stylesheet" />
    
    <script src="../js/generic.js"></script>
    <script src="../js/views/frames/text.js"></script>
    
    <style id="customStyle" runat="server"></style>

    <title id="headerTitle" runat="server"></title>
</head>
<body>
    <form id="mForm" runat="server">
        <div id="mPanel" runat="server">
            <div id="lbTitleContainer" runat="server" class="title_container">
                <div id="lbTitle" runat="server" class="title"></div>
            </div>
    
            <div id="freeText" runat="server"></div>
        </div>
    </form>
</body>
</html>
