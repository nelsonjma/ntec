var pTemplateCfg; // codemirror variable

$(document).ready(function () {
    
    CodeMirrorInit(); // loads codemirror to textarea

    // Load template with ajax
    $("#btTemplateLoad").click(function () {

        if (confirm("Load file ?")) {
            var fileName = $("#templateSelector").val();

            $("#selectedTemplateFile").val(fileName);
            GetTemplateFiles(pTemplateCfg, fileName, $("#templateType").val());
        }
    });
    
    // Clear the selected template
    $("#btTemplateClear").click(function () {

        if (confirm("Clear textboxs ?")) {
            $("#selectedTemplateFile").val(""); // clean the hidden input box
            pTemplateCfg.setValue("");          // clean codemirror
        }
    });

    // Copy the selected template
    $("#btTemplateCopy").click(function () {

        if (confirm("Copy file ?")) {

            var oldFileName = $("#selectedTemplateFile").val();

            if (oldFileName != "") {
                var newFileName = prompt("Please enter new template file name please.");

                if (newFileName != null)
                    CopyTemplateFile(oldFileName, newFileName, $("#templateType").val());
            } else
                alert("You have to select existing template file and load it, then you can copy it.");
        }
    });

    $("#btTemplateDelete").click(function() {

        if (confirm("Delete file ?")) {

            DeleteTemplateFile($("#selectedTemplateFile").val(), $("#templateType").val());
        }

    });
    
});


// init codemirror in text area
function CodeMirrorInit() {
    var textAreaHeight = getDocHeight() - 125;

    pTemplateCfg = LoadCodeMirror('templatecfg', 'text/xml', 'vibrant-ink', false);
    pTemplateCfg.setSize("100%", textAreaHeight + "px");
}

// Read template files to show in textares
function GetTemplateFiles(container, filename, type) {
    $.ajax({
        type: "POST",
        url: "../bo_cmds.asmx/LoadTemplateFile",
        data: "{'fileName': '" + filename + "', 'templateType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
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
        url: "../bo_cmds.asmx/CopyTemplateFile",
        data: "{'oldFileName': '" + oldfilename + "', 'newFileName': '" + newfilename + "', 'templateType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
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
        url: "../bo_cmds.asmx/DeleteTemplateFile",
        data: "{'fileName': '" + filename + "', 'templateType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
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