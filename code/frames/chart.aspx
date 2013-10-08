<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chart.aspx.cs" Inherits="frames_chart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    
    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>

    <!-- Site -->
    <link href="~/css/views/frames/default.css" rel="stylesheet" />
    <link id="cssStyle" runat="server" href="~/css/views/frames/default_chart.css" rel="stylesheet" />
    
    <script src="../js/views/frames/chart.js"></script>
    <script src="../js/generic.js"></script>

    <title id="headerTitle" runat="server"></title>
</head>
<body>
    <form id="mform" runat="server">
        
        <div id="mPanel" runat="server" class="mpanel">
            
            <div id="lbTitleContainer" runat="server" class="title_container">
                <div id="lbTitle" runat="server" class="title"></div>
            </div>

            <div class="small_divider"></div>
            <asp:Chart ID="mChart" runat="server">
                <ChartAreas>
                    <asp:ChartArea Name="mChartArea"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
    </form>
</body>
</html>
