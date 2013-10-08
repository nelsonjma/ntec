<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cfg_frame.aspx.cs" Inherits="bo_pages_cfg_frame" ValidateRequest="false" %>

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
    <link rel="stylesheet" href="../../css/views/backoffice/pages/cfg_frame.css"/>
    
    <script src="../../js/views/backoffice/generic.js"></script>
    <script src="../../js/views/backoffice/pages/cfg_frame.js"></script>
    <script src="../../js/generic.js"></script>
    
    <title>Frame Configs</title>
</head>
<body>
    <form id="mform" runat="server">
    <div>
        <input id="webServiceKey" type="hidden" runat="server" />
    
        <div class="title_container">
            <h4 id="page_title" class="title">Page Frames Configuration</h4>
        </div>

        <div id="vr_container" runat="server"><iframe></iframe></div>

        <div class="header">
	        <asp:Button ID="btSaveP" runat="server" Text="Save" class="button" OnClick="Save_OnClick" OnClientClick="return confirm('Are you sure you want to save current frame?');"/>
            <asp:Button ID="btClone" runat="server" Text="Clone" class="button" OnClick="Clone_OnClick" OnClientClick="return confirm('Are you sure you want to clone current frame?');"/>
            <asp:Button ID="btDelete" runat="server" Text="Delete" class="button" OnClick="Delete_OnClick" OnClientClick="return confirm('Are you sure you want to delete current frame?');"/>
            <input type="button" id="bt_clear" value="Clear" class="button"/>
            <input type="button" id="bt_preview" value="Preview" class="button"/>
            <input type="button" id="bt_vr_refresh" value="Refresh Viewer" class="button"/>
            <input type="text"   id="tbVrHeight" value="300px" runat="server" class="button"/>
            <input type="button" id="bt_vr_set_height" value="Set Height" class="button"/>
            <input type="button" id="bt_vr_showhide" value="Hide Viewer" class="button"/>
            <input type="button" id="bt_move_up" value="&uarr;" class="button"/>
            <input type="button" id="bt_move_down" value="&darr;" class="button"/>
        </div>
        
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="cell">
                    <input id="f_id" type="hidden" runat="server"/>
                    <span class="label">Is Active: </span>
                                                        <select id="f_isactive" runat="server" class="opselect">
                                                            <option value="0">no</option>                        
                                                            <option value="1">yes</option>
                                                        </select>
                    <span class="label">Page: </span><select id="f_pageid" runat="server" class="opselect"></select>
                    <span class="label">Title: </span><input type="text" id="f_title" runat="server" class="textbox"/>
                    
                </div>
            </div>
        </div>

        <div class="spacer"></div>
        <div class="spacer backspacer"></div>
        <div class="spacer"></div>
        
        <div class="table">
            <div class="row">
                <div class="cell">
                    <span class="label">X: </span><input type="text" id="f_x" runat="server" class="textbox"/>
                    <span class="label">Y: </span><input type="text" id="f_y" runat="server" class="textbox"/>
                    <span class="label">Width: </span><input type="text" id="f_width" runat="server" class="textbox"/>
                    <span class="label">Height: </span><input type="text" id="f_height" runat="server" class="textbox"/>
                </div>
            </div>
        </div>
        
        <div class="spacer"></div>
        <div class="spacer backspacer"></div>
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div class="cell">
                    <span class="label">Frame Type: </span><select id="f_type" runat="server" class="opselect"></select>
                    <span class="label">Scroll Bar: </span>
                                                            <select id="f_scroll" runat="server" class="opselect">
                                                                <option value="auto">auto</option>
                                                                <option value="yes">yes</option>
                                                                <option value="no">no</option>
                                                            </select>
                    <span class="label">Schedule Interval: </span><select id="f_schedule_interval" runat="server" class="opselect"></select>
                </div>
            </div>
        </div>
        
        <div class="spacer"></div>
        <div class="spacer backspacer"></div>
        <div class="spacer"></div>

        <div class="table">
            <div class="row">
                <div id="option_table" class="cell">

                    <div class="table">
                        <div class="row"><div class="cell cell_title">Options</div></div>
                        <div class="row"><div class="cell"><textarea id="f_options" runat="server"></textarea></div></div>
                    </div>

                </div>
                <div id="aval_table" class="cell">
                    
                    <div class="table">
                        <div class="row">
                            <div class="cell cell_title">Generic Options</div></div>
                        <div class="row">
                            <div class="cell"><textarea id="aval_generic_op" runat="server"></textarea></div>
                        </div>
                        <div class="row">
                            <div class="cell cell_title">Frame Type Options</div>
                        </div>
                        <div class="row">
                            <div class="cell"><textarea id="aval_type_op" runat="server"></textarea></div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

    </div>
    </form>
</body>
</html>
