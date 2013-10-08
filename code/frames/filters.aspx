<%@ Page Language="C#" AutoEventWireup="true" CodeFile="filters.aspx.cs" Inherits="frames_filters" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link href="~/css/views/frames/default.css" rel="stylesheet" />
    <link id="cssStyle" runat="server" href="~/css/views/frames/default_filters.css" rel="stylesheet" />
    
    <script src="../js/generic.js"></script>


    <title id="headerTitle" runat="server"></title>
</head>
<body>
    <form id="mform" runat="server">
        <div id="mPanel" runat="server" class="mpanel">
            
            <div id="lbTitleContainer" runat="server" class="title_container">
                    <div id="lbTitle" runat="server" class="title"></div>
            </div>
            
            <div id="filterPanel" runat="server"></div>
            
            <div class="buttons_container">
                <asp:Button id="btFilter" Text="filter" runat="server" OnClick="btFilter_Click" CssClass="filter_buttons"/>
                <asp:Button id="btClear" Text="clear" runat="server" OnClick="btClear_Click" CssClass="filter_buttons"/>           
            </div>
        </div>
    </form>
</body>
</html>
