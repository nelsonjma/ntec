var mpOptions;
var mpAvailableOptions;

$(document).ready(function () {
    InitTextArea();

    GetMasterPageOption();
});


function InitTextArea() {
    var textAreaWidth = (getDocWidth() / 2) - 14;

    mpOptions = LoadCodeMirror('mp_options', 'text/x-ntec', 'vibrant-ink', true);
    mpOptions.setSize(textAreaWidth + "px", "450px");

    mpAvailableOptions = LoadCodeMirror('mp_available_options', 'text/x-ntec', 'vibrant-ink', false);
    mpAvailableOptions.setSize(textAreaWidth + "px", "450px");
}

// load master page available options
function GetMasterPageOption() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'master_page', 'name':'generic', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            mpAvailableOptions.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}