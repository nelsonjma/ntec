<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_template_editor_css.aspx.cs" Inherits="bo_site_cfg_page_css" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />    

    <!-- JQuery -->
    <script src="../../js/jquery-1.9.0.js"></script>

    <!-- CodeMirror -->
    <link rel="stylesheet" href="../../js/CodeMirror/theme/vibrant-ink.css" />
    <link rel="stylesheet" href="../../js/CodeMirror/lib/codemirror.css"/>    

    <script src="../../js/CodeMirror/lib/codemirror.js"></script>
    <script src="../../js/CodeMirror/mode/css/css.js"></script>
    
    <!-- Site  -->
    <link rel="stylesheet" href="../../css/views/backoffice/default.css"/>
    <link rel="stylesheet" href="../../css/views/backoffice/site/cfg_template_editor.css"/>
    
    <script type="text/javascript" src="../../js/generic.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/generic.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/site/cfg_template_editor_css.js"></script>

    <title>Page Css Config</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
        <input id="cssTemplateType" type="hidden" runat="server" />

        <div class="title_container">
            <h4 id="cssTemplateTitle" runat="server" class="title"></h4>
        </div>

        <div class="spacer"></div>

        <div class="header">
            <asp:DropDownList ID="cssSelector" runat="server" CssClass="dropdownbox"></asp:DropDownList>
            <input type="button" id="btCssLoad" value="Load" class="button"/>
            <input type="button" id="btClearCss" value="Clear" class="button"/>
            <input type="button" id="btCopyCss" value="Copy" class="button"/>
            <input type="button" id="btDeleteCss" value="Delete" class="button"/>
            <asp:Button ID="btSaveCss" runat="server" Text="Save" class="button" OnClick="SaveCss_OnClick" OnClientClick="return confirm('Are you sure you want to save current css file?');"/>
        </div>
        
        <div class="spacer"></div>

        <div>
            <textarea id="csscfg" runat="server"></textarea>
        </div>

        <input id="selectedCssFile" type="hidden" value="" runat="server" />
    </div>
    </form>
</body>
</html>
