<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_page.aspx.cs" Inherits="bo_pages_cfg_page" ValidateRequest="false"%>

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
    <script src="../../js/CodeMirror/mode/ntec.js"></script>
    
    <!-- Site  -->
    <link rel="stylesheet" href="../../css/views/backoffice/default.css"/>
    <link rel="stylesheet" href="../../css/views/backoffice/pages/cfg_page.css"/>
    
    
    <script src="../../js/views/backoffice/pages/cfg_page.js"></script>
    <script src="../../js/views/backoffice/generic.js"></script>
    <script src="../../js/generic.js"></script>

    <title>Page Configs</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
        
        <div class="title_container">
            <h4 id="pageTitle" runat="server" class="title">Page Configuration</h4>
        </div>

        <div class="header">
	        <asp:Button ID="btSaveP" runat="server" Text="Save" class="button" OnClick="Save_OnClick" OnClientClick="return confirm('Are you sure you want to save current page?');"/>
            <input type="button" id="btPreviewPage" value="Preview" class="button"/>
        </div>
        
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="cell">
                    <input id="p_id" type="hidden" runat="server"/>
                    <span class="label">Master Page: </span><select id="p_masterpageid" runat="server"></select>
                    <span class="label">Name: </span><input type="text" id="p_name" runat="server" class="textbox"/>
                    <span class="label">Title: </span><input type="text" id="p_title" runat="server" class="textbox"/>

                </div>
            </div>
        </div>
        
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="cell">
                    <span class="label">Xml/DataFile Folder: </span><input type="text" id="p_xml_folder" runat="server" class="textbox"/>
                    <span class="label">Xml Url: </span><input type="text" id="p_xml_url" runat="server" class="textbox"/>
                </div>
            </div>
        </div>
        
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="cell"><textarea id="p_options" runat="server"></textarea></div>
                <div class="cell"><textarea id="p_available_options" runat="server"></textarea></div>  
            </div>
        </div>
        
        <div class="spacer"></div>
        
        <div class="title_container">
            <h4 class="sub_title">Page Css Control</h4>
        </div>

        <div class="spacer"></div>

        <div class="header">
            <asp:DropDownList ID="cssSelector" runat="server" CssClass="dropdownbox"></asp:DropDownList>
            <input type="button" id="btCssLoad" value="Load" class="button"/>
            <input type="button" id="btClearCss" value="Clear" class="button"/>
            <input type="button" id="btCopyCss" value="Copy" class="button"/>
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
