<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_master_page.aspx.cs" Inherits="bo_pages_cfg_master_page" ValidateRequest="false"%>

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
    <link rel="stylesheet" href="../../css/views/backoffice/pages/cfg_master_page.css"/>

    <script src="../../js/views/backoffice/pages/cfg_master_page.js"></script>
    <script src="../../js/views/backoffice/generic.js"></script>
    <script src="../../js/generic.js"></script>

    <title>Master Page Config</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
        
        <div class="title_container">
            <h4 id="masterPageTitle" runat="server" class="title">Master Page Configuration</h4>
        </div>

        <div class="spacer"></div>

        <div class="header">
	        <asp:Button ID="btSaveOp" runat="server" Text="Save" class="button" OnClick="Save_OnClick" OnClientClick="return confirm('Are you sure you want to save current master page?');"/>
        </div>
        
        <div class="table">
            <div class="row">
                <div class="cell cell_title">Title</div>
                <div class="cell cell_title">Description</div>
            </div>
            <div class="row">
                <div class="cell">
                    <input id="mp_id" type="hidden" runat="server"/>
                    <input type="text" id="mp_title" runat="server" class="textbox"/>
                </div>
                <div class="cell"><textarea id="mp_desc" runat="server"></textarea></div>

            </div>
        </div>
        
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="cell cell_title">Options</div>
                <div class="cell cell_title">Available Options</div>
            </div>
            <div class="row">
                <div class="cell">
                    <textarea id="mp_options" runat="server"></textarea>
                </div>
                <div class="cell">
                    <textarea id="mp_available_options" runat="server"></textarea>
                </div>  
            </div>
        </div>

    </div>
    </form>
</body>
</html>
