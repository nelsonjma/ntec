<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_users.aspx.cs" Inherits="bo_cfg_users" ValidateRequest="false"%>

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

    <!-- Site -->
    <link rel="stylesheet" href="../../css/views/backoffice/default.css" />
    <link rel="stylesheet" href="../../css/views/backoffice/site/cfg_users.css"/>
    
    <script type="text/javascript" src="../../js/generic.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/generic.js"></script>
    <script type="text/javascript" src="../../js/views/backoffice/site/cfg_users.js"></script>


    <title>Users Config</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
    
        <div class="title_container">
            <h4 class="title">User Config</h4>
        </div>
        
        <div class="spacer"></div>
        
        <!-- List of users -->
        <div id="user_selector">
            <div id="user_container"><table></table></div>
        </div>

        <div class="spacer"></div>

        <div class="title_container">
            <h4 class="sub_title">User config</h4>
        </div>

        <div class="spacer"></div>

        <div class="header">
            <asp:Button ID="btSaveOp" runat="server" Text="Save" class="button" OnClick="Save_OnClick" OnClientClick="return confirm('Are you sure you want to save current user?');" />
            <asp:Button ID="btNewOp" runat="server" Text="New" class="button" OnClick="New_OnClick" OnClientClick="return confirm('Are you sure you want to add new user? \n Pages will not be added now, you will have to add them later. first create user then add pages to him :)');" />
            <input type="button" id="btcleanop" value="Clear" class="button" />
            <input type="button" id="btClone" value="Clone" class="button" />
        </div>

        <div class="spacer"></div>
        
        <!-- User generic information -->
        <div class="table">
            <div class="row">
                <div class="cell"></div>
                <div class="cell"><span class="label">User:</span></div>
                <div class="cell"><span class="label">Pass:</span></div>
                <div class="cell"><span class="label">Is Admin:</span></div>
                <div class="cell"><span class="label">Description:</span></div>
            </div>
            <div class="row">
                <div class="cell"><input type="hidden" id="uid" runat="server"/></div>
                <div class="cell"><input type="text" id="uname" runat="server" class="textbox"/></div>
                <div class="cell"><input type="password" id="upass" runat="server" class="textbox"/></div>
                <div class="cell"><input type="checkbox" id="uadmin" runat="server" class="textbox"/></div>
                <div class="cell"><textarea id="udesc" runat="server"></textarea></div>
            </div>
        </div>

        <div class="spacer"></div>
    
        <!-- User config options ctrls  -->
        <div class="table">
            <div class="row">
                <div class="cell cell_title">User options</div>
                <div class="cell cell_title">Available User Options</div>
            </div>
            <div class="row">
                <div class="cell"><textarea id="u_options" runat="server"></textarea></div>
                <div class="cell"><textarea id="u_available_op"></textarea></div>

            </div>
        </div>
        
        <!-- Admin options ctrls  -->
        <div class="table">
            <div class="row">
                <div class="cell cell_title">Admin Options</div>
                <div class="cell cell_title">Available Admin Options</div>
            </div>
            <div class="row">
                <div class="cell"><textarea id="u_admin_op" runat="server"></textarea></div>
                <div class="cell"><textarea id="u_admin_available_op"></textarea></div>
            </div>
        </div>
        
        <div class="spacer"></div>

        <!-- Divider -->
        <div class="title_container">
            <h4 class="sub_title">Page Editor</h4>
        </div>
        
        <div class="spacer"></div>
        
        <!-- User Pages Ctrls -->
        <div class="table">
            <div class="row">
                <div class="cell cell_title">Available Pages</div>
                <div class="cell cell_title">Authenticated Pages</div>
            </div>
            <div class="row">
                <div class="cell">
                    <div id="availablepages_container">
                        <div id="availablepages"><table></table></div>
                    </div>
                </div>
                <div class="cell">
                    <div id="userpages_container">
                        <div id="userpages"><table></table></div>
                    </div>
                </div>
            </div>          
        </div>

    </div>
    </form>
</body>
</html>
