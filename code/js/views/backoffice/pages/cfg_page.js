var pOptions;
var pAvailableOptions;
var pCssCfg;

$(document).ready(function () {
    CodeMirrorInit();

    GetPageOptions();
    
    // Load css with ajax from css file
    $("#btCssLoad").click(function () {

        if (confirm("Load file ?")) {
            var cssFileName = $("#cssSelector").val();

            $("#selectedCssFile").val(cssFileName);
            GetCssFiles(pCssCfg, cssFileName, "page");
        }
    });
    
    // Clear the selected css
    $("#btClearCss").click(function () {

        if (confirm("Clear textboxs ?")) {
            $("#selectedCssFile").val("");
            pCssCfg.setValue("");
        }
    });

    // Copy the selected css
    $("#btCopyCss").click(function () {

        if (confirm("Copy file ?")) {

            var oldFileName = $("#selectedCssFile").val();

            if (oldFileName != "") {
                var newFileName = prompt("Please enter css file name please.");

                if (newFileName != null)
                    CopyCssFile(oldFileName, newFileName, "page");
            } else
                alert("You have to select existing css file and load it, then you can copy it.");
        }
    });

    // Open page in new tab
    $("#btPreviewPage").click(function() {
        var pageName = $("#p_name").val();

        if (pageName != '')
            OpenInNewTab("../../page.aspx?nm=" + pageName);
        else
            alert("first you have to save a page then you can preview it.");
    });
});

// init codemirror in text area
function CodeMirrorInit() {
    var textAreaWidth = (getDocWidth() / 2) - 14;
    var textAreaHeight = 300;

    pOptions = LoadCodeMirror('p_options', 'text/x-ntec', 'vibrant-ink', true);
    pOptions.setSize(textAreaWidth + "px", textAreaHeight + "px");

    pAvailableOptions = LoadCodeMirror('p_available_options', 'text/x-ntec', 'vibrant-ink', false);
    pAvailableOptions.setSize(textAreaWidth + "px", textAreaHeight + "px");
    
    pCssCfg = LoadCodeMirror('csscfg', 'text/x-ntec', 'vibrant-ink', false);
    pCssCfg.setSize("100%", "400px");
}

// Ajax method that copy css files in server side
function CopyCssFile(oldfilename, newfilename, type) {
    $.ajax({
        type: "POST",
        url: "../bo_cmds.asmx/CopyCssFile",
        data: "{'oldFileName': '" + oldfilename + "', 'newFileName': '" + newfilename + "', 'cssType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data.d == "File Copied") {

                if (newfilename.indexOf(".css") == -1)
                    newfilename = newfilename + ".css";

                $("#cssSelector").append(new Option(newfilename, newfilename, false, false));                
            }
            
            alert(data.d);

        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// load master page available options
function GetPageOptions() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'page', 'name':'generic', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            pAvailableOptions.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}