<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mon.aspx.cs" Inherits="mon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <!-- RGraph -->
    <script src="../js/RGraph/libraries/RGraph.common.core.js" ></script>
    <script src="../js/RGraph/libraries/RGraph.common.dynamic.js" ></script>
    <script src="../js/RGraph/libraries/RGraph.common.effects.js" ></script>

    <script src="../js/RGraph/libraries/RGraph.gauge.js" ></script>
    <script src="../js/RGraph/libraries/RGraph.meter.js" ></script>
    <!--[if lt IE 9]><script src="../js/RGraph/excanvas/excanvas.js"></script><![endif]-->
    
    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link href="~/css/views/frames/default.css" rel="stylesheet" />
    <link id="cssStyle" runat="server" href="~/css/views/frames/default_mon.css" rel="stylesheet" />

    <script src="../js/generic.js"></script>
    <script src="../js/views/frames/mon.js"></script>

    <title id="headerTitle" runat="server"></title>
</head>
<body>
    <form id="mform" runat="server">
        <input id="webServiceKey" type="hidden" runat="server" />

        <div id="mPanel" runat="server" class="mpanel">

            <div id="lbTitleContainer" runat="server" class="title_container">
                <div id="lbTitle" runat="server" class="title"></div>
            </div>
            <canvas id="cvs" runat="server">[browser does not suport canvas]</canvas>
            <div id="monValue"></div>
        </div>
    </form>
</body>
</html>
