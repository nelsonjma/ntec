<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="cfg_frontoffice.aspx.cs" Inherits="bo_cfg_frontoffice" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    
    <!-- CodeMirror -->
    <link rel="stylesheet" href="../../js/CodeMirror/theme/vibrant-ink.css" />
    <link rel="stylesheet" href="../../js/CodeMirror/lib/codemirror.css"/>
    
    <script src="../../js/CodeMirror/lib/codemirror.js"></script>
    <script src="../../js/CodeMirror/mode/ntec.js"></script>
    <script src="../../js/CodeMirror/mode/css/css.js"></script>

    <!-- JQuery -->
    <script type="text/javascript" src="../../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link rel="stylesheet" href="../../css/views/backoffice/default.css"/>
    <link rel="stylesheet" href="../../css/views/backoffice/pages/cfg_frontoffice.css"/>
    

    <script type="text/javascript" src="../../js/generic.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/generic.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/pages/cfg_frontoffice.js"></script>

    <title>FrontOffice Config</title>
</head>
<body>
    <form id="mform" runat="server">

        <input id="webServiceKey" type="hidden" runat="server" />
        
        <div class="title_container">
            <h4 class="title">FrontOffice Config</h4>
        </div>
        
        <div class="spacer"></div>

        <div class="header">
	        <asp:Button ID="btSaveOp" runat="server" Text="Save" class="button" OnClick="Save_Click" OnClientClick="return confirm('Are you sure you want to save frontoffice options?');" />
            <asp:Button ID="btLoadOp" runat="server" Text="Load" class="button" OnClick="Load_Click" OnClientClick="return confirm('Are you sure you want to reload frontoffice options?');" />
        </div>

        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="left"><h4 class="sub_title">Options</h4></div>
                <div class="right"><h4 class="sub_title">Available Options</h4></div>
            </div>
            <div class="row">
                <div class="left"><textarea id="options" runat="server"></textarea></div>
                <div class="right"><textarea id="availabelOp" runat="server"></textarea></div>
            </div>
        </div>

        <div class="spacer"></div>

        <div class="header">
            <asp:DropDownList ID="cssSelector" runat="server" CssClass="dropdownbox"></asp:DropDownList>
            <input type="button" id="btCssLoad"  class="button" value="Load"/>
            <input type="button" id="btClearCss" class="button" value="Clear"/>
            <input type="button" id="btCopyCss"  class="button" value="Copy"/>
        </div>

        <div class="spacer"></div>
        
        <div class="title_container">
            <h4 class="sub_title">Css Control</h4>
            <textarea id="csscfg" runat="server"></textarea>
        </div>

        <input id="selectedCssFile" type="hidden" value="" runat="server" />
    </form>
</body>
</html>
