<%@ Page Language="C#" AutoEventWireup="true" CodeFile="backoffice.aspx.cs" Inherits="bo_backoffice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    
    <!-- Logo -->
    <link rel="shortcut icon" href="favicon.ico" />

    <!-- JQuery -->
    <script src="../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <link href="../css/views/backoffice/backoffice.css" rel="stylesheet" />
    
    <script src="../js/generic.js"></script>
    <script type="text/javascript" src="../js/views/backoffice/backoffice.js"></script>
    <script src="../js/views/backoffice/page_tree_ctrl.js"></script>

    <title>Backoffice</title>
</head>
<body>
    <form id="mform" runat="server">
        
        <input id="webServiceKey" type="hidden" runat="server" />

        <div class="headerbar">
	        <ul class="navbar">
		        <li><a href="../frontoffice.aspx">Home</a></li>
		        
                
                <li><a>Site Configs</a>
                    <ul>
                        <li><a>FrontOffice</a>
                            <ul>
                                <li><a id="frontoffice_cfg" href="pages/cfg_frontoffice.aspx">Config</a></li>
                                <li><a id="frontoffice_css" href="site/cfg_template_editor_css.aspx?tmp=frontoffice">CSS</a></li>
                            </ul>
                        </li>
                        <li><a id="available_options_cfg" href="site/cfg_available_options.aspx">Available Options Config</a></li>
                        <li><a id="user_cfg" href="site/cfg_users.aspx">Users Config</a></li>
                        <li><a>Template</a>
                            <ul>
                                <li><a id="char_template" href="site/cfg_template_editor_xml.aspx?tmp=chart_template">Chart</a></li>
                                <li><a id="mon_template" href="site/cfg_template_editor_xml.aspx?tmp=mon_template">Mon</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>

                <li><a>Page Configs</a>
                    <ul>
                        <li><a>Css Config</a>
                            <ul>
                                <li><a id="page_css" href="site/cfg_template_editor_css.aspx?tmp=page">Page</a></li>
                                <li><a id="frames_css" href="site/cfg_template_editor_css.aspx?tmp=frames">Frames</a></li>
                            </ul>
                        </li>
                        <li><a>Add New</a>
                            <ul>
                                <li><a id="new_masterpage" href="pages/cfg_master_page.aspx">New Master Page</a></li>
                                <li><a id="new_page" href="pages/cfg_page.aspx">New Page</a></li>
                                <li><a id="new_frame" href="pages/cfg_frame.aspx">New Frame</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>
	        </ul>

            <div id="treeCtrl">
                <a>Hide Tree</a>
            </div>
        </div>

        <div id="container">
            <div id="row">
                <div id="left">
                    <div id="tree"></div>

                    <div id="treePageOpBox" class="treeOpBox">
                        <input type="hidden" id="treePageId"/>
                        <dl>
                            <dt><a href="#">Delete</a></dt>
                            <dt><a href="#">Clone</a></dt>
                            <dt><a href="#">Edit Page</a></dt>
                            <dt><a href="#">Edit Page Frames</a></dt>
                        </dl>
                    </div>

                    <div id="treeMasterPageOpBox" class="treeOpBox">
                        <input type="hidden" id="treeMasterPageId"/>
                        <dl>
                            <dt><a href="#">Delete</a></dt>
                            <dt><a href="#">Edit Master Page</a></dt>
                        </dl>
                    </div>
                </div>
                
                <div id="right">
                    <iframe id="objframe"></iframe>
                </div>
            </div>
        </div>

    </form>
</body>
</html>

