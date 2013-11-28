var fOptions;
var fAvalGenericOp;
var fAvalTypedOp;

var pageType;

var frameChanged;

$(document).ready(function () {
    frameChanged = false;


    RefreshFrameViewer(); // loads frames from pageid;

    SetDefaultViewerHeight(); // load default viewers height

    CodeMirrorInit(); // start codemirror

    GetFrameGenericOptions(); // load generic frame info to textarea

    // refresh all textbox
    $("#bt_clear").click(function () {
        ClearAllTextBoxs();
    });

    // change default iframe height
    $("#bt_vr_set_height").click(function () {

        SetDefaultViewerHeight();
    });

    // hide/show viewer
    $("#bt_vr_showhide").click(function () {

        var shViewer = $(this);

        if (shViewer.val() == "Hide Viewer") {
            shViewer.val("Show Viewer");

            $('iframe').css("height", "0");
        } else {
            shViewer.val("Hide Viewer");

            SetDefaultViewerHeight();
        }

    });

    // frame type selector change and load new frame option
    $("#f_type").change(function () {
        GetCurretFrameType($(this).val());
    });

    // refresh iframe
    $("#bt_vr_refresh").click(function () {

        ViewerRefresh();

    });

    // preview the page
    $("#bt_preview").click(function () {

        PreviewFrame($("#f_pageid").val());

    });

    // move frame up
    $("#bt_move_up").click(function() {

        var pageId = $("#f_pageid").val();
        var frameId = $("#f_id").val();
        var height = -1;

        if (parseInt(pageId) >= 0 && parseInt(frameId) >= 0) {
            if (pageType == "free_draw") {
                var auxHeight = prompt("Give height in px to move up", "0");

                if (auxHeight.indexOf("px") >= 0)
                    auxHeight = auxHeight.replace("px", " ");

                height = auxHeight;
            }

            MoveFrame(pageId, frameId, pageType, height > 0 ? height * -1 : height);
        } else
            alert("no frame or page selected");
    });
    
    // move frame down
    $("#bt_move_down").click(function () {

        var pageId = $("#f_pageid").val();
        var frameId = $("#f_id").val();
        var height = 1;

        if (parseInt(pageId) >= 0 && parseInt(frameId) >= 0) {
            if (pageType == "free_draw") {
                var auxHeight = prompt("Give height in px to move down", "0");

                if (auxHeight.indexOf("px") >= 0)
                    auxHeight = auxHeight.replace("px", " ");

                height = parseInt(auxHeight);
            }

            MoveFrame(pageId, frameId, pageType, height < 0 ? height * -1 : height);
        } else
            alert("no frame or page selected");
    });

    // change size of type available options text area
    fAvalTypedOp.on("dblclick", function () {

        if (fAvalTypedOp.getOption("lineNumbers") == false) {
            fAvalTypedOp.setOption("lineNumbers", true);

            fAvalTypedOp.setSize("1000px", "400px");
            ShrinkOptionBox();

        } else {
            fAvalTypedOp.setOption("lineNumbers", false);

            fAvalTypedOp.setSize("500px", "400px");
            
            RefreshOptionBox();
        }

    });

    // change size of generic available options text area
    fAvalGenericOp.on("dblclick", function () {

        if (fAvalGenericOp.getOption("lineNumbers") == false) {
            fAvalGenericOp.setOption("lineNumbers", true);

            fAvalGenericOp.setSize("1000px", "300px");
            ShrinkOptionBox();

        } else {
            fAvalGenericOp.setOption("lineNumbers", false);

            fAvalGenericOp.setSize("500px", "300px");
            RefreshOptionBox();
        }

    });
    

    /***************************** detect data changed *****************************/
    $("#f_width, #f_height, #f_x, #f_y, #f_title, #f_isactive, #f_pageid, #f_type, #f_scroll, #f_schedule_interval").bind("change paste keyup", function () {
        frameChanged = true;
    });

    fOptions.on('keyup', function (cMirror) {
        frameChanged = true;
    });

});

$(window).resize(function() {
    RefreshOptionBox();
});

// read tbVrHeight textbox value to set iframe height
function SetDefaultViewerHeight() {
    var viewerHeight = $('#tbVrHeight').val();
    if (viewerHeight.indexOf == -1) viewerHeight = viewerHeight + "px";

    $('iframe').css("height", viewerHeight);
}

// CodeMirror TextArea Init
function CodeMirrorInit() {

    fOptions = LoadCodeMirror('f_options', 'text/x-ntec', 'vibrant-ink', true);
    RefreshOptionBox(); // set fOption width
   
    fAvalGenericOp = LoadCodeMirror('aval_generic_op', 'text/x-ntec', 'vibrant-ink', false);
    fAvalGenericOp.setSize("500px", "300px");

    fAvalTypedOp = LoadCodeMirror('aval_type_op', 'text/x-ntec', 'vibrant-ink', false);
    fAvalTypedOp.setSize("500px", "400px");
}

// load generic options into textarea
function GetFrameGenericOptions() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'frame', 'name':'generic', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            
            var strData = RemoveSpecialChars(data.d.toString());

            fAvalGenericOp.setValue(strData);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// gets the frame type available options to put in the textarea
function GetCurretFrameType(ftype) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'frame', 'name':'" + ftype + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var strData = RemoveSpecialChars(data.d.toString());

            fAvalTypedOp.setValue(strData);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// get frame from server side
function GetFrame(frameId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetFrame",
        data: "{'frameId':'" + frameId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            var id = jsonData.ID;
            var pageId = jsonData.PageId;
            var title = jsonData.Title;
            var x = jsonData.X;
            var y = jsonData.Y;
            var width = jsonData.Width;
            var height = jsonData.Height;
            var fType = jsonData.FrameType;
            var scroll = jsonData.Scroll;
            var options = jsonData.Options;
            var isActive = jsonData.IsActive;
            var interval = jsonData.ScheduleInterval;

            $("#f_id").val(id);
            $("#f_title").val(title);

            $("#f_x").val(x);
            $("#f_y").val(y);
            $("#f_width").val(width);
            $("#f_height").val(height);

            fOptions.setValue(options);

            $("#f_pageid option").each(function (i, item) {
                if (item.value == pageId) {
                    $("#f_pageid").prop("selectedIndex", i);
                }
            });

            $("#f_isactive option").each(function (i, item) {
                if (item.value == isActive) {
                    $("#f_isactive").prop("selectedIndex", i);
                }
            });

            $("#f_scroll option:contains('" + scroll + "')").prop('selected', true);
            $("#f_type option:contains('" + fType + "')").prop('selected', true);
            $("#f_schedule_interval option:contains('" + interval + "')").prop('selected', true);

            GetCurretFrameType(fType);

        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

function GetPageType(pageId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetPageType",
        data: "{'pageId':'" + pageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            if (data.d == "free_draw")
                $("iframe").attr("src", "page_frame/free_style_frame_view.aspx?pageid=" + pageId);
            else
                $("iframe").attr("src", "page_frame/table_frame_view.aspx?pageid=" + pageId);

            pageType = data.d;
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// change selected frame
function SelectFrame(id) {

    var frameId = $("#f_id").val();
    frameId = frameId == '' ? -1 : frameId;

    if (frameId < 0) {

        GetFrame(id); // loads frame
        
    } else if (frameId != -1) {

        if (frameChanged && confirm("Save current frame ?")) {
            document.getElementById('btSaveP').click();
        } else {
            GetFrame(id); // loads frame
        }
        
        frameChanged = false;
    }
}

// change x,y coordinates from frame, its used for free draw page type
function ChangeCoordOfFrame(x, y) {
    $("#f_x").val(x.replace('px', ''));
    $("#f_y").val(y.replace('px', ''));
}

// reades the url variable pageid and builds the structure of the iframe
function RefreshFrameViewer() {

    var pageid = GetURLParameter("pageid");

    if (parseInt(pageid) >= 0) {

        $("iframe").css("display", "block");
        
        GetPageType(pageid);
        BuildPageTitle(pageid);

        // set the page dropdown to this page position
        $('#f_pageid').val(pageid);
    }

}

// clear all text boxs
function ClearAllTextBoxs() {
    $("#f_id").val("");
    $("#f_title").val("");

    $("#f_x").val("0");
    $("#f_y").val("0");
    $("#f_width").val("0");
    $("#f_height").val("0");

    fOptions.setValue("");

    $("#f_pageid").prop("selectedIndex", 0);
    $("#f_isactive").prop("selectedIndex", 1);

    $("#f_scroll").prop("selectedIndex", 0);
    $("#f_type").prop("selectedIndex", 0);
    $("#f_schedule_interval").prop("selectedIndex", 0);

}

// refresh viewer
function ViewerRefresh() {
    $('iframe').attr("src", $('iframe').attr("src"));
}

// open page in new table
function PreviewFrame(pageId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetPageName",
        data: "{'pageId':'" + pageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            OpenInNewTab("../../page.aspx?nm=" + data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

function MoveFrame(pageId, frameId, pageType, height) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/MoveFrame",
        data: "{'pageId':'" + pageId + "', 'frameId':'" + frameId + "', 'pageType':'" + pageType + "', 'height':" + height + ", 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);

            try {
                GetFrame(frameId);
                ViewerRefresh();
            } catch (e) {
                alert(e.message);
            }

        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// option box will grow to mutch in some frames, this method will try to shrink it
function RefreshOptionBox() {

    //var winWidth = getDocWidth();     // this sucks
    var winWidth = $(window).width();   // this is better for now

    fOptions.setSize(winWidth - 510 + "px", "725px");
}

// set option box very small
function ShrinkOptionBox() {

    //var winWidth = getDocWidth();     // this sucks
    var winWidth = $(window).width();   // this is better for now

    fOptions.setSize(winWidth - 1010 + "px", "725px");
}

// get the title from server side and puts it the title box
function BuildPageTitle(pageId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetPageTitle",
        data: "{'pageId':'" + pageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            $('#page_title').text(data.d + " Frames Configuration");
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}