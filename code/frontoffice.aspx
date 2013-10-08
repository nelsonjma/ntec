<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frontoffice.aspx.cs" Inherits="frontoffice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" /> 
    
    <!-- Logo -->
    <link rel="shortcut icon" href="favicon.ico" />

    <!-- JQuery -->
    <script src="js/jquery-1.9.0.js"></script>
    
    <!-- Site  -->    
    <link id="cssStyle" runat="server" href="css/views/frontoffice/default.css" rel="stylesheet" />

    <script src="js/generic.js"></script>

    <title>FrontOffice</title>
</head>
<body>
    <form id="mform" runat="server">
        <div id="headerContainer" runat="server"></div>
        <iframe id="pageContainer" runat="server"></iframe>
    
    <script type="text/javascript">
        $(window).resize(function ()    { $('iframe').attr('height', (getDocHeight() - 26) + "px"); });
        $(document).ready(function ()   { $('iframe').attr('height', (getDocHeight() - 26) + "px"); });
        
        $(document).ready(function () {

            $('a').click(function (e) {
                if ($(this).html() != 'Home') {
                    $('iframe').attr('src', $(this).attr('href')); // if its home refresh page...
                    e.preventDefault();
                }
            });
            
            $('#login').on("click", '#btcustom', function () {
                $('iframe').attr('src', $(this).attr('href')); // open the user page 
            });
        });
    </script>
    </form>
</body>
</html>
