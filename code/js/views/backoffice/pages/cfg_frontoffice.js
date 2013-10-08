// CodeMirror variables
var cm_script;
var cm_availabelOp;
var cm_css;

$(document).ready(function () {

    CodeMirrorInit();

    GetFrontOfficeAvailableOptions();

    GetFrontOfficeOptions();

    // Load css with ajax from css file
    $("#btCssLoad").click(function () {

        if (confirm("Load file ?")) {
            var cssFileName = $("#cssSelector").val();

            $("#selectedCssFile").val(cssFileName);
            GetCssFiles(cm_css, cssFileName, "frontoffice");
        }
    });

    // Clear the selected css
    $("#btClearCss").click(function () {

        if (confirm("Clear textboxs ?")) {
            $("#selectedCssFile").val("");
            cm_css.setValue("");
        }
    });

    // Copy the selected css
    $("#btCopyCss").click(function () {

        if (confirm("Copy file ?")) {

            var oldFileName = $("#selectedCssFile").val();

            if (oldFileName != "") {
                var newFileName = prompt("Please enter css file name please.");

                if (newFileName != null)
                    CopyCssFile(oldFileName, newFileName, "frontoffice");
            } else
                alert("You have to select existing css file and load it, then you can copy it.");
        }
    });
});

// set manualy width/height size of css text area
$(document).ready(function () { cm_css.setSize(getDocWidth(), getDocHeight() - (getDocHeight() / 2) + getDocHeight() * 0.10); });

// Initialize CodeMirror text area
function CodeMirrorInit() {

    cm_script = LoadCodeMirror('options', 'text/css', 'vibrant-ink', true);
        
    cm_availabelOp = LoadCodeMirror('availabelOp', 'text/css', 'vibrant-ink', true);
     
    cm_css = LoadCodeMirror('csscfg', 'text/css', 'vibrant-ink', true);

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

// load frontoffice available options
function GetFrontOfficeAvailableOptions() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'frontoffice', 'name':'configs', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            cm_availabelOp.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// load frontoffice options
function GetFrontOfficeOptions() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetFrontOfficeOption",
        data: "{'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            cm_script.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}