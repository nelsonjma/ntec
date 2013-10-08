<%@ Page Language="C#" AutoEventWireup="true" CodeFile="table.aspx.cs" Inherits="frames_table" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link href="~/css/views/frames/default.css" rel="stylesheet" />
    <link id="cssStyle" runat="server" href="~/css/views/frames/default_table.css" rel="stylesheet" />
    
    <script src="../js/views/frames/table.js"></script>
    <script src="../js/generic.js"></script>
    
    <title id="headerTitle" runat="server"></title>
</head>
<body>
    <form id="mForm" runat="server">
        <div id="mPanel" runat="server" class="mpanel">
        
            <div id="lbTitleContainer" runat="server" class="title_container">
                <div id="lbTitle" runat="server" class="title"></div>
            </div>
            
            <div id="table_container">
                <asp:Label ID="lbCount" runat="server" Text="" Visible="false" CssClass="label_counter"/>
                <a id="lbDownload" runat="server" href="#" target="_blank" class="label_counter">Download</a>
                <asp:Label ID="lbFilterText" runat="server" Text="Filters:" Visible="false" CssClass="filter_label"/>
                <asp:TextBox ID="tbFilters" runat="server" Width="200px" Visible="false" CssClass="filter_textbox"/>
                <asp:Button ID="btFilters" runat="server" Text="Apply" Visible="false" OnClick="btFilters_Click" CssClass="filter_buttons" />
                <asp:Button ID="btClearFilter" runat="server" Text="Clear" Visible="false" OnClick="btClearFilters_Click" CssClass="filter_buttons" />
                <div class="small_divider"></div>
                <asp:GridView id="mGridView" runat="server" OnRowDataBound="mGridView_RowDataBound" OnRowCreated="mGridView_OnRowCreated"></asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
