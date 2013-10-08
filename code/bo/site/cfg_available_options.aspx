<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_available_options.aspx.cs" Inherits="bo_cfg_available_options" ValidateRequest="false"%>

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
    <link rel="stylesheet" href="../../css/views/backoffice/site/cfg_available_options.css"/>
    
    <script src="../../js/views/backoffice/site/cfg_available_options.js"></script>
    <script src="../../js/views/backoffice/generic.js"></script>
    <script src="../../js/generic.js"></script>

    <title>Available Options Config</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
        
        <div class="title_container">
            <h4 class="title">Available Options Config</h4>
        </div>
        
        <div class="spacer"></div>

        <div id="op_selector" class="main">
            <div id="opcontainer">
                <table></table>
            </div>
        </div>
        
        <div class="header">
	        <asp:Button ID="btSaveOp" runat="server" Text="Save" class="button" OnClick="Save_OnClick" OnClientClick="return confirm('Are you sure you want to save current option?');" />
            <asp:Button ID="btNewOp" runat="server" Text="New" class="button" OnClick="New_OnClick" OnClientClick="return confirm('Are you sure you want to add new option?');" />
            <input type="button" id="btcleanop" value="Clear" class="button" />
        </div>
        
        <div id="op_edit" class="main">
            
            <div class="table">
                <div class="row">
                    <div class="cell">
                        <div class="spacer"></div>

                        <input id="op_id" type="hidden" runat="server"/>
                        <span class="label">Type: </span><input type="text" id="op_type" runat="server" class="textbox"/>
                        <span class="label">Name: </span><input type="text" id="op_name" runat="server" class="textbox"/>
                        <span class="label">Url: </span><input  type="text" id="op_url"  runat="server" class="textbox"/>

                        <div class="spacer"></div>
                    </div>
                </div>
                <div class="row">
                    <div class="cell">
                        <textarea id="op_options" runat="server"></textarea>
                    </div>
                </div>
            </div>

        </div>


    </div>
    </form>
</body>
</html>
