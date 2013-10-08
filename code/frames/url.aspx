<%@ Page Language="C#" AutoEventWireup="true" CodeFile="url.aspx.cs" Inherits="frames_url" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link href="~/css/views/frames/default.css" rel="stylesheet" />
    <link id="cssStyle" runat="server" href="~/css/views/frames/default_mon.css" rel="stylesheet" />

    <script src="../js/generic.js"></script>

    <title id="headerTitle" runat="server"></title>

</head>
<body>
    <form id="mform" runat="server">
        <div id="mPanel" runat="server" class="mpanel">

            <div id="lbTitleContainer" runat="server" class="title_container">
                <div id="lbTitle" runat="server" class="title"></div>
            </div>
            
            <div id="url_contailer">
                <iframe id="url" runat="server"></iframe>    
            </div>
            
        </div>
    </form>
</body>
</html>
