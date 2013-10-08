<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_template_editor_xml.aspx.cs" Inherits="bo_site_cfg_template_editor" ValidateRequest="false"%>

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
    <script src="../../js/CodeMirror/mode/xml/xml.js"></script>

    <!-- Site  -->    
    <link rel="stylesheet" href="../../css/views/backoffice/default.css"/>
    <link rel="stylesheet" href="../../css/views/backoffice/site/cfg_template_editor.css"/>
    
    <script type="text/javascript" src="../../js/views/backoffice/site/cfg_template_editor_xml.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/generic.js"></script>
    <script type="text/javascript" src="../../js/generic.js"></script>

    <title>Templates Config</title>

</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
        <input id="templateType" type="hidden" runat="server" />
        
        <div class="title_container">
            <h4 id="templateTitle" runat="server" class="title"></h4>
        </div>

        <div class="spacer"></div>

        <div class="header">
            <asp:DropDownList ID="templateSelector" runat="server" CssClass="dropdownbox"></asp:DropDownList>
            <input type="button" id="btTemplateLoad" value="Load" class="button"/>
            <input type="button" id="btTemplateClear" value="Clear" class="button"/>
            <input type="button" id="btTemplateCopy" value="Copy" class="button"/>
            <input type="button" id="btTemplateDelete" value="Delete" class="button"/>
            <asp:Button ID="btTemplateSave" runat="server" Text="Save" class="button" OnClick="SaveTemplate_OnClick" OnClientClick="return confirm('Are you sure you want to save current template file?');"/>
        </div>
        
        <div class="spacer"></div>
        
        <div>
            <textarea id="templatecfg" runat="server"></textarea>
        </div>
        
        <input id="selectedTemplateFile" type="hidden" value="" runat="server" />

    </div>
    </form>
</body>
</html>
