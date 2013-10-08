<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_user_configs.aspx.cs" Inherits="bo_site_cfg_user_configs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" /> 
    
    <!-- JQuery -->
    <script src="../../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link rel="stylesheet" href="../../css/views/backoffice/default.css" />
    <link rel="stylesheet" href="../../css/views/backoffice/site/cfg_user_configs.css"/>
    
    <script src="../../js/generic.js"></script>
    <script src="../../js/views/backoffice/generic.js"></script>
    <script src="../../js/views/backoffice/site/cfg_user_configs.js"></script>

    <title>User configuration Page</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
        <input id="userId" type="hidden" runat="server" />
        
        <div class="title_container">
            <h4 class="title">User Configurations</h4>
        </div>
        
        <div class="spacer"></div>
        
        <div class="title_container">
            <h4 class="sub_title title_warning_color">You have to logoff for changes to take efect</h4>
        </div>

        <div class="spacer"></div>
        
        <!-- Password -->
        <div class="title_container">
            <h4 class="sub_title">Change Pass</h4>
        </div>
        
        <div class="spacer"></div>
        
        <div class="table">
            <div class="row">
                <div class="cell">
                    <span>New Pass:</span>
                    <input type="password" class="textbox" id="new_pass"/> <input type="button" id="change_pass" class='button' value="Change"/>
                </div>
            </div>
        </div>
        
        <div class="spacer"></div>
        
        <!-- User Favorites -->
        <div class="title_container">
            <h4 class="sub_title">Favorites</h4>
        </div>

        <div class="table">
            <div class="row">
                <div class="cell">Availabel Pages</div>
                <div class="cell">Favorite Tab Pages</div>
            </div>
            <div class="row">
                <div class="cell">
                    <div id="available_pages"><table></table></div>
                </div>
                <div class="cell">
                    <div id="favorite_pages"><table></table></div>
                </div>
            </div>          
        </div>

        <div class="spacer"></div>
        
        <div class="title_container">
            <h4 class="sub_title">FrontOffice Default Page</h4>
        </div>

        <!-- FrontOffice Default Page -->
        <div class="table">
            <div class="row">
                <div class="cell">
                    <div id="default_page"><table></table></div>
                </div>
                <div class="cell">
                    <div id="selectedPage" runat="server">
                        <input type="hidden" id="selected_page_name"/>
                        <span>Frontoffice Default Page:</span><br/>
                        <input type="button" id="remove_selected_page" class='button' value="remove" />
                        <span id="selected_page_title"></span>
                    </div>
                </div>
            </div>
        </div>

    </div>
    </form>
</body>
</html>
