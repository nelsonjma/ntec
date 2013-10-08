var cssTemplateCfg; // codemirror variable

$(document).ready(function () {
    
    CodeMirrorInit(); // loads codemirror to textarea

    // Load template with ajax
    $("#btCssLoad").click(function () {

        if (confirm("Load file ?")) {
            var fileName = $("#cssSelector").val();

            $("#selectedCssFile").val(fileName);
            GetTemplateFiles(cssTemplateCfg, fileName, $("#cssTemplateType").val());
        }
    });
    
    // Clear the selected template
    $("#btClearCss").click(function () {

        if (confirm("Clear textboxs ?")) {
            $("#selectedCssFile").val(""); // clean the hidden input box
            cssTemplateCfg.setValue("");          // clean codemirror
        }
    });

    // Copy the selected template
    $("#btCopyCss").click(function () {

        if (confirm("Copy file ?")) {

            var oldFileName = $("#selectedCssFile").val();

            if (oldFileName != "") {
                var newFileName = prompt("Please enter new template file name please.");

                if (newFileName != null)
                    CopyTemplateFile(oldFileName, newFileName, $("#cssTemplateType").val());
            } else
                alert("You have to select existing template file and load it, then you can copy it.");
        }
    });

    $("#btDeleteCss").click(function () {

        if (confirm("Delete file ?")) {

            DeleteTemplateFile($("#selectedCssFile").val(), $("#cssTemplateType").val());
        }

    });
    
});


// init codemirror in text area
function CodeMirrorInit() {
    var textAreaHeight = getDocHeight() - 125;

    cssTemplateCfg = LoadCodeMirror('csscfg', 'text/x-scss', 'vibrant-ink', false);
    cssTemplateCfg.setSize("100%", textAreaHeight + "px");
}

// Read template files to show in textares
function GetTemplateFiles(container, filename, type) {
    $.ajax({
        type: "POST",
        url: "../bo_cmds.asmx/LoadCssFile",
        data: "{'fileName': '" + filename + "', 'cssType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            container.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// Ajax method that copy template file from server side
function CopyTemplateFile(oldfilename, newfilename, type) {
    $.ajax({
        type: "POST",
        url: "../bo_cmds.asmx/CopyCssFile",
        data: "{'oldFileName': '" + oldfilename + "', 'newFileName': '" + newfilename + "', 'cssType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            alert(data.d);

            if (data.d == "File Copied")
                location.href = location.href; // refresh;
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// Ajax method that deletes template files from server side
function DeleteTemplateFile(filename, type) {
    $.ajax({
        type: "POST",
        url: "../bo_cmds.asmx/DeleteCssFile",
        data: "{'fileName': '" + filename + "', 'cssType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            alert(data.d);
            
            if (data.d == "File Deleted")
                location.href = location.href; // refresh;
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}